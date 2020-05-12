using System;

namespace IPROG.Uppgifter.uppg2_1_2
{
    /// <summary>
    /// Static helper functions updating the Title text of the cmd window.
    /// </summary>
    static class ConsoleTitleBuilder
    {
        private static int s_connectedClients;
        private static int s_port;
        private static string s_iP;

        /// <summary>
        /// Gets or sets the IP portion of the title.
        /// </summary>
        public static string IP { get => s_iP; set { s_iP = value; Update(); } }

        /// <summary>
        /// Gets or sets the port portion of the title.
        /// </summary>
        public static int Port { get => s_port; set { s_port = value; Update(); } }

        /// <summary>
        /// Gets or sets the Connected clients portion of the title.
        /// </summary>
        public static int ConnectedClients { get => s_connectedClients; set { s_connectedClients = value; Update(); } }

        /// <summary>
        /// Updates the title text.
        /// </summary>
        private static void Update()
        {
            var titleText = $"Server Running on IP: {IP} PORT {Port}. CLIENTS: {ConnectedClients}";
            Console.Title = titleText;
        }
    }
}
