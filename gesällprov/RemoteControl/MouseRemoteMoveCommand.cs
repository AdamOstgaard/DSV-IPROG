using System;
using System.Drawing;

namespace IPROG.Uppgifter.gesallprov.RemoteControl
{
    /// <summary>
    /// Wrapper for mouseMoveData
    /// </summary>
    [Serializable]
    internal class MouseRemoteMoveCommand
    {
        /// <summary>
        /// The new absolute position for the data.
        /// </summary>
        public Point Position { get; set; }
    }
}
