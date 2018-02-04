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
using System.Linq;
using Airlock.EntityFrameworkCore.Hive;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ExampleApp
{
    class Program
    {
        public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] {new ConsoleLoggerProvider((_, __) => true, true)});

        static void Main(string[] args)
        {
            var connectionString = args[0];

            using (var context = new EstateAgentContext(new DbContextOptionsBuilder<EstateAgentContext>().UseHive(connectionString).UseLoggerFactory(MyLoggerFactory).Options))
            {
                var viewings = from p in context.Properties
                               select new {p.ShortAddress};
                Console.WriteLine(viewings.First().ShortAddress);
            }
        }
    }
}
