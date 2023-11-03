using SkiaSharp.QrCode;
using SkiaSharp;
using QRCoder;
using QRCodeGenerator = QRCoder.QRCodeGenerator;
using QRCodeData = QRCoder.QRCodeData;

namespace WebToApp2.Services
{
    public interface IGeneratorService
    {
        public Task<byte[]> GenerateQr(string qrUrl);
        //public Task<string> GenerateQr(string qrUrl);
    }

    public class GeneratorManager : IGeneratorService
    {
        public Task<byte[]> GenerateQr(string qrUrl)
        {
            QRCodeGenerator qrGenerator = new();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrUrl, QRCodeGenerator.ECCLevel.L);

            BitmapByteQRCode byteQrCode = new BitmapByteQRCode(qrCodeData);
            var qrCodeImage = byteQrCode.GetGraphic(5);

            return Task.FromResult(qrCodeImage);
        }

        //public Task<string> GenerateQr(string qrUrl)
        //{
        //    string qrImageBase64String;
        //    using (var generator = new QRCodeGenerator())
        //    {
        //        // Generate QrCode
        //        var qr = generator.CreateQrCode(qrUrl, ECCLevel.L);
        //        // Render to canvas
        //        var info = new SKImageInfo(306, 306);
        //        using (var surface = SKSurface.Create(info))
        //        {
        //            var canvas = surface.Canvas;
        //            canvas.Render(qr, info.Width, info.Height);
        //            // Output to Stream -> File
        //            using (var image = surface.Snapshot())
        //            using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
        //                qrImageBase64String = Convert.ToBase64String(data.AsSpan());
        //        }
        //    }

        //    return Task.FromResult(qrImageBase64String);
        //}
    }
}
