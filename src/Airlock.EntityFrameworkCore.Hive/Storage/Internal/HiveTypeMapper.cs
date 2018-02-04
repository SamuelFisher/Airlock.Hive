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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Airlock.EntityFrameworkCore.Hive.Storage.Internal
{
    public class HiveTypeMapper : RelationalTypeMapper
    {
        public HiveTypeMapper(RelationalTypeMapperDependencies dependencies) : base(dependencies)
        {
        }

        protected override IReadOnlyDictionary<Type, RelationalTypeMapping> GetClrTypeMappings()
        {
            return new Dictionary<Type, RelationalTypeMapping>
            {
                [typeof(string)] = new StringTypeMapping("STRING"),
                [typeof(int)] = new IntTypeMapping("INT"),
                [typeof(long)] = new LongTypeMapping("BIGINT"),
                [typeof(double)] = new DoubleTypeMapping("DOUBLE"),
                [typeof(DateTime)] = new DateTimeTypeMapping("TIMESTAMP"),
                [typeof(bool)] = new BoolTypeMapping("BOOLEAN")
            };
        }

        protected override IReadOnlyDictionary<string, RelationalTypeMapping> GetStoreTypeMappings()
        {
            throw new NotImplementedException();
        }

        protected override string GetColumnType(IProperty property) => property.Relational().ColumnType;
    }
}
