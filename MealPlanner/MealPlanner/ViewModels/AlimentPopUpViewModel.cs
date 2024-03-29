﻿using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class AlimentPopUpViewModel : BaseViewModel
    {
        private Aliment aliment;

        public AlimentPopUpViewModel(Aliment selectedAliment)
        {
            var aliment = RefData.Aliments.Where(x=> x.Id == selectedAliment.Id && x.AlimentType == selectedAliment.AlimentType).FirstOrDefault();


            this.aliment = aliment;

            AlimentCalories = aliment.Calories;
            AlimentProteins = aliment.Proteins;
            AlimentCarbs = aliment.Carbs;
            AlimentFats = aliment.Fats;
            AlimentServingSize = selectedAliment.ServingSize;
            AlimentUnit = selectedAliment.Unit;
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
                if(alimentCalories != value)
                {
                    alimentCalories = value;
                    AlimentCaloriesProgress = alimentCalories / RefData.User.TDEE;
                    OnPropertyChanged("AlimentCalories");
                    OnPropertyChanged("AlimentCaloriesProgress");
                }
            }
        }
        public double AlimentCaloriesProgress { get; set; }


        private double alimentProteins;
        public double AlimentProteins
        {
            get 
            {
                return alimentProteins;
            }
            set
            {
                if( alimentProteins != value)
                {
                    alimentProteins = value;
                    AlimentProteinProgress = ((alimentProteins * 4) / AlimentCalories) * 100;
                    OnPropertyChanged("AlimentProteins");
                    OnPropertyChanged("AlimentProteinProgress");
                }
            }
        }
        public double AlimentProteinProgress { get; set; }


        private double alimentCarbs;
        public double AlimentCarbs
        {
            get
            {
                return alimentCarbs; 
            }
            set
            {
                if(alimentCarbs != value)
                {
                    alimentCarbs = value;
                    AlimentCarbsProgress = ((alimentCarbs * 4) / AlimentCalories) * 100;
                    OnPropertyChanged("AlimentCarbs");
                    OnPropertyChanged("AlimentCarbsProgress");
                }
            }
        }
        public double AlimentCarbsProgress { get; set; }


        private double alimentFats;
        public double AlimentFats
        {
            get
            { 
                return alimentFats;
            }
            set
            {
                if(alimentFats != value)
                {
                    alimentFats = value;
                    AlimentFatsProgress = ((alimentFats * 9) / AlimentCalories) * 100;
                    OnPropertyChanged("AlimentFats");
                    OnPropertyChanged("AlimentFatsProgress");
                }
            }
        }
        public double AlimentFatsProgress { get; set; }

        private double alimentServingSize;
        public double AlimentServingSize 
        { 
            get
            { 
                return alimentServingSize;
            }
            set 
            { 
                if(alimentServingSize != value)
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
            double ratio = AlimentServingSize / aliment.OriginalServingSize;

            AlimentCalories = aliment.Calories * ratio;
            AlimentProteins = aliment.Proteins * ratio;
            AlimentCarbs = aliment.Carbs * ratio;
            AlimentFats = aliment.Fats * ratio;
        }
    }
}
