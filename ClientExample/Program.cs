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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace ClientExample
{
    class Program
    {
        const string SERVER_IP = "130.83.109.156";
        const int SERVER_PORT = 8080;

        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();
            Console.Write("Connecting... ");

            client.Connect(SERVER_IP, SERVER_PORT);
            Console.WriteLine("Connected\n");

            using (Stream stream = client.GetStream())
            {
                while (true)
                {
                    Console.Write("Enter a string and press ENTER (empty string to exit): ");

                    string message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message))
                        break;

                    byte[] data = Encoding.Default.GetBytes(message);
                    Console.WriteLine("Sending... ");

                    stream.Write(data, 0, data.Length);

                    byte[] response = new byte[4096];
                    int bytesRead = stream.Read(response, 0, response.Length);
                    Console.WriteLine("Response: " + Encoding.Default.GetString(response, 0, bytesRead));

                    Console.WriteLine();
                }
            }

            client.Close();
        }
    }
}
