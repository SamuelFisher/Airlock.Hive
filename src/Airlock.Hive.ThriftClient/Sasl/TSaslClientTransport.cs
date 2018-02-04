// Copyright (C) 2018  Samuel Fisher
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Transports;
using Thrift.Transports.Client;
using static Airlock.Hive.ThriftClient.Sasl.EndianEncoding;

namespace Airlock.Hive.ThriftClient.Sasl
{
    /// <summary>
    /// Wraps an ITransport, performing SASL negotiation when Open() is called.
    /// </summary>
    public class TSaslClientTransport : TClientTransport
    {
        private const int StatusBytes = 1;
        private const int PayloadLengthBytes = 4;
        private const int MessageHeaderLength = StatusBytes + PayloadLengthBytes;

        private readonly SaslNegotiator saslNegotiator;
        private readonly TSocketClientTransport socket;
        private readonly MemoryStream writeBuffer = new MemoryStream();
        private readonly TMemoryInputTransport readBuffer = new TMemoryInputTransport();

        private bool isOpen;

        public TSaslClientTransport(TSocketClientTransport socket, string userName, string password)
        {
            saslNegotiator = new SaslNegotiator(new PlainMechanism(userName, password));
            this.socket = socket;
        }

        public override bool IsOpen => isOpen;

        public override Task OpenAsync()
        {
            return OpenAsync(CancellationToken.None);
        }

        public override async Task OpenAsync(CancellationToken cancellationToken)
        {
            if (!IsOpen)
            {
                await socket.OpenAsync(cancellationToken);
                SendSaslMessage(SaslStatus.Start, saslNegotiator.MechanismName);
                SendSaslMessage(SaslStatus.Ok, saslNegotiator.RespondToChallenge(null));

                while (true)
                {
                    var result = ReceiveSaslMessage();
                    if (result.Status == SaslStatus.Complete)
                    {
                        isOpen = true;
                        break;
                    }
                    else if (result.Status == SaslStatus.Ok)
                    {
                        SendSaslMessage(SaslStatus.Ok, saslNegotiator.RespondToChallenge(Encoding.UTF8.GetBytes(result.Body)));
                    }
                    else
                    {
                        socket.Close();
                        throw new ProtocolViolationException($"Bad SASL negotiation status: {result.Status} ({result.Body})");
                    }
                }
            }
        }

        public override void Close()
        {
            socket.Close();
            isOpen = false;
        }

        public void SendSaslMessage(SaslStatus status, string body)
        {
            SendSaslMessage(status, Encoding.UTF8.GetBytes(body));
        }

        public void SendSaslMessage(SaslStatus status, byte[] body)
        {
            var header = new byte[MessageHeaderLength];
            header[0] = (byte)status;
            EncodeBigEndian(body.Length, header, StatusBytes);
            socket.WriteAsync(header).Wait();
            socket.WriteAsync(body).Wait();
            socket.FlushAsync().Wait();
        }

        public SaslMessage ReceiveSaslMessage()
        {
            var result = new SaslMessage();
            var header = new byte[MessageHeaderLength];
            socket.ReadAllAsync(header, 0, header.Length).Wait();
            result.Status = (SaslStatus)header[0];
            byte[] body = new byte[DecodeBigEndianInt32(header, StatusBytes)];
            socket.ReadAllAsync(body, 0, body.Length).Wait();

            result.Body = Encoding.UTF8.GetString(body);
            return result;
        }

        public int ReadLength()
        {
            byte[] lenBuf = new byte[4];
            socket.ReadAllAsync(lenBuf, 0, lenBuf.Length).Wait();
            return DecodeBigEndianInt32(lenBuf);
        }

        public void WriteLength(int length)
        {
            byte[] lenBuf = new byte[4];
            EncodeBigEndian(length, lenBuf);
            socket.WriteAsync(lenBuf).Wait();
        }

        public override Task<int> ReadAsync(byte[] buf, int off, int len)
        {
            return ReadAsync(buf, off, len, CancellationToken.None);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int length, CancellationToken cancellationToken)
        {
            int readLength = await readBuffer.ReadAsync(buffer, offset, length);
            if (readLength > 0)
                return readLength;

            await ReadFrame();

            return await readBuffer.ReadAsync(buffer, offset, length);
        }

        private async Task ReadFrame()
        {
            int dataLength = ReadLength();
            if (dataLength < 0)
                throw new TTransportException($"Read a negative frame size ({dataLength}).");

            byte[] buff = new byte[dataLength];
            await socket.ReadAllAsync(buff, 0, dataLength);
            readBuffer.Reset(buff);
        }

        public override Task WriteAsync(byte[] buf, int off, int len)
        {
            return WriteAsync(buf, off, len, CancellationToken.None);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int length, CancellationToken cancellationToken)
        {
            writeBuffer.Write(buffer, offset, length);
            return Task.CompletedTask;
        }

        public override Task FlushAsync()
        {
            return FlushAsync(CancellationToken.None);
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            byte[] data = writeBuffer.ToArray();
            // Reset write buffer
            writeBuffer.SetLength(0);
            WriteLength(data.Length);
            await socket.WriteAsync(data, 0, data.Length, cancellationToken);
            await socket.FlushAsync(cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            socket.Close();
        }
    }
}
