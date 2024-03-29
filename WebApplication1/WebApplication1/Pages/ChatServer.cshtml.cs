﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace WebApplication1.Pages
{
   
    public class ChatServerModel : PageModel
    {
        public string ChatContent { get; set; }
        public string ServerStatus { get; set; }

        //benyt bindproperty fra HTML variabler til CS.


        public async Task<IActionResult> OnPostAsync()
        {
            SetupServer();
            return null; //RedirectToPage("/Index");
        }


        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Internetwork således at serveren fungerer på IPv4
        private static List<Socket> _clientSockets = new List<Socket>(); //En liste over alle tilsluttede client sockets
        private const int BUFFER_SIZE = 1024;
        private const int PORT = 100;
        private static byte[] _buffer = new byte[1024];
        private static string chatSession = string.Empty;
        private static bool messageReceived = false;

        private void SetupServer()
        {
            //Setting up server
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));

            ServerStatus = "Server is now activated";
            //Sætter socket i et lytterstadie. Har blokerende effekt indtil at klient kommunikerer.
            
            _serverSocket.Listen(2); //Har en kø op til 2.

            //Accepterer en klient der vil kommunikere.
            _serverSocket.BeginAccept(AcceptCallback, null); //Hvis første parameter ikke var async, ville vi ikke kunne gå videre. Returnerer IAsyncResult.
        }

        private void AcceptCallback(IAsyncResult AR) //Bemærk at AR er resultat fra  beginAccept. todo: asp.net task, asynchron call, await. AR == returværdien for beginAccept
        {
            Socket clientSocket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(clientSocket);
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket); //Eksekverer kun ReceiveCallback når operationen er færdig.

            ServerStatus = "Client Connected";

            _serverSocket.BeginAccept(AcceptCallback, null);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = socket.EndReceive(AR);
            }
            catch (SocketException)
            {
                ServerStatus = "Client was forefully disconnected";
                //Console.WriteLine("Client was forcefully disconnected");
                socket.Close();
                return;
            }

            byte[] dataBuf = new byte[received];
            Array.Copy(_buffer, dataBuf, received);
            string text = Encoding.ASCII.GetString(dataBuf);
            //Console.WriteLine(text);
            ChatContent = text;
            string response = string.Empty;

            response = Console.ReadLine();

            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);

            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, socket);
        }

        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;

            socket.EndSend(AR);

            int numberOfSockets = 0;
            foreach (Socket clientSocket in _clientSockets)
            {
                numberOfSockets++;
            }
            //Console.WriteLine("Number of connected clients: " + numberOfSockets);
        }

        public void OnGet()
        {

        }
    }
}