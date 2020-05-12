using System;

namespace IPROG.Uppgifter.gesallprov.ScreenSharing
{
    /// <summary>
    /// Wrapper for image data sent by a binary serializer.
    /// </summary>
    [Serializable]
    internal class StreamedImage
    {
        /// <summary>
        /// The image data as bytes.
        /// </summary>
        public byte[] data { get; set; }
    }
}
