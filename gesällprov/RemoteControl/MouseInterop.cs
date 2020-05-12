namespace IPROG.Uppgifter.gesallprov.RemoteControl
{
    /// <summary>
    /// Static methods for controlling mouse input through user32.dll as there is no built in support for it in C#.
    /// <remarks>
    /// This class is not cross platform and mainly works on Windows.
    /// </remarks>
    /// </summary>
    internal static class MouseInterop
    {
        /// <summary>
        /// Call external method mouse_event from user32.dll
        /// </summary>
        /// <param name="dwFlags">Event type</param>
        /// <param name="dx">Position x</param>
        /// <param name="dy">Position y</param>
        /// <param name="dwData">Data such as mouse wheel movement</param>
        /// <param name="dwExtraInfo">Extra</param>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private enum MouseAction
        {
            MouseEventLeftDown = 0x02,
            MouseEventLeftUp = 0x04,
            MouseEventRightDown = 0x08,
            MouseEventRightUp = 0x10
        }

        /// <summary>
        /// Press left mouse button.
        /// </summary>
        public static void LeftButtonDown()
        {
            mouse_event((int)MouseAction.MouseEventLeftDown, 0, 0, 0, 0);
        }

        /// <summary>
        /// Press right mouse button.
        /// </summary>
        public static void RightButtonDown()
        {
            mouse_event((int)MouseAction.MouseEventRightDown, 0, 0, 0, 0);
        }

        /// <summary>
        /// Release left mouse button.
        /// </summary>
        public static void LeftButtonUp()
        {
            mouse_event((int)MouseAction.MouseEventLeftUp, 0, 0, 0, 0);
        }

        /// <summary>
        /// Release right mouse button.
        /// </summary>
        public static void RightButtonUp()
        {
            mouse_event((int)MouseAction.MouseEventRightUp, 0, 0, 0, 0);
        }
    }
}
