using SkiaSharp.QrCode;
using SkiaSharp;

namespace WebToApp2.Services
{
    public interface IGeneratorService
    {
        public Task<string> GenerateQr(string qrUrl);
    }

    public class GeneratorManager : IGeneratorService
    {
        public Task<string> GenerateQr(string qrUrl)
        {
            string qrImageBase64String;
            using (var generator = new QRCodeGenerator())
            {
                // Generate QrCode
                var qr = generator.CreateQrCode(qrUrl, ECCLevel.L);
                // Render to canvas
                var info = new SKImageInfo(306, 306);
                using (var surface = SKSurface.Create(info))
                {
                    var canvas = surface.Canvas;
                    canvas.Render(qr, info.Width, info.Height);
                    // Output to Stream -> File
                    using (var image = surface.Snapshot())
                    using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
                        qrImageBase64String = Convert.ToBase64String(data.AsSpan());
                }
            }

            return Task.FromResult(qrImageBase64String);
        }
    }
}
