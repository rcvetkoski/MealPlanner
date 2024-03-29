﻿using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.Models
{
    public class MuscleGroup : BaseModel, IHaveImage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string imageSourcePath;
        public string ImageSourcePath
        {
            get
            {
                return imageSourcePath;
            }
            set
            {
                if (imageSourcePath != value)
                {
                    imageSourcePath = value;
                    OnPropertyChanged("ImageSourcePath");
                    OnPropertyChanged("ImageSource");
                }
            }
        }
        private byte[] imageBlob;
        public byte[] ImageBlob
        {
            get
            {
                return imageBlob;
            }
            set
            {
                if (imageBlob != value)
                {
                    imageBlob = value;
                    OnPropertyChanged("ImageBlob");
                    OnPropertyChanged("ImageSource");
                }
            }
        }
        [Ignore]
        public ImageSource ImageSource
        {
            get
            {
                if (ImageBlob != null)
                    return ImageSource.FromStream(() => new MemoryStream(ImageBlob));

                if (string.IsNullOrEmpty(ImageSourcePath))
                    return ImageSource.FromResource($"MealPlanner.Resources.Images.{Name}.jpg");

                if (imageSourcePath.Contains("https"))
                    return new UriImageSource() { Uri = new Uri(ImageSourcePath), CachingEnabled = true, CacheValidity = TimeSpan.FromDays(1) };
                else
                    return ImageSource.FromFile(ImageSourcePath);
            }
        }
    }
}
