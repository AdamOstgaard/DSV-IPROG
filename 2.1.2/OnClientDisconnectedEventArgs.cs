namespace IPROG.Uppgifter.uppg2_1_2
{
    /// <summary>
    /// Event arguments for a disconnecting client.
    /// </summary>
    internal class OnClientDisconnectedEventArgs
    {
        /// <summary>
        /// The client disconnected.
        /// </summary>
        public string Client { get; set; }
    }
}
