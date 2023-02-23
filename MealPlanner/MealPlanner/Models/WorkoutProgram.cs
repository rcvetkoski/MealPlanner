using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.Models
{
    public class WorkoutProgram : BaseModel, IHaveImage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 

        public string Name { get; set; }

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
                    return ImageSource.FromResource("MealPlanner.Resources.Images.image.png");

                if (imageSourcePath.Contains("https"))
                    return new UriImageSource() { Uri = new Uri(ImageSourcePath), CachingEnabled = true, CacheValidity = TimeSpan.FromDays(1) };
                else
                    return ImageSource.FromFile(ImageSourcePath);
            }
        }

        [Ignore]
        public ObservableCollection<WorkoutWeek> WorkoutWeeks { get; set; }

        private WorkoutWeek selectedWorkoutWeek;
        [Ignore]
        public WorkoutWeek SelectedWorkoutWeek
        {
            get
            {
                return selectedWorkoutWeek;
            }
            set
            {
                if(selectedWorkoutWeek != value)
                {
                    selectedWorkoutWeek = value;
                    OnPropertyChanged(nameof(SelectedWorkoutWeek));
                }
            }
        }

        public WorkoutProgram()
        {
            WorkoutWeeks = new ObservableCollection<WorkoutWeek>();
        }
    }
}
