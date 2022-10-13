using CoreGraphics;
using Foundation;
using MealPlanner.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;

namespace MealPlanner.iOS.Services
{
    public class ImageServiceIOS : IImageService
    {
        public void ResizeImage(string sourceFile, string targetFile, float maxWidth, float maxHeight)
        {
            if (File.Exists(sourceFile) && !File.Exists(targetFile))
            {
                using (UIImage sourceImage = UIImage.FromFile(sourceFile))
                {
                    var sourceSize = sourceImage.Size;
                    var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

                    if (!Directory.Exists(Path.GetDirectoryName(targetFile)))
                        Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

                    if (maxResizeFactor > 0.9)
                    {
                        File.Copy(sourceFile, targetFile);
                    }
                    else
                    {
                        var width = maxResizeFactor * sourceSize.Width;
                        var height = maxResizeFactor * sourceSize.Height;

                        UIGraphics.BeginImageContextWithOptions(new CGSize((float)width, (float)height), true, 1.0f);
                        //  UIGraphics.GetCurrentContext().RotateCTM(90 / Math.PI);
                        sourceImage.Draw(new CGRect(0, 0, (float)width, (float)height));

                        var resultImage = UIGraphics.GetImageFromCurrentImageContext();
                        UIGraphics.EndImageContext();


                        if (targetFile.ToLower().EndsWith("png"))
                            resultImage.AsPNG().Save(targetFile, true);
                        else
                            resultImage.AsJPEG().Save(targetFile, true);
                    }
                }
            }
        }

        public void ResizeImage(string sourceFile, string targetFile, nfloat sizePercentage)
        {
            if (File.Exists(sourceFile) && !File.Exists(targetFile))
            {
                using (UIImage sourceImage = UIImage.FromFile(sourceFile))
                {
                    var sourceSize = sourceImage.Size;

                    nfloat maxWidth = (sizePercentage >= 1 && sizePercentage <= 100) ? (sourceSize.Width * sizePercentage) / 100 : sourceSize.Width;
                    nfloat maxHeight = (sizePercentage >= 1 && sizePercentage <= 100) ? (sourceSize.Height * sizePercentage) / 100 : sourceSize.Height;

                    var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

                    if (!Directory.Exists(Path.GetDirectoryName(targetFile)))
                        Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

                    if (maxResizeFactor > 0.9)
                    {
                        File.Copy(sourceFile, targetFile);
                    }
                    else
                    {
                        var width = maxResizeFactor * sourceSize.Width;
                        var height = maxResizeFactor * sourceSize.Height;

                        UIGraphics.BeginImageContextWithOptions(new CGSize((float)width, (float)height), true, 1.0f);
                        //  UIGraphics.GetCurrentContext().RotateCTM(90 / Math.PI);
                        sourceImage.Draw(new CGRect(0, 0, (float)width, (float)height));

                        var resultImage = UIGraphics.GetImageFromCurrentImageContext();
                        UIGraphics.EndImageContext();


                        if (targetFile.ToLower().EndsWith("png"))
                            resultImage.AsPNG().Save(targetFile, true);
                        else
                            resultImage.AsJPEG().Save(targetFile, true);
                    }
                }
            }
        }
    }
}