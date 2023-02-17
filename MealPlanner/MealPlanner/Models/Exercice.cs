using MealPlanner.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.Models
{
    public class Exercice : BaseModel, IHaveImage
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
        private MuscleGroup muscleGroup;
        [Ignore]
        public MuscleGroup MuscleGroup
        { 
            get
            {
                return muscleGroup;
            }
            set
            {
                if(muscleGroup != value)
                {
                    muscleGroup = value;
                    MuscleGroupId = muscleGroup.Id;
                    OnPropertyChanged(nameof(MuscleGroup));
                }
            }
        }
        private TimeSpan restTimeBetweenSets;
        [Ignore]
        public TimeSpan RestTimeBetweenSets
        {
            get
            {
                return restTimeBetweenSets;
            }
            set
            {
                if(restTimeBetweenSets != value)
                {
                    restTimeBetweenSets = value;
                    OnPropertyChanged(nameof(RestTimeBetweenSets));
                }
            }
        }
        public bool Archived { get; set; }
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
        public int WorkoutExerciceId { get; set; }
        [Ignore]
        public double TotalWeight 
        {
            get
            {
                return Sets.Sum(x => (x.Weight * x.Reps));
            }
        }
        private ObservableCollection<Set> sets;
        [Ignore]
        public ObservableCollection<Set> Sets
        { 
            get
            {
                return sets;
            }
            set
            {
                if(sets != value)
                {
                    sets = value;
                    OnPropertyChanged(nameof(Sets));
                    OnPropertyChanged(nameof(TotalWeight));
                }
            }
        }
        private List<ExerciceHistoryHelper> history;
        [Ignore]
        public List<ExerciceHistoryHelper> History 
        { 
            get
            {
                return history;
            }
            set
            {
                if(history != value)
                {
                    history = value;
                    OnPropertyChanged(nameof(History));
                }
            }
        }

        public Exercice()
        {
            Sets = new ObservableCollection<Set>();
        }
    }
}
