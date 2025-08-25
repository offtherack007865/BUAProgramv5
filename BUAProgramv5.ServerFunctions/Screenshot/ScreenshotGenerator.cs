using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BUAProgramv5.ServerFunctions.Screenshot
{
    public class ScreenshotGenerator : IScreenshotGenerator
    {
        /// <summary>
        /// Captures the WPF screen to verify the selected choices.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public MemoryStream CreateBitmapFromVisual(Visual target)
        {
            Rect boundary = VisualTreeHelper.GetDescendantBounds(target);

            RenderTargetBitmap renderTarget = new RenderTargetBitmap((Int32)boundary.Width, (Int32)boundary.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext context = visual.RenderOpen())
            {
                VisualBrush visualBrush = new VisualBrush(target);
                context.DrawRectangle(visualBrush, null, new Rect(new System.Windows.Point(), boundary.Size));
            }

            renderTarget.Render(visual);

            MemoryStream stream = new MemoryStream();
            GifBitmapEncoder bitmapEncoder = new GifBitmapEncoder();

            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
            bitmapEncoder.Save(stream);

            Bitmap bitmap = new Bitmap(stream);
            ImageConverter image = new ImageConverter();

            Byte[] byteArray = (Byte[])image.ConvertTo(bitmap, typeof(Byte[]));
            MemoryStream result = new MemoryStream(byteArray);

            return result;
        }
    }
}
