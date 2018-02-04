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
using System.Text.RegularExpressions;
using System.Web;

namespace Airlock.Hive.Database
{
    /// <summary>
    /// TODO: Make this compatible with JDBC Hive connection strings.
    /// </summary>
    class HiveConnectionString
    {
        public HiveConnectionString(string connectionString)
        {
            var uri = new Uri(connectionString);

            Host = uri.Host;
            Port = uri.Port;
            Database = uri.AbsolutePath.Substring(1);

            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            UserName = queryParams.Get("username");
            Password = queryParams.Get("password");

            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }

        public string Host { get; }

        public int Port { get; }

        public string Database { get; }

        public string UserName { get; }

        public string Password { get; }
    }
}
