using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.Services
{
    public interface IImageService
    {
        void ResizeImage(string sourceFile, string targetFile, float maxWidth, float maxHeight);
        void ResizeImage(string sourceFile, string targetFile, float sizePercentage);
    }
}
