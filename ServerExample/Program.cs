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
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace ServerExample
{
    public partial class Program
    {
        void ProgramStarted()
        {
            ethernet.NetworkUp += new GTM.Module.NetworkModule.NetworkEventHandler(ethernet_NetworkUp);
            ethernet.NetworkDown += new GTM.Module.NetworkModule.NetworkEventHandler(ethernet_NetworkDown);
            ethernet.UseDHCP();

            SetupWindow();

            Debug.Print("Program Started");
        }

        private Text txtAddress;
        private Text txtReceivedMessage;

        private void SetupWindow()
        {
            var window = display.WPFWindow;
            var baseFont = Resources.GetFont(Resources.FontResources.NinaB);

            Canvas canvas = new Canvas();
            window.Child = canvas;

            txtAddress = new Text(baseFont, "Loading, please wait...");
            canvas.Children.Add(txtAddress);
            Canvas.SetTop(txtAddress, 50);
            Canvas.SetLeft(txtAddress, 30);

            txtReceivedMessage = new Text(baseFont, string.Empty);
            txtReceivedMessage.Width = 300;
            txtReceivedMessage.TextWrap = true;
            canvas.Children.Add(txtReceivedMessage);
            Canvas.SetTop(txtReceivedMessage, 100);
            Canvas.SetLeft(txtReceivedMessage, 10);
        }

        private void ethernet_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network Down!");
        }

        private void ethernet_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network Up!");

            txtAddress.TextContent = "IP Address: " + ethernet.NetworkSettings.IPAddress + ", port 8080";
            
            SocketServer server = new SocketServer(8080);
            server.DataReceived += new DataReceivedEventHandler(server_DataReceived);
            server.Start();
        }

        private void server_DataReceived(object sender, DataReceivedEventArgs e)
        {
            string receivedMessage = BytesToString(e.Data);
            txtReceivedMessage.Dispatcher.BeginInvoke(
                delegate(object arg) { 
                    txtReceivedMessage.TextContent = "Received message: " + arg.ToString(); 
                    return null; 
                }, 
                receivedMessage);
                        
            string response = "Response from server for the request '" + receivedMessage + "'";
            e.ResponseData = System.Text.Encoding.UTF8.GetBytes(response);

            if (receivedMessage == "close")
                e.Close = true;
        }

        private string BytesToString(byte[] bytes)
        {
            string str = string.Empty;
            for (int i = 0; i < bytes.Length; ++i)
                str += (char)bytes[i];

            return str;
        }
    }
}
