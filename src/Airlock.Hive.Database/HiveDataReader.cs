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
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Airlock.Hive.ThriftClient;

namespace Airlock.Hive.Database
{
    public class HiveDataReader : DbDataReader, IDataReader
    {
        private readonly HiveThriftStatementExecutor statementExecutor;
        private readonly int batchSize;

        private ColumnBuffer[] results;
        private int rowIndex = -1;
        private bool noMoreRows;
        private bool isClosed;
        private Dictionary<string, int> columnPositions;

        public HiveDataReader(HiveThriftStatementExecutor statementExecutor, int batchSize)
        {
            this.statementExecutor = statementExecutor;
            this.batchSize = batchSize;
        }

        /// <inheritdoc />
        public override int FieldCount => results.Length;

        public override bool HasRows { get; }

        /// <inheritdoc />
        public override bool IsClosed => isClosed;

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/>.
        /// </summary>
        public override int Depth => throw new NotSupportedException();

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/>.
        /// </summary>
        public override int RecordsAffected => -1;

        /// <inheritdoc />
        public override bool GetBoolean(int i)
        {
            return results[i].BoolValue(rowIndex);
        }

        /// <inheritdoc />
        public override byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime GetDateTime(int i)
        {
            if (IsDBNull(i))
                throw new InvalidOperationException("Cannot convert NULL to DateTime.");

            var dateTimeString = results[i].StringValue(rowIndex);
            return DateTime.Parse(dateTimeString);
        }

        /// <inheritdoc />
        public override decimal GetDecimal(int i)
        {
            return Convert.ToDecimal(GetValue(i));
        }

        /// <inheritdoc />
        public override double GetDouble(int i)
        {
            return Convert.ToDouble(GetValue(i));
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Type GetFieldType(int i)
        {
            var typeDesc = statementExecutor.GetSchema().Columns[i].TypeDesc;
            return ThriftTypeToClrType.GetClrType(typeDesc);
        }

        /// <inheritdoc />
        public override float GetFloat(int i)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/>.
        /// </summary>
        public override Guid GetGuid(int i)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override short GetInt16(int i)
        {
            return Convert.ToInt16(GetValue(i));
        }

        /// <inheritdoc />
        public override int GetInt32(int i)
        {
            return Convert.ToInt32(GetValue(i));
        }

        /// <inheritdoc />
        public override long GetInt64(int i)
        {
            return Convert.ToInt64(GetValue(i));
        }

        /// <inheritdoc />
        public override string GetName(int i)
        {
            return statementExecutor.GetSchema().Columns[i].ColumnName;
        }

        public override int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public override string GetString(int i)
        {
            return results[i].StringValue(rowIndex);
        }

        public override object GetValue(int i)
        {
            if (IsDBNull(i))
                return null;

            var type = statementExecutor.GetSchema().Columns[i].TypeDesc;
            return results[i].GetClrObject(rowIndex, type);
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int i)
        {
            return results[i].IsNull(rowIndex);
        }

        public override object this[int i] => GetValue(i);

        public override object this[string name]
        {
            get
            {
                int columnPosition = GetColumnPosition(name);
                return ((IDataReader)this)[columnPosition];
            }
        }

        private int GetColumnPosition(string columnName)
        {
            if (columnPositions == null)
            {
                columnPositions = statementExecutor.GetSchema().Columns.ToDictionary(x => x.ColumnName, x => x.Position - 1);
            }

            return columnPositions[columnName];
        }

        public override DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public override bool NextResult()
        {
            throw new NotSupportedException();
        }

        public override bool Read()
        {
            rowIndex++;
            if (rowIndex < results?[0].Length)
                return true; // Already fetched next row

            if (rowIndex == results?[0].Length && noMoreRows)
                return false; // No more rows

            // Fetch more rows
            var rowSet = statementExecutor.Fetch(batchSize);
            results = rowSet.Columns.Select(x => new ColumnBuffer(x)).ToArray();

            // Reset row index
            rowIndex = 0;

            // Further fetch calls will not produce more rows if
            // we fetched less than the batch size this time.
            noMoreRows = results[0].Length < batchSize;
            return results.Length > 0;
        }

        public override void Close()
        {
            Dispose();
        }

        protected override void Dispose(bool disposiing)
        {
            isClosed = true;
            statementExecutor.Dispose();
        }
    }
}
