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
    static class EndianEncoding
    {
        public static void EncodeBigEndian(int integer, byte[] buf)
        {
            EncodeBigEndian(integer, buf, 0);
        }

        public static void EncodeBigEndian(int integer, byte[] buf, int offset)
        {
            buf[offset] = (byte)(0xff & (integer >> 24));
            buf[offset + 1] = (byte)(0xff & (integer >> 16));
            buf[offset + 2] = (byte)(0xff & (integer >> 8));
            buf[offset + 3] = (byte)(0xff & (integer));
        }

        public static int DecodeBigEndianInt32(byte[] buf)
        {
            return DecodeBigEndianInt32(buf, 0);
        }

        public static int DecodeBigEndianInt32(byte[] buf, int offset)
        {
            return ((buf[offset] & 0xff) << 24) |
                   ((buf[offset + 1] & 0xff) << 16) |
                   ((buf[offset + 2] & 0xff) << 8) |
                   ((buf[offset + 3] & 0xff));
        }
    }
}
