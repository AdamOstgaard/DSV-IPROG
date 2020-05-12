using System;
using System.Net;
using System.Windows.Forms;

namespace IPROG.Uppgifter.uppg2_2_1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length != 3)
            {
                return;
            }

            if (!int.TryParse(args[0], out var remotePort) || remotePort > 65535)
            {
                Console.WriteLine("Invalid port!");
                return;
            }

            if (!IPAddress.TryParse(args[1], out var ipAddress))
            {
                Console.WriteLine("Invalid IP!");
                return;
            }

            if (!int.TryParse(args[2], out var port) || port > 65535)
            {
                Console.WriteLine("Invalid port!");
                return;
            }

            using (var pointSender = new UdpSender())
            using (var pointListener = new PointListener())
            {
                pointSender.Connect(ipAddress, remotePort);
                pointListener.StartListen(port: port);
                Application.Run(new WhiteBoard(pointSender, pointListener));
            }
        }
    }
}
