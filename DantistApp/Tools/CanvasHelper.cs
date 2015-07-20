using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DantistApp.Tools
{
    public static class CanvasHelper
    {
        public static RenderTargetBitmap SaveCanvasToBitmap(Canvas canvas)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width, 
                (int)canvas.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);

            return rtb;

            //var crop = new CroppedBitmap(rtb, new Int32Rect(50, 50, 250, 250));

            //BitmapEncoder pngEncoder = new PngBitmapEncoder();
            //pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            //using (var fs = System.IO.File.OpenWrite("logo.png"))
            //{
            //    pngEncoder.Save(fs);
            //}
        }
    }
}
