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
using NUnit.Framework;

namespace Airlock.Hive.Database.Test
{
    [TestFixture]
    public class ColumnBufferTest
    {
        private readonly byte[] nulls = {4};

        [Test]
        public void Test_Bool()
        {
            var column = new TColumn
            {
                BoolVal = new TBoolColumn(new List<bool>
                {
                    true,
                    false,
                    false
                }, nulls)
            };

            var cb = new ColumnBuffer(column);

            Assert.That(cb.BoolValue(0), Is.True);
            Assert.That(cb.IsNull(0), Is.False);
            Assert.That(cb.BoolValue(1), Is.False);
            Assert.That(cb.IsNull(1), Is.False);
            Assert.That(cb.IsNull(2), Is.True);
        }
    }
}
