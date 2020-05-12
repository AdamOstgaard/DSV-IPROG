using System.Drawing.Imaging;
using System.Linq;

namespace IPROG.Uppgifter.gesallprov.ScreenSharing
{
    /// <summary>
    /// Helper functions for getting codec and encoder options for image serialization.
    /// </summary>
    internal static class ImageEncoder
    {
        /// <summary>
        /// Get encoder and codec for a specific image format.
        /// </summary>
        /// <param name="format">The image format to get the codec for</param>
        /// <param name="quality">Target quality for compression</param>
        /// <returns>The codec and parameters needed for image encoding</returns>
        public static (ImageCodecInfo codec, EncoderParameters parameters) GetImageEncoder(ImageFormat format, long quality)
        {
            var jpgEncoder = GetEncoder(format);

            var myEncoder = Encoder.Quality;
            
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            return (jpgEncoder, myEncoderParameters);
        }

        /// <summary>
        /// Get the encoder corresponding to the image format.
        /// </summary>
        /// <param name="format">Desired image format</param>
        /// <returns>An image encoder for the format</returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();

            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }
    }
}
