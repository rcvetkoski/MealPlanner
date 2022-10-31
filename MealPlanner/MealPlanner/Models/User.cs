using MealPlanner.Helpers.Enums;
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
        public double Weight { get => weight; 
            set 
            {
                if(weight != value)
                {
                    weight = value; 
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
                    OnPropertyChanged(nameof(DailyProteinsRatio));
                    OnPropertyChanged(nameof(DailyCarbsRatio));
                    OnPropertyChanged(nameof(DailyFatsRatio));
                }
            }
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
                return $"{Math.Round(DailyProteins, 0)}  /  {Math.Round(TargetProteins, 0)}";
            } 
        }
        [Ignore]
        public string DailyCarbsRatio 
        {
            get
            { 
                return $"{Math.Round(DailyCarbs, 0)}  /  {Math.Round(TargetCarbs, 0)}";
            } 
        }
        [Ignore]
        public string DailyFatsRatio 
        { 
            get
            { 
                return $"{Math.Round(DailyFats, 0)}  /  {Math.Round(TargetFats, 0 )}";
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
                    OnPropertyChanged("SelectedTypeOfRegime");
                    OnPropertyChanged("TargetFats");
                    OnPropertyChanged("TargetCarbs");
                    OnPropertyChanged("TargetFats");
                    OnPropertyChanged("DailyProteinsRatio");
                    OnPropertyChanged("DailyCarbsRatio");
                    OnPropertyChanged("DailyFatsRatio");
                    NotifyProgressBars();
                }
            }
        }
        public TypesOfRegimeEnum SelectedTypeOfRegimeDB { get; set; }


        public User()
        {

        }

        private void Calcul()
        {
            if (SelectedBMRFormula == null || string.IsNullOrEmpty(Name) || SelectedObjectif == null || SelectedPhysicalActivityLevel == null)
                return;

            if (SelectedBMRFormula == "Mifflin - St Jeor")
            {
                var s = Gender == GenderEnum.Female ? -165 : 5;
                BMR = (10 * Weight) + (6.25 * Height) - (5 * Age) + s;
            }
            else if (SelectedBMRFormula == "Harris-Benedict")
            {
                BMR = Gender == GenderEnum.Female ? 655.1 + (9.563 * Weight) + (1.850 * Height) - (4.676 * age)
                                                  : 66.5 + (13.75 * Weight) + (5.003 * Height) - (6.75 * Age);
            }
            else if (SelectedBMRFormula == "Revised Harris-Benedict")
            {
                BMR = Gender == GenderEnum.Female ? 447.593 + (9.247 * Weight) + (3.098 * Height) - (4.33 * age)
                                                  : 88.362 + (13.397 * Weight) + (4.799 * Height) - (5.667 * Age);
            }
            else if (SelectedBMRFormula == "Katch-McArdle")
            {
                double LeanBodyMass = BodyFat != 0
                                    ? (Weight * (100 - BodyFat) / 100)
                                    : Gender == GenderEnum.Female
                                        ? (0.252 * Weight) + (0.473 * Height) - 48.3
                                        : (0.407 * Weight) + (0.267 * Height) - 19.2;

                BMR = 370 + (21.6 * LeanBodyMass);
            }
            else if (SelectedBMRFormula == "Schofield")
            {
                switch (Age)
                {
                    case int n when (n >= 18 && n < 30):
                        BMR = Gender == GenderEnum.Male ? 15.057 * Weight + 692.2 : 14.818 * Weight + 486.6;
                        break;

                    case int n when (n >= 30 && n <= 60):
                        BMR = Gender == GenderEnum.Male ? 11.472 * Weight + 873.1 : 8.126 * Weight + 845.6;
                        break;
                    case int n when (n > 60):
                        BMR = Gender == GenderEnum.Male ? 11.711 * Weight + 587.7 : 9.082 * Weight + 658.5;
                        break;
                }
            }

            TDEE = Math.Round((BMR * SelectedPhysicalActivityLevel.Ratio * SelectedObjectif.Ratio), 0);
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
        }




        public class ObjectifItem
        {
            public string Name { get; set; }
            public ObjectifTypeEnum ObjectifType { get; set; }
            public double Ratio { get; set; }
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


        public class TypeOfRegimeItem
        {
            public TypesOfRegimeEnum TypeOfRegime { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public double ProteinPercentage { get; set; }
            public double CarbsPercentage { get; set; }
            public double FatsPercentage { get; set; }
        }
        public enum TypesOfRegimeEnum
        {
            Standard,
            Balanced,
            LowInFats,
            RichInProteins,
            Keto
        }
    }
}
