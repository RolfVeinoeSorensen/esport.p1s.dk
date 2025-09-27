using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Security.Cryptography;

namespace Esport.Backend.Services
{
    public static class CaptchaService
    {
        // Character set for captcha generation.
        // https://dotnettutorials.net/lesson/how-to-implement-captcha-in-asp-net-core/
        // Ambiguous characters like 0/O and 1/l/I are excluded to avoid confusion.
        private static readonly char[] Alphabet =
            "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789".ToCharArray();

        // Generates a random captcha code of the given length.
        // Uses a cryptographically secure random number generator for better unpredictability.
        public static string GenerateCaptchaCode(int length = 6)
        {
            var code = new char[length];
            for (int i = 0; i < length; i++)
                code[i] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
            return new string(code);
        }

        // Generates a captcha image (PNG format) from the provided captcha text.
        // Includes random rotation, position, color variations, and noise lines to
        // make automated recognition more difficult while keeping it human-readable.
        public static byte[] GenerateCaptchaImage(string captchaCode)
        {
            int width = 150;  // Overall image width in pixels
            int height = 60;  // Overall image height in pixels

            // Create a blank white image
            using var image = new Image<Rgba32>(width, height, Color.White);
            var random = new Random();

            // Choose font style and size for captcha characters
            var font = SystemFonts.CreateFont("Arial", 32, FontStyle.Bold);

            image.Mutate(ctx =>
            {
                float x = 8f; // Starting X position for first character

                // Render each captcha character with random variations
                foreach (char c in captchaCode)
                {
                    var angle = random.Next(-15, 15); // Random tilt to make OCR harder
                    var color = Color.FromRgb(
                        (byte)random.Next(50, 200),   // Random R
                        (byte)random.Next(50, 200),   // Random G
                        (byte)random.Next(50, 200));  // Random B

                    // Random vertical offset (some characters higher/lower)
                    float y = random.Next(5, 25);

                    // Draw the character at calculated position
                    ctx.DrawText(c.ToString(), font, color, new PointF(x, y));

                    // Move X slightly for the next character (prevent overlap)
                    x += 22;
                }

                // Add a few random noise lines across the image
                // These lines reduce the success rate of automated captcha solvers
                for (int i = 0; i < 4; i++)
                {
                    var p1 = new PointF(random.Next(width), random.Next(height));
                    var p2 = new PointF(random.Next(width), random.Next(height));
                    var lineColor = Color.FromRgb(
                        (byte)random.Next(150, 255),
                        (byte)random.Next(150, 255),
                        (byte)random.Next(150, 255));

                    ctx.DrawLine(lineColor, 1f, new[] { p1, p2 });
                }
            });

            // Save image into memory as PNG and return as byte array
            using var ms = new MemoryStream();
            image.Save(ms, new PngEncoder());
            return ms.ToArray();
        }
    }
}