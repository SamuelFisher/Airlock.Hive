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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airlock.Hive.Database;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Airlock.EntityFrameworkCore.Hive.FunctionalTest
{
    [TestFixture(Category = "Functional")]
    public class EntityFrameworkTest
    {
        private static readonly string ConnectionString = TestContext.Parameters["ConnectionString"] ??
                                                          Environment.GetEnvironmentVariable("TEST_HIVE_CONNECTION_STRING");

        private static readonly string EFConnectionString = TestContext.Parameters["EFConnectionString"] ??
                                                            Environment.GetEnvironmentVariable("TEST_EF_HIVE_CONNECTION_STRING");

        private FunctionalTestContext context;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            Seed();


            var options = new DbContextOptionsBuilder<FunctionalTestContext>().UseHive(EFConnectionString).Options;
            context = new FunctionalTestContext(options);
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            context.Dispose();

            using (var connection = new HiveConnection(ConnectionString))
            {
                connection.Open();

                var dropDb = connection.CreateCommand();
                dropDb.CommandText = "DROP DATABASE ef_test CASCADE";
                dropDb.ExecuteNonQuery();
                dropDb.Dispose();
            }
        }

        [Test, Order(1)]
        public void Count()
        {
            var count = context.TestEntities.Count();
            Assert.That(count, Is.EqualTo(1));
        }

        [Test, Order(2)]
        public void Select()
        {
            var rows = context.TestEntities.ToList();

            Assert.That(rows, Has.Count.EqualTo(1));

            var row = rows.Single();
            Assert.That(row.IntProperty, Is.EqualTo(1));
            Assert.That(row.StringProperty, Is.EqualTo("Test"));
            Assert.That(row.DoubleProperty, Is.EqualTo(1.0));
            Assert.That(row.TimestampProperty, Is.EqualTo(new DateTime(2018, 1, 1, 0, 1, 2)));
            Assert.That(row.BooleanProperty, Is.True);
        }

        private void Seed()
        {
            using (var connection = new HiveConnection(ConnectionString))
            {
                connection.Open();

                var dropIfExists = connection.CreateCommand();
                dropIfExists.CommandText = "DROP DATABASE IF EXISTS ef_test CASCADE";
                dropIfExists.ExecuteNonQuery();
                dropIfExists.Dispose();

                var createDb = connection.CreateCommand();
                createDb.CommandText = "CREATE DATABASE ef_test";
                createDb.ExecuteNonQuery();
                createDb.Dispose();

                var useDatabase = connection.CreateCommand();
                useDatabase.CommandText = "USE ef_test";
                useDatabase.ExecuteNonQuery();
                useDatabase.Dispose();

                var createTable = connection.CreateCommand();
                createTable.CommandText = @"CREATE TABLE TestEntities (
IntProperty INT,
StringProperty STRING,
DoubleProperty DOUBLE,
TimestampProperty TIMESTAMP,
BooleanProperty BOOLEAN
)";
                createTable.ExecuteNonQuery();
                createTable.Dispose();

                var insert = connection.CreateCommand();
                insert.CommandText = @"INSERT INTO TABLE TestEntities
SELECT
  1 as IntProperty,
  'Test' as StringProperty,
  1.0 as DoubleProperty,
  '2018-01-01 00:01:02' as TimestampProperty,
  true as BooleanProperty
FROM (SELECT 1 as x) x";
                insert.ExecuteNonQuery();
                insert.Dispose();
            }
        }
    }

    public class FunctionalTestContext : DbContext
    {
        public FunctionalTestContext(DbContextOptions<FunctionalTestContext> contextOptions)
            : base(contextOptions)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }
    }

    public class TestEntity
    {
        [Key]
        public int IntProperty { get; set; }

        public string StringProperty { get; set; }

        public double DoubleProperty { get; set; }

        public DateTime TimestampProperty { get; set; }

        public bool BooleanProperty { get; set; }
    }
}
