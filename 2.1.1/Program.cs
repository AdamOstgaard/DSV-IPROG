using System;
using System.Net;
using System.Threading.Tasks;

namespace IPROG.Uppgifter.uppg2_1_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize with "sane defaults"
            var port = 2000;
            var ip = "127.0.0.1";

            // Parse arguments
            switch (args.Length)
            {
                case 1:
                    ip = args[0];
                    break;
                case 2:
                {
                    if (!int.TryParse(args[1], out port) || port > 65535)
                    {
                        Console.WriteLine("Invalid port, defaulting to 2000");
                    }

                    break;
                }
            }

            if (!IPAddress.TryParse(ip, out var remoteAddress))
            {
                Console.WriteLine("Invalid IP!");
                return;
            }

            Start(remoteAddress, port);
        }

        /// <summary>
        /// Start the chat
        /// </summary>
        /// <param name="remoteAddress">Server address to connect to</param>
        /// <param name="port">Port of the server</param>
        private static void Start(IPAddress remoteAddress, int port)
        { 
            try
            {
                var client = new ChatClient(remoteAddress, port);
                var chat = new ChatClientInterface();

                client.Connect();
                client.StartListening();

                chat.AttachInput(client);
                chat.StartAcceptInput();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Could not connect to server {remoteAddress}. \n" +
                    $"Press any key to exit.");

                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }
}
