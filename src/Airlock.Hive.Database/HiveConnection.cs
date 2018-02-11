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
using System.Data;
using System.Data.Common;
using System.Text;
using Airlock.Hive.ThriftClient;
using Airlock.Hive.ThriftClient.ThriftConnection;

namespace Airlock.Hive.Database
{
    public class HiveConnection : DbConnection, IDbConnection
    {
        private readonly string connectionString;
        private readonly ThriftConnectionFactory thriftConnectionFactory;

        private HiveThriftConnection thriftConnection;

        private string database;

        private ConnectionState state;

        public override string ConnectionString
        {
            get => connectionString;
            set => throw new NotSupportedException();
        }

        public override int ConnectionTimeout { get; }

        public override string Database => database;

        public override string DataSource { get; }

        public override string ServerVersion { get; } = "1";

        public override ConnectionState State => state;

        public HiveConnection(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null.");

            var cs = new HiveConnectionString(connectionString);
            this.connectionString = connectionString;
            database = cs.Database;
            DataSource = cs.Host;
            thriftConnectionFactory = new SaslConnectionFactory(cs.Host, cs.Port, cs.UserName, cs.Password);
        }

        public HiveConnection(string host, int port, string username, string password)
        {
            thriftConnection = new HiveThriftConnection(new SaslConnectionFactory(host, port, username, password));
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotSupportedException("Transactions are not supported.");
        }

        IDbTransaction IDbConnection.BeginTransaction()
        {
            throw new NotSupportedException("Transactions are not supported.");
        }

        IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il)
        {
            throw new NotSupportedException("Transactions are not supported.");
        }

        public override void ChangeDatabase(string databaseName)
        {
            using (var executor = thriftConnection.CreateStatementExecutor())
            {
                executor.Execute($"USE {databaseName}");
            }
        }

        public override void Open()
        {
            if (thriftConnection != null)
                throw new InvalidOperationException("Connection is already open.");

            state = ConnectionState.Connecting;
            thriftConnection = new HiveThriftConnection(thriftConnectionFactory);
            thriftConnection.Open();

            if (!string.IsNullOrEmpty(Database))
            {
                ChangeDatabase(Database);
            }

            state = ConnectionState.Open;
        }

        public override void Close()
        {
            thriftConnection.Close();
            thriftConnection.Dispose();
            thriftConnection = null;
            state = ConnectionState.Closed;
        }

        IDbCommand IDbConnection.CreateCommand()
        {
            return CreateDbCommand();
        }

        protected override DbCommand CreateDbCommand()
        {
            return new HiveCommand(thriftConnection.CreateStatementExecutor());
        }

        public new void Dispose()
        {
            thriftConnection.Dispose();
        }
    }
}
