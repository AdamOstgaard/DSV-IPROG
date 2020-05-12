using System;
using System.Windows.Forms;

namespace IPROG.Uppgifter.gesallprov.RemoteControl
{
    /// <summary>
    /// Wrapper for mouse click data
    /// </summary>
    [Serializable]
    internal class MouseRemoteClickCommand
    {
        /// <summary>
        /// The button that affected
        /// </summary>
        public MouseButtons Button { get; set; }

        /// <summary>
        /// True if the button was pressed false if it was released.
        /// </summary>
        public bool Pressed { get; set; }
    }
}