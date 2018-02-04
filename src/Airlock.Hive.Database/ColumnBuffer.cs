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
using System.Text;
using Apache.Hive.Service.Rpc.Thrift;

namespace Airlock.Hive.Database
{
    class ColumnBuffer
    {
        private readonly List<byte[]> binaryColumn;
        private readonly List<bool> boolColumn;
        private readonly List<sbyte> byteColumn;
        private readonly List<double> doubleColumn;
        private readonly List<short> shortColumn;
        private readonly List<int> intColumn;
        private readonly List<long> longColumn;
        private readonly List<string> stringColumn;
        private readonly Func<int, object> anyColumn;
        private readonly BitArray nulls;

        public ColumnBuffer(TColumn column)
        {
            if (column.__isset.binaryVal)
            {
                binaryColumn = column.BinaryVal.Values;
                anyColumn = BinaryValue;
                Length = binaryColumn.Count;
                nulls = new BitArray(column.BinaryVal.Nulls);
            }
            else if (column.__isset.boolVal)
            {
                boolColumn = column.BoolVal.Values;
                anyColumn = row => BoolValue(row);
                Length = boolColumn.Count;
                nulls = new BitArray(column.BoolVal.Nulls);
            }
            else if (column.__isset.byteVal)
            {
                byteColumn = column.ByteVal.Values;
                anyColumn = row => ByteValue(row);
                Length = byteColumn.Count;
                nulls = new BitArray(column.ByteVal.Nulls);
            }
            else if (column.__isset.doubleVal)
            {
                doubleColumn = column.DoubleVal.Values;
                anyColumn = row => DoubleValue(row);
                Length = doubleColumn.Count;
                nulls = new BitArray(column.DoubleVal.Nulls);
            }
            else if (column.__isset.i16Val)
            {
                shortColumn = column.I16Val.Values;
                anyColumn = row => ShortValue(row);
                Length = shortColumn.Count;
                nulls = new BitArray(column.I16Val.Nulls);
                ;
            }
            else if (column.__isset.i32Val)
            {
                intColumn = column.I32Val.Values;
                anyColumn = row => IntValue(row);
                Length = intColumn.Count;
                nulls = new BitArray(column.I32Val.Nulls);
            }
            else if (column.__isset.i64Val)
            {
                longColumn = column.I64Val.Values;
                anyColumn = row => LongValue(row);
                Length = longColumn.Count;
                nulls = new BitArray(column.I64Val.Nulls);
            }
            else if (column.__isset.stringVal)
            {
                stringColumn = column.StringVal.Values;
                anyColumn = StringValue;
                Length = stringColumn.Count;
                nulls = new BitArray(column.StringVal.Nulls);
            }
        }

        public int Length { get; }

        public byte[] BinaryValue(int row) => binaryColumn[row];

        public bool BoolValue(int row) => boolColumn[row];

        public sbyte ByteValue(int row) => byteColumn[row];

        public double DoubleValue(int row) => doubleColumn[row];

        public short ShortValue(int row) => shortColumn[row];

        public int IntValue(int row) => intColumn[row];

        public long LongValue(int row) => longColumn[row];

        public string StringValue(int row) => stringColumn[row];

        public object ObjectValue(int row) => anyColumn(row);

        public bool IsNull(int row) => nulls[row];
    }
}
