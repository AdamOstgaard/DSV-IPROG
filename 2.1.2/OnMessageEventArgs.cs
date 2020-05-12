namespace IPROG.Uppgifter.uppg2_1_2
{
    /// <summary>
    /// Event arguments for a new incoming message.
    /// </summary>
    internal class OnMessageEventArgs
    {
        /// <summary>
        /// Message payload.
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Sender hostname.
        /// </summary>
        public string Sender { get; set; }
    }
}
