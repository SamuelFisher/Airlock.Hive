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

namespace Airlock.Hive.Database
{
    public class HiveCommand : DbCommand, IDbCommand
    {
        private readonly HiveThriftStatementExecutor statementExecutor;

        private int batchSize = 1000;

        internal HiveCommand(HiveThriftStatementExecutor statementExecutor)
        {
            this.statementExecutor = statementExecutor;
        }

        public override string CommandText { get; set; }

        public override int CommandTimeout { get; set; }

        public override CommandType CommandType { get; set; }

        public override UpdateRowSource UpdatedRowSource { get; set; }

        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection { get; } = new HiveParameterCollection();

        protected override DbTransaction DbTransaction { get; set; }

        public override bool DesignTimeVisible { get; set; }

        public int BatchSize
        {
            get => batchSize;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Batch size must be greater than zero.");
                batchSize = value;
            }
        }

        public override void Cancel()
        {
            throw new NotSupportedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            statementExecutor.Execute(CommandText);
            return 0;
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            statementExecutor.Execute(CommandText);
            return new HiveDataReader(statementExecutor, BatchSize);
        }

        IDataReader IDbCommand.ExecuteReader()
        {
            statementExecutor.Execute(CommandText);
            return new HiveDataReader(statementExecutor, BatchSize);
        }

        IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)
        {
            return ExecuteReader();
        }

        public override object ExecuteScalar()
        {
            var reader = ExecuteReader();
            reader.Read();
            return reader.GetValue(0);
        }

        public override void Prepare()
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            statementExecutor.Dispose();
        }
    }
}
