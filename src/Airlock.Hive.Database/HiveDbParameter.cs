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

namespace Airlock.Hive.Database
{
    public class HiveDbParameter : DbParameter
    {
        public override DbType DbType { get; set; }

        public override ParameterDirection Direction { get; set; }

        public override bool IsNullable { get; set; }

        public override string ParameterName { get; set; }

        public override string SourceColumn { get; set; }

        public override object Value { get; set; }

        public override bool SourceColumnNullMapping { get; set; }

        public override int Size { get; set; }

        public override void ResetDbType()
        {
            throw new NotSupportedException();
        }

        public string ToEscapedSql()
        {
            switch (DbType)
            {
                case DbType.String:
                case DbType.AnsiString:
                {
                    var value = ((string)Value).Replace("\\", "\\\\").Replace("'", "\\'");
                    return $"'{value}'";
                }
                default:
                    throw new NotSupportedException($"Parameters of type {DbType} are not supported.");
            }
        }
    }
}
