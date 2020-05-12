using System;
using System.Windows.Forms;

namespace IPROG.Uppgifter.gesallprov.RemoteControl
{
    /// <summary>
    /// Wrapper for key press data
    /// </summary>
    [Serializable]
    internal class KeyDownRemoteCommand
    {
        /// <summary>
        /// The pressed key
        /// </summary>
        public Keys Key { get; set; }
    }
}
