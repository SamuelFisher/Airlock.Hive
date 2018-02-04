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
using System.Text;
using Airlock.Hive.ThriftClient.Sasl;
using Thrift.Transports;
using Thrift.Transports.Client;

namespace Airlock.Hive.ThriftClient
{
    public class SaslConnectionFactory : ThriftConnectionFactory
    {
        public SaslConnectionFactory(string host,
                                     int port,
                                     string username,
                                     string password)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
        }

        public string Host { get; }

        public int Port { get; }

        public string Username { get; }

        public string Password { get; }

        internal override TClientTransport CreateTransport()
        {
            var ipAddress = IPAddress.Parse(Host); // Dns.GetHostEntry(Host).AddressList.First();
            var socket = new TSocketClientTransport(ipAddress, Port);
            return new TSaslClientTransport(socket, Username, Password);
        }
    }
}
