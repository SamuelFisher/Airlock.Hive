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
using Apache.Hive.Service.Rpc.Thrift;

namespace Airlock.Hive.Database
{
    static class ThriftTypeToClrType
    {
        public static Type GetClrType(TTypeDesc typeDesc)
        {
            switch (typeDesc.Types[0].PrimitiveEntry.Type)
            {
                case TTypeId.INT_TYPE:
                    return typeof(int);
                case TTypeId.BIGINT_TYPE:
                    return typeof(long);
                case TTypeId.STRING_TYPE:
                    return typeof(string);
                case TTypeId.BOOLEAN_TYPE:
                    return typeof(bool);
                case TTypeId.DOUBLE_TYPE:
                    return typeof(double);
                case TTypeId.NULL_TYPE:
                    return null;
                case TTypeId.TIMESTAMP_TYPE:
                    return typeof(DateTime);
                default:
                    throw new NotSupportedException($"Type not supported: {typeDesc}");
            }
        }

        public static object GetClrObject(this ColumnBuffer column, int row, TTypeDesc typeDesc)
        {
            if (!typeDesc.Types[0].__isset.primitiveEntry)
                throw new NotSupportedException("Non-primitive types are not supported.");

            switch (typeDesc.Types[0].PrimitiveEntry.Type)
            {
                case TTypeId.INT_TYPE:
                    return column.IntValue(row);
                case TTypeId.BIGINT_TYPE:
                    return column.LongValue(row);
                case TTypeId.STRING_TYPE:
                    return column.StringValue(row);
                case TTypeId.BOOLEAN_TYPE:
                    return column.BoolValue(row);
                case TTypeId.DOUBLE_TYPE:
                    return column.DoubleValue(row);
                case TTypeId.NULL_TYPE:
                    return null;
                case TTypeId.TIMESTAMP_TYPE:
                    return DateTime.Parse(column.StringValue(row));
                default:
                    throw new NotSupportedException($"Type not supported: {typeDesc}");
            }
        }
    }
}
