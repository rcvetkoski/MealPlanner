using MealPlanner.Helpers.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.Models
{
    public abstract class Aliment : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        private double calories;
        public double Calories { get { return calories; } set { calories = value; OnPropertyChanged("Calories"); OnPropertyChanged("CaloriesString"); OnPropertyChanged("CaloriesProgress"); } }

        private double proteins;
        public double Proteins { get { return proteins; } set { proteins = value; OnPropertyChanged("Proteins"); OnPropertyChanged("NutritionValuesString"); OnPropertyChanged("ProteinsProgress"); } }

        private double carbs;
        public double Carbs { get { return carbs; } set { carbs = value; OnPropertyChanged("Carbs"); OnPropertyChanged("NutritionValuesString"); OnPropertyChanged("CarbsProgress"); } }

        private double fats;
        public double Fats { get { return fats; } set { fats = value; OnPropertyChanged("Fats"); OnPropertyChanged("NutritionValuesString"); OnPropertyChanged("FatsProgress"); } }
        public double OriginalServingSize { get; set; }

        private double servingSize;
        public double ServingSize { get { return servingSize; } set { servingSize = value; OnPropertyChanged("ServingSize"); OnPropertyChanged("ServingSizeWithUnit"); } }

        public int DayMealAlimentId { get; set; } = 0;

        private string imageSourcePath;
        public string ImageSourcePath { get { return imageSourcePath; } set { imageSourcePath = value; OnPropertyChanged("ImageSourcePath"); OnPropertyChanged("ImageSource"); } }
        private byte[] imageBlob;
        public byte[] ImageBlob { get { return imageBlob; } set { imageBlob = value; OnPropertyChanged("ImageBlob"); OnPropertyChanged("ImageSource"); } }
        public virtual AlimentTypeEnum AlimentType { get; }

        private AlimentUnitEnum unit;
        public AlimentUnitEnum Unit { get { return unit; } set { unit = value; OnPropertyChanged("Unit"); OnPropertyChanged("ServingSizeWithUnit"); } }


        [Ignore]
        public double CaloriesProgress { get { return calories / App.RefData.User.TargetCalories; } }
        [Ignore]
        public double FatsProgress { get { return fats / App.RefData.User.TargetFats; } }
        [Ignore]
        public double CarbsProgress { get { return carbs / App.RefData.User.TargetCarbs; } }
        [Ignore]
        public double ProteinsProgress { get { return proteins / App.RefData.User.TargetProteins; } }



        [Ignore]
        public string ServingSizeWithUnit { get { return $"{ServingSize} {Unit}"; } }
        [Ignore]
        public string NutritionValuesString { get { return $"P: {Math.Round(Proteins, 2)},  C: {Math.Round(Carbs, 2)},  F: {Math.Round(Fats, 2)}"; } }
        [Ignore]
        public string CaloriesString { get { return $"{Math.Round(Calories, 2)} Kcal"; } }
        [Ignore]
        public ImageSource ImageSource 
        { 
            get
            { 
                if(ImageBlob != null)
                    return ImageSource.FromStream(() => new MemoryStream(ImageBlob));

                if(string.IsNullOrEmpty(ImageSourcePath))
                    return null;    

                if(imageSourcePath.Contains("https"))
                    return new UriImageSource() { Uri = new Uri(ImageSourcePath), CachingEnabled = true, CacheValidity = TimeSpan.FromDays(1) };
                else
                    return ImageSource.FromFile(ImageSourcePath);
            }
        } 
    }
}
