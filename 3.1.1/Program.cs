using System;
using System.IO;
using System.Net;
using IPROG.Uppgifter.Networking;

namespace IPROG.Uppgifter.uppg3_1_1
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please enter a url as argument.");
                return 1;
            }

            var addr = args[0];

            try
            {
                var content = GetWebsiteContent(addr);
                Console.Write(content);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured when trying to open the link!");
                Console.WriteLine(e);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Get the website content and return it as a string.
        /// </summary>
        /// <param name="addr">Website to download</param>
        /// <returns>Downloaded content as string</returns>
        private static string GetWebsiteContent(string addr)
        {
            var request = WebRequest.Create(addr);
            var response = request.GetResponse();
            var data = response.GetResponseStream();

            using var sr = new StreamReader(data);
            return sr.ReadToEnd();
        }
    }
}
