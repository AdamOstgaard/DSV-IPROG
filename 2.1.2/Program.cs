using System;

namespace IPROG.Uppgifter.uppg2_1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var chatServer = new ChatServer())
            {
                chatServer.StartAccept();
                Console.ReadKey();
            }
        }
    }
}
