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
using System.Data.Common;
using System.Data.Odbc;
using System.Text;
using Airlock.Hive.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace Airlock.EntityFrameworkCore.Hive.Storage.Internal
{
    class HiveRelationalConnection : RelationalConnection
    {
        public HiveRelationalConnection(RelationalConnectionDependencies dependencies)
            : base(dependencies)
        {
        }

        protected override DbConnection CreateDbConnection()
        {
            if (ConnectionString.StartsWith("hive2://"))
            {
                return new HiveConnection(ConnectionString);
            }
            else
            {
                return new OdbcConnection(ConnectionString) {ConnectionTimeout = 30000};
            }
        }
    }
}
