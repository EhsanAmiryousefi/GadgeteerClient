/* Copyright 2011 Marco Minerva, marco.minerva@gmail.com

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using Microsoft.SPOT;
using System.Net;
using System.Net.Sockets;

namespace ServerExample
{
    public class DataReceivedEventArgs : EventArgs
    {
        public EndPoint LocalEndPoint { get; private set; }
        public EndPoint RemoteEndPoint { get; private set; }
        public byte[] Data { get; private set; }
        public bool Close { get; set; }
        public byte[] ResponseData { get; set; }

        public DataReceivedEventArgs(EndPoint localEndPoint, EndPoint remoteEndPoint, byte[] data)
        {
            LocalEndPoint = localEndPoint;
            RemoteEndPoint = remoteEndPoint;
            if (data != null)
            {
                Data = new byte[data.Length];
                data.CopyTo(Data, 0);
            }
        }
    }
}
