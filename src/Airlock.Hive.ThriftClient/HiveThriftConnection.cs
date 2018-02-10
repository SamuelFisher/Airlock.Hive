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
using Apache.Hive.Service.Rpc.Thrift;
using Thrift.Protocols;
using Thrift.Transports;

namespace Airlock.Hive.ThriftClient
{
    /// <summary>
    /// Connects to a Hive Thrift server for executing queries.
    /// </summary>
    public class HiveThriftConnection : IDisposable
    {
        private readonly TProtocolVersion version;

        private TClientTransport transport;
        private TCLIService.Client client;
        private TSessionHandle session;

        public HiveThriftConnection(ThriftConnectionFactory thriftConnectionFactory,
                                    TProtocolVersion version = TProtocolVersion.HIVE_CLI_SERVICE_PROTOCOL_V7)
        {
            transport = thriftConnectionFactory.CreateTransport();
            var protocol = new TBinaryProtocol(transport);
            client = new TCLIService.Client(protocol);
            this.version = version;
        }

        private TSessionHandle GetSession()
        {
            var openReq = new TOpenSessionReq(version);
            var openResp = client.OpenSessionAsync(openReq, CancellationToken.None).Result;
            openResp.Status.ThrowIfError();
            return openResp.SessionHandle;
        }

        public void Open()
        {
            if (transport != null && !transport.IsOpen)
                transport.OpenAsync().Wait();
            if (session == null)
                session = GetSession();
        }

        public void Close()
        {
            if (session != null)
                CloseSession();
            if (transport != null && transport.IsOpen)
                transport.Close();
        }

        private void CloseSession()
        {
            var closeSessionReq = new TCloseSessionReq
            {
                SessionHandle = session
            };
            var closeSessionResp = client.CloseSessionAsync(closeSessionReq, CancellationToken.None).Result;
            closeSessionResp.Status.ThrowIfError();
            session = null;
        }

        public HiveThriftStatementExecutor CreateStatementExecutor()
        {
            Open();
            return new HiveThriftStatementExecutor(session, client);
        }

        public void Dispose()
        {
            Close();
            client = null;
            transport = null;
            session = null;
        }
    }
}
