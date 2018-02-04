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
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using Apache.Hive.Service.Rpc.Thrift;

namespace Airlock.Hive.ThriftClient
{
    /// <summary>
    /// Executes a single Hive statement, and provides methods for fetching
    /// the results in batches, and the schema of the results.
    /// </summary>
    public class HiveThriftStatementExecutor : IDisposable
    {
        private readonly TSessionHandle session;
        private readonly TCLIService.Client client;
        private TOperationHandle operation;
        private TTableSchema lastSchema;
        private bool isDisposed;

        public HiveThriftStatementExecutor(TSessionHandle session, TCLIService.Client client)
        {
            this.session = session;
            this.client = client;
        }

        public void Execute(string statement)
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(HiveThriftStatementExecutor));

            if (operation != null)
                throw new InvalidOperationException("A statement has already been executed.");

            var execReq = new TExecuteStatementReq
            {
                SessionHandle = session,
                Statement = statement,
            };
            lastSchema = null;
            var execResp = client.ExecuteStatementAsync(execReq, CancellationToken.None).Result;
            execResp.Status.ThrowIfError();
            operation = execResp.OperationHandle;
        }

        public TRowSet Fetch(int count = int.MaxValue)
        {
            if (operation == null || !operation.HasResultSet)
                throw new InvalidOperationException($"{nameof(Fetch)} can only be called when an operation with a result set has been executed.");

            var req = new TFetchResultsReq
            {
                MaxRows = count,
                Orientation = TFetchOrientation.FETCH_NEXT,
                OperationHandle = operation,
            };
            var resultsResp = client.FetchResultsAsync(req, CancellationToken.None).Result;
            resultsResp.Status.ThrowIfError();
            return resultsResp.Results;
        }

        public TTableSchema GetSchema()
        {
            if (operation == null || !operation.HasResultSet)
                throw new InvalidOperationException($"{nameof(GetSchema)} can only be called when an operation with a result set has been executed.");

            if (lastSchema == null)
            {
                var req = new TGetResultSetMetadataReq(operation);
                var resp = client.GetResultSetMetadataAsync(req, CancellationToken.None).Result;
                resp.Status.ThrowIfError();
                lastSchema = resp.Schema;
            }

            return lastSchema;
        }

        public void Dispose()
        {
            if (operation != null)
            {
                TCloseOperationReq closeReq = new TCloseOperationReq();
                closeReq.OperationHandle = operation;
                TCloseOperationResp closeOperationResp = client.CloseOperationAsync(closeReq, CancellationToken.None).Result;
                closeOperationResp.Status.ThrowIfError();
                operation = null;
            }

            isDisposed = true;
        }
    }
}
