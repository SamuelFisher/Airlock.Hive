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
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Thrift.Transports;

namespace Airlock.Hive.ThriftClient.ThriftConnection
{
    public abstract class ThriftConnectionFactory
    {
        internal abstract TClientTransport CreateTransport();

        protected IPAddress ResolveHost(string hostname)
        {
            if (IPAddress.TryParse(hostname, out IPAddress address))
                return address;

            var addresses = Dns.GetHostEntry(hostname).AddressList;
            var ipv4 = addresses.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            if (ipv4 != null)
                return ipv4;
            return addresses.First();
        }
    }
}
