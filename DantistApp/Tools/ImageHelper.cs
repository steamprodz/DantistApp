using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DantistApp.Tools
{
    static class ImageHelper
    {
        public static string SaveImageToFile(BitmapSource image)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.FileName = "Рисунок"; // Default file name
            dlg.DefaultExt = ".jpg"; // Default file extension
            dlg.Filter = "JPG images (.jpg)|*.jpg"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            string filePath = null;

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                filePath = dlg.FileName;

                SaveClipboardImageToFile(filePath, image);
            }

            return filePath;
        }

        private static void SaveClipboardImageToFile(string filePath, BitmapSource image)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }

        public static System.Drawing.Image ImageWpfToGDI(System.Windows.Media.ImageSource imageSource)
        {
            MemoryStream ms = new MemoryStream();
            var encoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(imageSource as System.Windows.Media.Imaging.BitmapSource));
            encoder.Save(ms);
            ms.Flush();

            return System.Drawing.Image.FromStream(ms);
        }

        public static string ReadJPEGComment(string imageFlePath)
        {
            string comments = "";


            try
            {
                string jpegDirectory = Path.GetDirectoryName(imageFlePath);
                string jpegFileName = Path.GetFileNameWithoutExtension(imageFlePath);

                BitmapDecoder decoder = null;
                BitmapFrame bitmapFrame = null;
                BitmapMetadata metadata = null;
                FileInfo originalImage = new FileInfo(imageFlePath);



                if (File.Exists(imageFlePath))
                {
                    // load the jpg file with a JpegBitmapDecoder    
                    using (Stream jpegStreamIn = File.Open(imageFlePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        decoder = new JpegBitmapDecoder(jpegStreamIn, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                    }

                    bitmapFrame = decoder.Frames[0];
                    metadata = (BitmapMetadata)bitmapFrame.Metadata;

                    if (metadata.Comment != null)
                        return metadata.Comment.ToString();
                    return null;
                    //if (bitmapFrame != null)
                    //{
                    //    BitmapMetadata metaData = (BitmapMetadata)bitmapFrame.Metadata.Clone();

                    //    if (metaData != null)
                    //    {
                    //        // read the metadata   
                    //         comments = metaData.Comment.ToString();
                    //    }
                    //}
                }
            }
            catch { }

            return comments;
        }

        public static void WriteJPEGComment(string imageFlePath, string comments)
        {
            string jpegDirectory = Path.GetDirectoryName(imageFlePath);
            string jpegFileName = Path.GetFileNameWithoutExtension(imageFlePath);

            BitmapDecoder decoder = null;
            BitmapFrame bitmapFrame = null;
            BitmapMetadata metadata = null;
            FileInfo originalImage = new FileInfo(imageFlePath);

            if (File.Exists(imageFlePath))
            {
                // load the jpg file with a JpegBitmapDecoder    
                using (Stream jpegStreamIn = File.Open(imageFlePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    decoder = new JpegBitmapDecoder(jpegStreamIn, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                }

                bitmapFrame = decoder.Frames[0];
                metadata = (BitmapMetadata)bitmapFrame.Metadata;

                if (bitmapFrame != null)
                {
                    BitmapMetadata metaData = (BitmapMetadata)bitmapFrame.Metadata.Clone();

                    if (metaData != null)
                    {
                        // modify the metadata   
                        metaData.Comment = comments;
                        //metaData.SetQuery("/app1/ifd/exif:{uint=40092}", comments);
                        //metaData.GetQuery("/app1/ifd/exif:{uint=40092}");

                        // get an encoder to create a new jpg file with the new metadata.      
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmapFrame, bitmapFrame.Thumbnail, metaData, bitmapFrame.ColorContexts));
                        //string jpegNewFileName = Path.Combine(jpegDirectory, "JpegTemp.jpg");

                        // Delete the original
                        originalImage.Delete();

                        // Save the new image 
                        using (Stream jpegStreamOut = File.Open(imageFlePath, FileMode.CreateNew, FileAccess.ReadWrite))
                        {
                            encoder.Save(jpegStreamOut);
                        }
                    }
                }
            }
        }

        public static string LoadImageToControl(Image image)
        {
            Stream stream = null;
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "JPEG файлы (*.jpg)|*.jpg";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if ((stream = dlg.OpenFile()) != null)
                    {
                        using (stream)
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            bitmap.Freeze();

                            image.Source = bitmap;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Файл не поддается чтению");
                }
            }

            return dlg.FileName;
        }

    }
}
