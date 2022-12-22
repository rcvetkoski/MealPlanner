using MealPlanner.Helpers;
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

        private double fibers;
        public double Fibers 
        {
            get
            {
                return fibers;
            }
            set
            {
                if(fibers != value)
                {
                    fibers = value;
                    OnPropertyChanged(nameof(Fibers));
                }
            }
        }

        private double saturatedFat;
        public double SaturatedFat
        { 
            get
            {
                return saturatedFat;
            }
            set
            {
                if(saturatedFat != value)
                {
                    saturatedFat = value;
                    OnPropertyChanged(nameof(SaturatedFat));
                }
            }
        }

        private double sugars;
        public double Sugars 
        { 
            get
            {
                return sugars;
            }
            set
            {
                if(sugars != value)
                {
                    sugars = value;
                    OnPropertyChanged(nameof(Sugars));
                }
            }
        }

        private double salt;
        public double Salt
        { 
            get
            {
                return salt;
            }
            set
            {
                if(salt != value)
                {
                    salt = value;
                    OnPropertyChanged(nameof(Salt));
                }
            }
        }

        private double sodium;
        public double Sodium
        { 
            get
            {
                return sodium;
            }
            set
            {
                if(sodium != value)
                {
                    sodium = value;
                    OnPropertyChanged(nameof(Sodium));
                }
            }
        }


        public double ServingQuantity { get; set; }
        public string ServingQuantityUnit { get; set; }
        public double EnergyKcalServing { get; set; }
        public double CaloriesServing { get; set; }
        public double CarbsServing { get; set; }
        public double FatsServing { get; set; }
        public double SaturatedFatServing { get; set; }
        public double FibersServing { get; set; }
        public double ProteinsServing { get; set; }
        public double SugarsServing { get; set; }
        public double SaltServing { get; set; }
        public double SodiumServing { get; set; }



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

        public bool Archived { get; set; }

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
                return $"{Math.Round(App.RefData.User.CaloriesToKcal(Calories), 0)} {App.RefData.User.EnergyUnit}";
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
