using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.Models
{
    public class Exercice : BaseModel
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
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        public int MuscleGroupId { get; set; }
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
                    //return new FontImageSource() { Glyph = FontAwesomeIcons.Image, FontFamily = "FA-Solid", Color = Color.Gray, Size = 120 };
                    return ImageSource.FromResource("MealPlanner.Resources.Images.image.png");

                if (imageSourcePath.Contains("https"))
                    return new UriImageSource() { Uri = new Uri(ImageSourcePath), CachingEnabled = true, CacheValidity = TimeSpan.FromDays(1) };
                else
                    return ImageSource.FromFile(ImageSourcePath);
            }
        }
        [Ignore]
        public ObservableCollection<Set> Sets { get; set; }


        public Exercice()
        {
            Sets = new ObservableCollection<Set>();
        }
    }
}
