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

namespace Airlock.Hive.ThriftClient.Sasl
{
    public class SaslNegotiator
    {
        private readonly PlainMechanism mechanism;

        public SaslNegotiator(PlainMechanism mechanism)
        {
            this.mechanism = mechanism;
            MechanismName = mechanism.Name;
        }

        public string MechanismName { get; }

        public byte[] RespondToChallenge(byte[] challenge)
        {
            return mechanism.RespondToChallenge(challenge);
        }
    }

    public class PlainMechanism
    {
        private readonly byte sign = 0x00;

        private readonly string userName;

        private readonly string password;

        public PlainMechanism(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        public string Name => "PLAIN";

        public byte[] RespondToChallenge(byte[] challenge)
        {
            var result = new List<byte>();

            result.Add(sign);
            result.AddRange(Encoding.UTF8.GetBytes(userName));
            result.Add(sign);
            result.AddRange(Encoding.UTF8.GetBytes(password));

            return result.ToArray();
        }
    }
}
