﻿// Copyright (C) 2018  Samuel Fisher
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
using System.Text;
using NUnit.Framework;

namespace Airlock.Hive.Database.FunctionalTest
{
    [TestFixture(Category = "Functional")]
    public class HiveConnectionTest
    {
        private HiveConnection connection;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            var connectionString = TestContext.Parameters["ConnectionString"] ??
                                   Environment.GetEnvironmentVariable("TEST_HIVE_CONNECTION_STRING");
            connection = new HiveConnection(connectionString);
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            connection.Dispose();
        }

        [Test, Order(1)]
        public void OpenConnection()
        {
            Assert.DoesNotThrow(() => connection.Open());
            Assert.That(connection.State, Is.EqualTo(ConnectionState.Open));
        }
    }
}
