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

namespace Airlock.Hive.Database
{
    internal static class HivePreparedStatement
    {
        internal static string PrepareStatement(string sql, HiveDbParameterCollection parameters)
        {
            var parts = SplitSqlStatement(sql);

            var newSql = new StringBuilder();
            foreach (var part in parts)
            {
                if (part.StartsWith("@"))
                    newSql.Append(((HiveDbParameter)parameters[part]).ToEscapedSql());
                else
                    newSql.Append(part);
            }

            return newSql.ToString();
        }

        private static IList<String> SplitSqlStatement(string sql)
        {
            var parts = new List<string>();
            int apCount = 0;
            int off = 0;
            var skip = false;

            for (int i = 0; i < sql.Length; i++)
            {
                char c = sql[i];
                if (skip)
                {
                    skip = false;
                    continue;
                }

                switch (c)
                {
                    case '\'':
                        apCount++;
                        break;
                    case '\\':
                        skip = true;
                        break;
                    case '@':
                        if ((apCount & 1) == 0)
                        {
                            parts.Add(sql.Substring(off, i - off));
                            var parameterName = new StringBuilder(sql[i].ToString());
                            i++;
                            while (i < sql.Length && IsPararameterNameCharacter(sql[i]))
                                parameterName.Append(sql[i++]);
                            parts.Add(parameterName.ToString());
                            off = i;
                        }
                        break;
                }
            }

            parts.Add(sql.Substring(off));
            return parts;
        }

        private static bool IsPararameterNameCharacter(char c)
        {
            return Regex.IsMatch(c.ToString(), "[_0-9a-zA-Z]");
        }
    }
}
