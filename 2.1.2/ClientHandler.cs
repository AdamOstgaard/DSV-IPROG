using System;
using System.Collections.Generic;

namespace IPROG.Uppgifter.uppg2_1_2
{
    /// <summary>
    /// Handles messaging between <see cref="ChatServerClient"/>s.
    /// </summary>
    internal class ClientHandler
    {
        private int ConnectedClientCount => _clients.Count;

        private readonly List<ChatServerClient> _clients;

        /// <summary>
        /// Creates a new <see cref="ClientHandler"/>.
        /// </summary>
        public ClientHandler()
        {
            _clients = new List<ChatServerClient>();
        }

        /// <summary>
        /// Adds a client to this <see cref="ClientHandler"/> and subscribe to its updates.
        /// </summary>
        /// <param name="client"><see cref="ChatServerClient"/> to add</param>
        public void AddClient(ChatServerClient client)
        {
            _clients.Add(client);

            client.OnMessage += Client_OnMessage;
            client.OnDisconnect += Client_OnDisconnect;

            Console.WriteLine($"{client.HostName} connected");
            ConsoleTitleBuilder.ConnectedClients = ConnectedClientCount;
        }

        /// <summary>
        /// Handles incoming client messages.
        /// </summary>
        /// <param name="sender">Source <see cref="ChatServerClient"/>.</param>
        /// <param name="e">Message Arguments.</param>
        private void Client_OnMessage(ChatServerClient sender, OnMessageEventArgs e)
        {
            BroadcastMessage(e.MessageText);

            Console.WriteLine($"{e.Sender} {e.MessageText}");
        }

        /// <summary>
        /// Handles client disconnects.
        /// </summary>
        /// <param name="sender">The disconnecting <see cref="ChatServerClient"/></param>
        /// <param name="e">Event arguments</param>
        private void Client_OnDisconnect(ChatServerClient sender, OnClientDisconnectedEventArgs e)
        {
            var disconnectMessage = $"{e.Client} disconnected";

            _clients.Remove(sender);

            BroadcastMessage(disconnectMessage);
            Console.WriteLine(disconnectMessage);

            ConsoleTitleBuilder.ConnectedClients = ConnectedClientCount;
        }

        private void BroadcastMessage(string text)
            => _clients.ForEach(async c => await c.SendMessageAsync(text));
    }
}
