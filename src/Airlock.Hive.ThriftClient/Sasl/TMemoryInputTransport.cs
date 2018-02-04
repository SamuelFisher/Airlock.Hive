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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Transports;

namespace Airlock.Hive.ThriftClient.Sasl
{
    class TMemoryInputTransport : TClientTransport
    {
        private byte[] buffer;
        private int pos;
        private int endPos;

        public TMemoryInputTransport()
        {
        }

        public TMemoryInputTransport(byte[] buf)
        {
            Reset(buf);
        }

        public TMemoryInputTransport(byte[] buf, int offset, int length)
        {
            Reset(buf, offset, length);
        }

        public override bool IsOpen => true;

        public int GetBufferPosition => pos;

        public int GetBytesRemainingInBuffer => endPos - pos;

        public override Task OpenAsync()
        {
            return Task.CompletedTask;
        }

        public override Task OpenAsync(CancellationToken ct)
        {
            return Task.CompletedTask;
        }

        public override Task<int> ReadAsync(byte[] buf, int offset, int length)
        {
            return ReadAsync(buf, offset, length, CancellationToken.None);
        }

        public override Task<int> ReadAsync(byte[] buf, int offset, int length, CancellationToken cancellationToken)
        {
            int bytesRemaining = GetBytesRemainingInBuffer;
            int amtToRead = length > bytesRemaining ? bytesRemaining : length;
            if (amtToRead > 0)
            {
                Array.Copy(buffer, pos, buf, offset, amtToRead);
                ConsumeBuffer(amtToRead);
            }

            return Task.FromResult(amtToRead);
        }

        public override Task WriteAsync(byte[] buf, int offset, int length)
        {
            return WriteAsync(buf, offset, length, CancellationToken.None);
        }

        public override Task WriteAsync(byte[] buf, int offset, int length, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public byte[] GetBuffer()
        {
            return buffer;
        }

        public void ConsumeBuffer(int len)
        {
            pos += len;
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Reset(byte[] buf)
        {
            Reset(buf, 0, buf.Length);
        }

        public void Reset(byte[] buf, int offset, int length)
        {
            buffer = buf;
            pos = offset;
            endPos = offset + length;
        }

        public void Clear()
        {
            buffer = null;
        }

        public override void Close()
        {
        }

        protected override void Dispose(bool disposing)
        {
            Dispose();
        }
    }
}
