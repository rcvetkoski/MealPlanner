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
        public string Name
        { 
            get
            { 
                return name;
            }
            set
            {
                if(name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            } 
        }
        private double calories;
        public double Calories 
        {
            get
            { 
                return calories;
            } 
            set
            { 
                if(calories != value)
                {
                    calories = value;
                    OnPropertyChanged("Calories");
                    OnPropertyChanged("CaloriesString");
                    OnPropertyChanged("CaloriesProgress");
                }
            }
        }

        private double proteins;
        public double Proteins 
        { 
            get
            { 
                return proteins;
            }
            set 
            {
                if(proteins != value)
                {
                    proteins = value;
                    OnPropertyChanged("Proteins");
                    OnPropertyChanged("NutritionValuesString");
                    OnPropertyChanged("ProteinsProgress");
                    //CalculateCalories();
                }
            } 
        }

        private double carbs;
        public double Carbs 
        {
            get
            { 
                return carbs; 
            } 
            set 
            { 
                if(carbs != value)
                {
                    carbs = value;
                    OnPropertyChanged("Carbs");
                    OnPropertyChanged("NutritionValuesString");
                    OnPropertyChanged("CarbsProgress");
                    //CalculateCalories();
                }
            }
        }

        private double fats;
        public double Fats 
        { 
            get
            {
                return fats; 
            }
            set 
            { 
                if(fats != value)
                {
                    fats = value;
                    OnPropertyChanged("Fats");
                    OnPropertyChanged("NutritionValuesString");
                    OnPropertyChanged("FatsProgress");
                    //CalculateCalories();
                }
            } 
        }
        public double OriginalServingSize { get; set; }

        private double servingSize;
        public double ServingSize
        { 
            get
            {
                return servingSize; 
            }
            set 
            { 
                if(servingSize != value)
                {
                    servingSize = value;
                    OnPropertyChanged("ServingSize");
                    OnPropertyChanged("ServingSizeWithUnit");
                }
            } 
        }

        public int MealAlimentId { get; set; } = 0;

        private string imageSourcePath;
        public string ImageSourcePath 
        {
            get
            { 
                return imageSourcePath;
            }
            set
            { 
                if(imageSourcePath != value)
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
                if(imageBlob != value)
                {
                    imageBlob = value;
                    OnPropertyChanged("ImageBlob");
                    OnPropertyChanged("ImageSource");
                }
            } 
        }
        public virtual AlimentTypeEnum AlimentType { get; }

        private AlimentUnitEnum unit;
        public AlimentUnitEnum Unit 
        { 
            get 
            {
                return unit;
            } 
            set 
            { 
                if(unit != value)
                {
                    unit = value;
                    OnPropertyChanged("Unit");
                    OnPropertyChanged("ServingSizeWithUnit");
                }
            } 
        }

        private void CalculateCalories()
        {
            Calories = Proteins * 4 + Carbs * 4 + Fats * 9;
        }

        [Ignore]
        public double CaloriesProgress
        {
            get
            { 
                return calories / App.RefData.User.TDEE; 
            }
        }
        [Ignore]
        public double FatsProgress
        {
            get
            { 
                return fats / App.RefData.User.TargetFats; 
            }
        }
        [Ignore]
        public double CarbsProgress
        {
            get 
            { 
                return carbs / App.RefData.User.TargetCarbs; 
            } 
        }
        [Ignore]
        public double ProteinsProgress
        { 
            get 
            { 
                return proteins / App.RefData.User.TargetProteins;
            }
        }



        [Ignore]
        public string ServingSizeWithUnit 
        { 
            get
            {
                return $"{ServingSize} {Unit}";
            } 
        }
        [Ignore]
        public string NutritionValuesString 
        { 
            get
            { 
                return $"P: {Math.Round(Proteins, 2)},  C: {Math.Round(Carbs, 2)},  F: {Math.Round(Fats, 2)}"; 
            } 
        }
        [Ignore]
        public string CaloriesString 
        { 
            get
            {
                return $"{Math.Round(Calories, 2)} Kcal";
            } 
        }
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
