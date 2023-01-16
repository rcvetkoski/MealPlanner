using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.ViewModels
{
    public class AlimentStatsViewModel : BaseViewModel
    {
        // Calories
        private double alimentCalories;
        public double AlimentCalories
        {
            get
            {
                return alimentCalories;
            }
            set
            {
                if (alimentCalories != value)
                {
                    alimentCalories = value;
                    AlimentCaloriesProgress = alimentCalories / RefData.User.TDEE;
                    OnPropertyChanged("AlimentCalories");
                    OnPropertyChanged("AlimentCaloriesProgress");
                    OnPropertyChanged(nameof(AlimentCaloriesRatio));
                }
            }
        }
        public double AlimentCaloriesProgress { get; set; }
        public string AlimentCaloriesRatio
        {
            get
            {
                return $"{Math.Round(alimentCalories, 0)} / {RefData.User.TDEE}";
            }
        }

        // Proteins
        private double alimentProteins;
        public double AlimentProteins
        {
            get
            {
                return alimentProteins;
            }
            set
            {
                if (alimentProteins != value)
                {
                    alimentProteins = value;
                    AlimentProteinProgress = ((alimentProteins * 4) / AlimentCalories);
                    OnPropertyChanged("AlimentProteins");
                    OnPropertyChanged("AlimentProteinProgress");
                    OnPropertyChanged("AlimentProteinPercentage");
                    OnPropertyChanged("AlimentProteinQuantity");
                    OnPropertyChanged(nameof(AlimentProteinsRatio));
                }
            }
        }
        public double AlimentProteinProgress { get; set; }
        public string AlimentProteinPercentage
        {
            get
            {
                return $"{Math.Round(AlimentProteinProgress * 100, 0)} %";
            }
        }
        public string AlimentProteinQuantity
        {
            get
            {
                return $"Protein ({Math.Round(AlimentProteins, 0)} g)";
            }
        }
        public string AlimentProteinsRatio
        {
            get
            {
                return $"{Math.Round(alimentProteins, 0)} / {RefData.User.TargetProteins}";
            }
        }

        // Carbs
        private double alimentCarbs;
        public double AlimentCarbs
        {
            get
            {
                return alimentCarbs;
            }
            set
            {
                if (alimentCarbs != value)
                {
                    alimentCarbs = value;
                    AlimentCarbsProgress = ((alimentCarbs * 4) / AlimentCalories);
                    OnPropertyChanged("AlimentCarbs");
                    OnPropertyChanged("AlimentCarbsProgress");
                    OnPropertyChanged("AlimentCarbsPercentage");
                    OnPropertyChanged("AlimentCarbsQuantity");
                    OnPropertyChanged(nameof(AlimentCarbsRatio));
                }
            }
        }
        public double AlimentCarbsProgress { get; set; }
        public string AlimentCarbsPercentage
        {
            get
            {
                return $"{Math.Round(AlimentCarbsProgress * 100, 0)} %";
            }
        }
        public string AlimentCarbsQuantity
        {
            get
            {
                return $"Carbs ({Math.Round(AlimentCarbs, 0)} g)";
            }
        }
        public string AlimentCarbsRatio
        {
            get
            {
                return $"{Math.Round(alimentCarbs, 0)} / {RefData.User.TargetCarbs}";
            }
        }

        // Fats
        private double alimentFats;
        public double AlimentFats
        {
            get
            {
                return alimentFats;
            }
            set
            {
                if (alimentFats != value)
                {
                    alimentFats = value;
                    AlimentFatsProgress = ((alimentFats * 9) / AlimentCalories);
                    OnPropertyChanged("AlimentFats");
                    OnPropertyChanged("AlimentFatsProgress");
                    OnPropertyChanged("AlimentFatsPercentage");
                    OnPropertyChanged("AlimentFatsQuantity");
                    OnPropertyChanged(nameof(AlimentFatsRatio));
                }
            }
        }
        public double AlimentFatsProgress { get; set; }
        public string AlimentFatsPercentage
        {
            get
            {
                return $"{Math.Round(AlimentFatsProgress * 100, 0)} %";
            }
        }
        public string AlimentFatsQuantity
        {
            get
            {
                return $"Fats ({Math.Round(AlimentFats, 0)} g)";
            }
        }
        public string AlimentFatsRatio
        {
            get
            {
                return $"{Math.Round(alimentFats, 0)} / {RefData.User.TargetFats}";
            }
        }


        private double alimentServingSize;
        public double AlimentServingSize
        {
            get
            {
                return alimentServingSize;
            }
            set
            {
                if (alimentServingSize != value)
                {
                    alimentServingSize = value;
                    UpdateNutrimentValues();
                    OnPropertyChanged("AlimentServingSize");
                }
            }
        }

        public AlimentUnitEnum AlimentUnit { get; set; }

        private void UpdateNutrimentValues()
        {
            double ratio = AlimentServingSize / CurrentAliment.OriginalServingSize;

            AlimentCalories = CurrentAliment.Calories * ratio;
            AlimentProteins = CurrentAliment.Proteins * ratio;
            AlimentCarbs = CurrentAliment.Carbs * ratio;
            AlimentFats = CurrentAliment.Fats * ratio;
        }

        public virtual void InitProperties(Aliment aliment)
        {
            AlimentCalories = aliment.Calories;
            AlimentProteins = aliment.Proteins;
            AlimentCarbs = aliment.Carbs;
            AlimentFats = aliment.Fats;
            AlimentServingSize = aliment.ServingSize;
            AlimentUnit = aliment.Unit;

            Title = $"{aliment.Name}";
        }
    }
}
