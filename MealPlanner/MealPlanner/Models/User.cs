﻿using MealPlanner.Helpers.Enums;
using SkiaSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using ZXing.Aztec.Internal;

namespace MealPlanner.Models
{
    public class User : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        private double height;
        public double Height 
        {
            get => height;
            set 
            { 
                if(height != value)
                {
                    height = value;
                    Calcul();
                }
            }
        }

        private double weight;
        public double Weight 
        { 
            get => weight; 
            set 
            {
                if(weight != value)
                {
                    weight = value; 
                    Calcul();
                }
            } 
        }
        private HeightUnitEnum heightUnit;
        public HeightUnitEnum HeightUnit
        {
            get
            {
                return heightUnit;
            }
            set
            {
                if (heightUnit != value)
                {
                    heightUnit = value;
                    Calcul();
                }
            }
        }
        private WeightUnitEnum weightUnit;
        public WeightUnitEnum WeightUnit
        {
            get
            {
                return weightUnit;  
            }
            set
            {
                if(weightUnit != value)
                {
                    weightUnit = value;
                    Calcul();
                }
            }
        }      
        private double bodyFat;
        public double BodyFat 
        {
            get => bodyFat;
            set 
            {
                if(bodyFat != value)
                {
                    bodyFat = value; 
                    Calcul();
                }
            } 
        }

        private int age;
        public int Age 
        {
            get => age;
            set 
            { 
                if(age != value)
                {
                    age = value;
                    Calcul();
                }
            } 
        }

        private GenderEnum gender;
        public GenderEnum Gender 
        {
            get => gender; 
            set 
            { 
                if(gender != value)
                {
                    gender = value;
                    Calcul();
                }
            }
        }

        public double TargetProteins 
        { 
            get 
            { 
                return Math.Round((TDEE * SelectedTypeOfRegime.ProteinPercentage) / 4, 0);
            } 
        } 
        public double TargetCarbs 
        {
            get 
            { 
                return Math.Round((TDEE * SelectedTypeOfRegime.CarbsPercentage) / 4, 0);
            }
        }
        public double TargetFats
        { 
            get 
            { 
                return Math.Round((TDEE * SelectedTypeOfRegime.FatsPercentage) / 9, 0); 
            } 
        }

        private double bmr;
        public double BMR
        { 
            get 
            { 
                return Math.Round(bmr, 0);
            } 
            set 
            { 
                if(bmr != value)
                {
                    bmr = value;
                    OnPropertyChanged(nameof(BMR));
                }
            } 
        }

        private double tdee;
        public double TDEE
        {
            get 
            { 
                return Math.Round(tdee, 0);
            }
            set 
            { 
                if(tdee != value)
                {
                    tdee = value;
                    OnPropertyChanged(nameof(TDEE));

                    OnPropertyChanged(nameof(TargetProteins));
                    OnPropertyChanged(nameof(TargetFats));
                    OnPropertyChanged(nameof(TargetCarbs));
                    NotifyProgressBars();
                }
            }
        }

        public double AdjustedCalories { get; set; }

        public int CurrentJournalTemplateId { get; set; }

        public bool AutoGenerateJournalEnabled { get; set; }

        private EnergyUnitEnum energyUnit;
        public EnergyUnitEnum EnergyUnit { get; set; }
        public double CaloriesToKcal(double calories)
        {
            if (EnergyUnit == EnergyUnitEnum.kj)
                return calories * 4.184;

            return calories;
        }

        // Daily Calories
        private double dailyCalories;
        [Ignore]
        public double DailyCalories
        {
            get
            {
                return dailyCalories;
            }
            set
            {
                if(dailyCalories != value)
                {
                    dailyCalories = value;
                    OnPropertyChanged(nameof(DailyCalories));
                }
            }
        }
        [Ignore]
        public double DailyCaloriesProgress
        {
            get
            {
                return DailyCalories / TDEE;
            } 
        }

        // Daily Proteins
        private double dailyProteins;
        [Ignore]
        public double DailyProteins
        {
            get 
            { 
                return dailyProteins;
            }
            set
            {
                if(dailyProteins != value)
                {
                    dailyProteins = value;
                    OnPropertyChanged("DailyProteins");
                }
            }
        }
        [Ignore]
        public double DailyProteinProgress
        {
            get
            { 
                return DailyProteins / TargetProteins; 
            }
        }

        // Daily Carbs
        private double dailyCarbs;
        [Ignore]
        public double DailyCarbs
        {
            get 
            {
                return dailyCarbs;
            }
            set
            {
                if(dailyCarbs != value)
                {
                    dailyCarbs = value;
                    OnPropertyChanged("DailyCarbs");
                }
            }
        }
        [Ignore]
        public double DailyCarbsProgress 
        {
            get
            { 
                return DailyCarbs / TargetCarbs; 
            } 
        }

        // Daily Fats
        private double dailyFats;
        [Ignore]
        public double DailyFats
        {
            get 
            { 
                return dailyFats;
            }
            set
            {
                if(dailyFats != value)
                {
                    dailyFats = value;
                    OnPropertyChanged("DailyFats");
                }
            }
        }
        [Ignore]
        public double DailyFatsProgress
        { 
            get 
            { 
                return DailyFats / TargetFats; 
            } 
        }

        [Ignore]
        public string DailyProteinsRatio
        {
            get 
            { 
                return $"{Math.Round(DailyProteins, 0)}  /  {Math.Round(TargetProteins, 0)} g";
            } 
        }
        [Ignore]
        public string DailyCarbsRatio 
        {
            get
            { 
                return $"{Math.Round(DailyCarbs, 0)}  /  {Math.Round(TargetCarbs, 0)} g";
            } 
        }
        [Ignore]
        public string DailyFatsRatio 
        { 
            get
            { 
                return $"{Math.Round(DailyFats, 0)}  /  {Math.Round(TargetFats, 0 )} g";
            } 
        }

        private PALItem selectedPhysicalActivityLevel;
        [Ignore]
        public PALItem SelectedPhysicalActivityLevel 
        {
            get
            { 
                return selectedPhysicalActivityLevel;
            }
            set 
            { 
                if( selectedPhysicalActivityLevel != value )
                {
                    selectedPhysicalActivityLevel = value;
                    OnPropertyChanged("SelectedPhysicalActivityLevel");
                    Calcul();
                }
            } 
        }
        public PALItemTypeEnum SelectedPhysicalActivityLevelDB { get; set; }

        private string selectedBMRFormula;
        public string SelectedBMRFormula 
        { 
            get 
            { 
                return selectedBMRFormula; 
            } 
            set 
            { 
                if(selectedBMRFormula != value)
                {
                    selectedBMRFormula = value;
                    OnPropertyChanged("SelectedBMRFormula");
                    Calcul();
                }
            }
        }

        private ObjectifItem selectedObjectif;
        [Ignore]
        public ObjectifItem SelectedObjectif 
        {
            get 
            { 
                return selectedObjectif;
            } 
            set
            { 
                if(selectedObjectif != value)
                {
                    selectedObjectif = value;
                    OnPropertyChanged("SelectedObjectif");
                    Calcul();
                }
            }
        }
        public ObjectifTypeEnum SelectedObjectiflDB { get; set; }

        private TypeOfRegimeItem selectedTypeOfRegime;
        [Ignore]
        public TypeOfRegimeItem SelectedTypeOfRegime 
        {
            get 
            { 
                return selectedTypeOfRegime;
            }
            set
            {
                if(selectedTypeOfRegime != value)
                {
                    selectedTypeOfRegime = value;
                    OnPropertyChanged(nameof(SelectedTypeOfRegime));
                    NotifyTargetValues();
                    OnPropertyChanged(nameof(DailyProteinsRatio));
                    OnPropertyChanged(nameof(DailyCarbsRatio));
                    OnPropertyChanged(nameof(DailyFatsRatio));
                    NotifyProgressBars();
                }
            }
        }
        public TypesOfRegimeEnum SelectedTypeOfRegimeDB { get; set; }



        public User()
        {

        }

        private double WeightToKg(double weight)
        {
            if (weightUnit == WeightUnitEnum.lbs)
                return Weight / 2.22;

            return weight;
        }

        private double HeightToCm(double height)
        {
            if (heightUnit == HeightUnitEnum.ft_in)
                return Height * 30.48;

            return height;
        }

        private void Calcul()
        {
            if (SelectedBMRFormula == null || string.IsNullOrEmpty(Name) || SelectedObjectif == null || SelectedPhysicalActivityLevel == null)
                return;

            var weightInKg = WeightToKg(Weight);
            var heightInCm = HeightToCm(Height);


            if (SelectedBMRFormula == "Mifflin - St Jeor")
            {
                var s = Gender == GenderEnum.Female ? -165 : 5;
                BMR = (10 * weightInKg) + (6.25 * heightInCm) - (5 * Age) + s;
            }
            else if (SelectedBMRFormula == "Harris-Benedict")
            {
                BMR = Gender == GenderEnum.Female ? 655.1 + (9.563 * weightInKg) + (1.850 * heightInCm) - (4.676 * age)
                                                  : 66.5 + (13.75 * weightInKg) + (5.003 * heightInCm) - (6.75 * Age);
            }
            else if (SelectedBMRFormula == "Revised Harris-Benedict")
            {
                BMR = Gender == GenderEnum.Female ? 447.593 + (9.247 * weightInKg) + (3.098 * heightInCm) - (4.33 * age)
                                                  : 88.362 + (13.397 * weightInKg) + (4.799 * heightInCm) - (5.667 * Age);
            }
            else if (SelectedBMRFormula == "Katch-McArdle")
            {
                double LeanBodyMass = BodyFat != 0
                                    ? (weightInKg * (100 - BodyFat) / 100)
                                    : Gender == GenderEnum.Female
                                        ? (0.252 * weightInKg) + (0.473 * heightInCm) - 48.3
                                        : (0.407 * weightInKg) + (0.267 * heightInCm) - 19.2;

                BMR = 370 + (21.6 * LeanBodyMass);
            }
            else if (SelectedBMRFormula == "Schofield")
            {
                switch (Age)
                {
                    case int n when (n >= 18 && n < 30):
                        BMR = Gender == GenderEnum.Male ? 15.057 * weightInKg + 692.2 : 14.818 * weightInKg + 486.6;
                        break;

                    case int n when (n >= 30 && n <= 60):
                        BMR = Gender == GenderEnum.Male ? 11.472 * weightInKg + 873.1 : 8.126 * weightInKg + 845.6;
                        break;
                    case int n when (n > 60):
                        BMR = Gender == GenderEnum.Male ? 11.711 * weightInKg + 587.7 : 9.082 * weightInKg + 658.5;
                        break;
                }
            }

            TDEE = Math.Round((BMR * SelectedPhysicalActivityLevel.Ratio * SelectedObjectif.Ratio) + AdjustedCalories, 0);
        }

        /// <summary>
        /// OnPropertyChanged : DailyCaloriesProgress, DailyProteinProgress, DailyCarbsProgress, DailyFatsProgress
        /// </summary>
        public void NotifyProgressBars()
        {
            OnPropertyChanged(nameof(DailyCaloriesProgress));
            OnPropertyChanged(nameof(DailyProteinProgress));
            OnPropertyChanged(nameof(DailyCarbsProgress));
            OnPropertyChanged(nameof(DailyFatsProgress));

            OnPropertyChanged(nameof(DailyProteinsRatio));
            OnPropertyChanged(nameof(DailyCarbsRatio));
            OnPropertyChanged(nameof(DailyFatsRatio));
        }

        public void NotifyTargetValues()
        {
            OnPropertyChanged(nameof(TargetProteins));
            OnPropertyChanged(nameof(TargetCarbs));
            OnPropertyChanged(nameof(TargetFats));
        }


        public class ObjectifItem
        {
            public string Name { get; set; }
            public string Description { get; set; }     
            public ObjectifTypeEnum ObjectifType { get; set; }
            public double Ratio { get; set; }
            public bool IsSelected { get; set; }
        }
        public enum ObjectifTypeEnum
        {
            Lose_Weight_20,
            Lose_Weight_slowly_10,
            Maintain_Weight,
            Gain_Weight_slowly_10,
            Gain_Weight_20
        }

        public class PALItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public PALItemTypeEnum PALItemType { get; set; }
            public double Ratio { get; set; }
            public bool IsSelected { get; set; }
        }
        public enum PALItemTypeEnum
        {
            Little_none_exercise,
            Light_exercise,
            Moderate_exercise,
            Hard_exercise,
            PhysicalJob_hard_exercise,
            Professional_athelete
        }

        public class TypeOfRegimeItem : BaseModel
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            public TypesOfRegimeEnum TypeOfRegime { get; set; }
            public string Name { get; set; }
            public string Description
            {
                get
                {
                    return $"Proteins {proteinPercentage * 100}%, Carbs {CarbsPercentage * 100}%, Fats {FatsPercentage * 100}%";
                }
            }

            private double proteinPercentage;
            public double ProteinPercentage 
            {
                get
                {
                    return proteinPercentage;
                }
                set
                {
                    if(proteinPercentage != value)
                    {
                        proteinPercentage = value;
                        OnPropertyChanged(nameof(ProteinPercentage));
                        OnPropertyChanged(nameof(Description));
                    }
                }
            }

            private double carbsPercentage;
            public double CarbsPercentage 
            { 
                get
                {
                    return carbsPercentage;
                }
                set
                {
                    if(carbsPercentage != value)
                    {
                        carbsPercentage = value;
                        OnPropertyChanged(nameof(CarbsPercentage));
                        OnPropertyChanged(nameof(Description));
                    }
                }
            }

            private double fatsPercentage;
            public double FatsPercentage 
            { 
                get
                {
                    return fatsPercentage;
                }
                set
                {
                    if(fatsPercentage != value)
                    {
                        fatsPercentage = value;
                        OnPropertyChanged(nameof(FatsPercentage));
                        OnPropertyChanged(nameof(Description));
                    }
                }
            }

            [Ignore]
            public bool IsSelected { get; set; }
        }
        public enum TypesOfRegimeEnum
        {
            Standard,
            Balanced,
            LowInFats,
            RichInProteins,
            Keto,
            Custom
        }
    }
}
