using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.ViewModels
{
    public class AlimentPopUpViewModel : BaseViewModel
    {
        public AlimentPopUpViewModel(IAliment aliment)
        {
            AlimentCalories = aliment.Calories;
            AlimentProteins = aliment.Proteins;
            AlimentCarbs = aliment.Carbs;
            AlimentFats = aliment.Fats;
            AlimentPortion = aliment.Portion;
            AlimentUnit = aliment.Unit;
            AlimentNumberOfPortions = aliment.NumberOfPortions;
        }


        private double alimentCalories;
        public double AlimentCalories
        {
            get
            {
                return alimentCalories;
            }
            set
            {
                alimentCalories = value;
                AlimentCaloriesProgress = alimentCalories / RefData.User.TargetCalories;
                OnPropertyChanged("AlimentCalories");
                OnPropertyChanged("AlimentCaloriesProgress");
            }
        }
        public double AlimentCaloriesProgress { get; set; }


        private double alimentProteins;
        public double AlimentProteins
        {
            get { return alimentProteins; }
            set
            {
                alimentProteins = value;
                AlimentProteinProgress = ((alimentProteins * 4) / AlimentCalories) * 100;
                OnPropertyChanged("AlimentProteins");
                OnPropertyChanged("AlimentProteinProgress");
            }
        }
        public double AlimentProteinProgress { get; set; }


        private double alimentCarbs;
        public double AlimentCarbs
        {
            get { return alimentCarbs; }
            set
            {
                alimentCarbs = value;
                AlimentCarbsProgress = ((alimentCarbs * 4) / AlimentCalories) * 100;
                OnPropertyChanged("AlimentCarbs");
                OnPropertyChanged("AlimentCarbsProgress");
            }
        }
        public double AlimentCarbsProgress { get; set; }


        private double alimentFats;
        public double AlimentFats
        {
            get { return alimentFats; }
            set
            {
                alimentFats = value;
                AlimentFatsProgress = ((alimentFats * 9) / AlimentCalories) * 100;
                OnPropertyChanged("AlimentFats");
                OnPropertyChanged("AlimentFatsProgress");
            }
        }
        public double AlimentFatsProgress { get; set; }


        public double AlimentPortion { get;set; }
        public double AlimentNumberOfPortions { get; set; }

        public AlimentUnitEnum AlimentUnit { get; set; }
    }
}
