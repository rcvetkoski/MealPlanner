using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Lights;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MealPlanner.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MealPlanner.Droid.Services
{
    public class ImageServiceDroid : IImageService
    {
        public void ResizeImage(string sourceFile, string targetFile, float maxWidth, float maxHeight)
        {
            if (File.Exists(sourceFile))
            {
                // First decode with inJustDecodeBounds=true to check dimensions
                var options = new BitmapFactory.Options()
                {
                    InJustDecodeBounds = false,
                    InPurgeable = true,
                };

                try
                {
                    using (var image = BitmapFactory.DecodeFile(sourceFile, options))
                    {
                        ExifInterface exifInterface = new ExifInterface(sourceFile);
                        var exifOrientation = exifInterface.GetAttributeInt(ExifInterface.TagOrientation, 1);

                        if (image != null)
                        {
                            var sourceSize = new Size((int)image.GetBitmapInfo().Height, (int)image.GetBitmapInfo().Width);

                            var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

                            string targetDir = System.IO.Path.GetDirectoryName(targetFile);
                            if (!Directory.Exists(targetDir))
                                Directory.CreateDirectory(targetDir);

                            if (maxResizeFactor > 0.9)
                            {
                                File.Copy(sourceFile, targetFile);
                            }
                            else
                            {
                                var width = (int)(maxResizeFactor * sourceSize.Width);
                                var height = (int)(maxResizeFactor * sourceSize.Height);

                                using (var bitmapScaled = Bitmap.CreateScaledBitmap(image, height, width, true))
                                {
                                    using (System.IO.Stream outStream = File.Create(targetFile))
                                    {
                                        if (targetFile.ToLower().EndsWith("png"))
                                            bitmapScaled.Compress(Bitmap.CompressFormat.Png, 100, outStream);
                                        else
                                            bitmapScaled.Compress(Bitmap.CompressFormat.Jpeg, 95, outStream);

                                        // Save orientation from original image to compressed one
                                        ExifInterface exif = new ExifInterface(targetFile);
                                        exif.SetAttribute(ExifInterface.TagOrientation, exifOrientation.ToString());
                                        exif.SaveAttributes();
                                    }
                                    bitmapScaled.Recycle();
                                }
                            }

                            image.Recycle();
                        }
                        else
                            Console.WriteLine("Image scaling failed: " + sourceFile);
                    }
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Scale image by percentage, sizePercentage must be between 1 - 100
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="targetFile"></param>
        /// <param name="sizePercentage"></param>
        public void ResizeImage(string sourceFile, string targetFile, float sizePercentage)
        {
            if (File.Exists(sourceFile))
            {
                // First decode with inJustDecodeBounds=true to check dimensions
                var options = new BitmapFactory.Options()
                {
                    InJustDecodeBounds = false,
                    InPurgeable = true,
                };

                try
                {
                    using (var image = BitmapFactory.DecodeFile(sourceFile, options))
                    {
                        ExifInterface exifInterface = new ExifInterface(sourceFile);
                        var exifOrientation = exifInterface.GetAttributeInt(ExifInterface.TagOrientation, 1);

                        int rotationDegrees = 0;

                        switch (exifOrientation)
                        {
                            case (int)Android.Media.Orientation.Rotate90:
                                rotationDegrees = 90;
                                break;
                            case (int)Android.Media.Orientation.Rotate180:
                                rotationDegrees = 180;
                                break;
                            case (int)Android.Media.Orientation.Rotate270:
                                rotationDegrees = 270;
                                break;
                        }

                        Matrix matrix = new Matrix();
                        if (exifOrientation != 0)
                        { 
                            matrix.PreRotate(rotationDegrees);
                        }


                        if (image != null)
                        {
                            var sourceSize = new Size((int)image.GetBitmapInfo().Width, (int)image.GetBitmapInfo().Height);

                            var normalizedBitmap = Bitmap.CreateBitmap(image, 0, 0, sourceSize.Width, sourceSize.Height, matrix, true);

                            float maxWidth = (sizePercentage >= 1 && sizePercentage <= 100) ? (sourceSize.Width * sizePercentage) / 100 : sourceSize.Width;
                            float maxHeight = (sizePercentage >= 1 && sizePercentage <= 100) ? (sourceSize.Height * sizePercentage) / 100 : sourceSize.Height;

                            var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

                            string targetDir = System.IO.Path.GetDirectoryName(targetFile);
                            if (!Directory.Exists(targetDir))
                                Directory.CreateDirectory(targetDir);

                            if (maxResizeFactor > 0.9)
                            {
                                File.Copy(sourceFile, targetFile);
                            }
                            else
                            {
                                var width = (int)(maxResizeFactor * sourceSize.Width);
                                var height = (int)(maxResizeFactor * sourceSize.Height);

                                using (var bitmapScaled = Bitmap.CreateScaledBitmap(normalizedBitmap, width, height, true))
                                {
                                    using (System.IO.Stream outStream = File.Create(targetFile))
                                    {
                                        if (targetFile.ToLower().EndsWith("png"))
                                            bitmapScaled.Compress(Bitmap.CompressFormat.Png, 100, outStream);
                                        else
                                            bitmapScaled.Compress(Bitmap.CompressFormat.Jpeg, 95, outStream);

                                        //// Save orientation from original image to compressed one
                                        //ExifInterface exif = new ExifInterface(targetFile);
                                        //exif.SetAttribute(ExifInterface.TagOrientation, exifOrientation.ToString());
                                        //exif.SaveAttributes();
                                    }
                                    bitmapScaled.Recycle();
                                }
                            }

                            image.Recycle();
                        }
                        else
                            Console.WriteLine("Image scaling failed: " + sourceFile);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);  
                }
            }
        }
    }
}