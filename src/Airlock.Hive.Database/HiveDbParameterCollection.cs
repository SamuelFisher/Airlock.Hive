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
using System.Data.Common;
using System.Text;

namespace Airlock.Hive.Database
{
    class HiveDbParameterCollection : DbParameterCollection
    {
        private readonly IDictionary<string, HiveDbParameter> members = new Dictionary<string, HiveDbParameter>();
        
        public override int Add(object value)
        {
            if (!(value is HiveDbParameter parameter))
                throw new ArgumentException($"Parameter must be a {nameof(HiveDbParameter)}.", nameof(parameter));

            members.Add(parameter.ParameterName, parameter);
            return 0;
        }

        public override void Clear()
        {
            // Do nothing
        }

        public override bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public override void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            throw new NotImplementedException();
        }

        public override int Count { get; }

        public override object SyncRoot { get; }

        public override int IndexOf(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(string value)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            return new string[0].GetEnumerator();
        }

        protected override DbParameter GetParameter(int index)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return members[parameterName];
        }

        public override void AddRange(Array values)
        {
            throw new NotImplementedException();
        }
    }
}
