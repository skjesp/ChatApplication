using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Sockets;
using System.Text;
using System.Net;

namespace WebApplication1.Pages
{
    public class ChatClientModel : PageModel
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //private static string chatSession = string.Empty;
        private static string name = string.Empty;
        public string ClientStatus;

        public List<string> ChatSession = new List<string>();

        public void WriteToChat()
        {
            ChatSession.Add("item1");
            ChatSession.Add("item2");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ChatSession.Add("item1");
            ChatSession.Add("item2");

            //LoopConnect();
            return null; //RedirectToPage("/Index");
        }

        private async Task SendLoop()
        {
            while (true)
            {
                //Console.Write("Enter chatmessage: ");
                //string req = name + " says: " + Console.ReadLine();
                string req = "testing from client";
                byte[] buffer = Encoding.ASCII.GetBytes(req);
                _clientSocket.Send(buffer);

                byte[] receivedBuf = new byte[1024];
                int rec = _clientSocket.Receive(receivedBuf);
                byte[] data = new byte[rec];
                Array.Copy(receivedBuf, data, rec);
                
                ChatSession.Add(Encoding.ASCII.GetString(data));
                //Console.WriteLine("Support says: " + Encoding.ASCII.GetString(data));
            }
        }

        private void LoopConnect()
        {
            int attempts = 0;

            while (!_clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    _clientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    //do nothing
                    //Console.Clear();
                    ClientStatus = "Connection attempts: " + attempts.ToString();
                    //Console.WriteLine("Connection attempts: " + attempts.ToString());
                }
            }
            ClientStatus = "Connected";
            //Console.Clear();
            //Console.WriteLine("Connected");
        }

    }
}



//        /// <summary>
//		/// The PORT.
//		/// </summary>
//		const int PORT = 9000;
//        /// <summary>
//        /// The BUFSIZE.
//        /// </summary>
//        const int BUFSIZE = 1000;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="file_client"/> class.
//        /// </summary>
//        /// <param name='args'>
//        /// The command-line arguments. First ip-adress of the server. Second the filename
//        /// </param>
//        private void file_client(string[] args)
//        {
//            //Create TCP client
//            TcpClient client = new TcpClient();

//            //Prompt User for IP-Adress
//            string ipAddress;
//            Console.Write("Enter server hosting address: (192.38.33.18)");
//            ipAddress = Console.ReadLine();


//            client.Connect(ipAddress, PORT);
//            Console.WriteLine("{0} has been connected", args[0]);

//            //Request clientstream
//            NetworkStream server = client.GetStream();
//            Console.WriteLine("Networkstream created");

//            //Console.WriteLine("'{0}' Filename requested",args[1]);
//            string File_Name = args[1];

//            LIB.writeTextTCP(server, File_Name);
//            Console.WriteLine("Text to server has been send.");
//            long File_Size = LIB.getFileSizeTCP(server);
//            Console.WriteLine("Filesize is: " + File_Size);

//            //User requests a specific filename for server to search
//            while (File_Size == 0)
//            {
//                Console.WriteLine("File not found");

//                //Prompt user for filename 
//                File_Name = Console.ReadLine();
//                Console.WriteLine("Requesting this file {0}", File_Name);

//                //Write to server
//                LIB.writeTextTCP(server, File_Name);
//                File_Size = LIB.getFileSizeTCP(server);
//            }
//            //file found...
//            Console.WriteLine("Size of file: {0}", File_Size);
//            receiveFile(File_Name, server, File_Size);
//        }

//        /// <summary>
//        /// Receives the file.
//        /// </summary>
//        /// <param name='fileName'>
//        /// File name.
//        /// </param>
//        /// <param name='io'>
//        /// Network stream for reading from the server
//        /// </param>
//        private void receiveFile(String fileName, NetworkStream io, long fileSize)
//        {
//            fileName = LIB.extractFileName(fileName);

//            //Create folder
//            string data_directory = "/root/Desktop/Destination/";
//            Directory.CreateDirectory(data_directory);

//            //
//            FileStream file = new FileStream(data_directory + fileName, FileMode.Create, FileAccess.Write);
//            byte[] data = new byte[BUFSIZE];

//            int bytesTotal = 0;
//            int bytesRead;

//            Console.WriteLine("Reading File: {0}", fileName);

//            //Read filename
//            while (fileSize > bytesTotal)
//            {
//                bytesRead = io.Read(data, 0, data.Length);
//                file.Write(data, 0, bytesRead);

//                bytesTotal += bytesRead;

//                Console.WriteLine("Bytes read: " + bytesRead.ToString() + "\tTotal bytes read: " + bytesTotal);
//            }
//            Console.WriteLine("File Received");
//        }

//    }
//}