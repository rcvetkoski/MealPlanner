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
        public double Height { get => height; set { height = value; Calcul(); } }

        private double weight;
        public double Weight { get => weight; set { weight = value; Calcul(); } }
        private double bodyFat;
        public double BodyFat { get => bodyFat; set { bodyFat = value; Calcul(); } }

        private int age;
        public int Age { get => age; set { age = value; Calcul(); } }

        private GenderEnum gender;
        public GenderEnum Gender { get => gender; set { gender = value; Calcul(); } }

        public double TargetProteins { get { return Math.Round((TDEE * SelectedTypeOfRegime.ProteinPercentage) / 4, 0); } } 
        public double TargetCarbs { get { return Math.Round((TDEE * SelectedTypeOfRegime.CarbsPercentage) / 4, 0); } }
        public double TargetFats { get { return Math.Round((TDEE * SelectedTypeOfRegime.FatsPercentage) / 9, 0); } }

        private double bmr;
        public double BMR { get { return Math.Round(bmr, 0); } set { bmr = value; OnPropertyChanged("BMR"); } }

        private double tdee;
        public double TDEE
        {
            get { return Math.Round(tdee, 0); }
            set 
            { 
                tdee = value; OnPropertyChanged("TDEE");

                OnPropertyChanged("TargetFats");
                OnPropertyChanged("TargetCarbs");
                OnPropertyChanged("TargetFats");

                OnPropertyChanged("DailyCaloriesProgress");
                OnPropertyChanged("DailyCarbsProgress");
                OnPropertyChanged("DailyFatsProgress");

                OnPropertyChanged("DailyProteinsRatio");
                OnPropertyChanged("DailyCarbsRatio");
                OnPropertyChanged("DailyFatsRatio");
            }
        }

        // Daily Calories
        private double dailyCalories;
        public double DailyCalories
        {
            get
            {
                return dailyCalories;
            }
            set
            {
                dailyCalories = value;
                OnPropertyChanged("DailyCalories");
            }
        }
        public double DailyCaloriesProgress { get { return DailyCalories / TDEE; } }

        // Daily Proteins
        private double dailyProteins;
        public double DailyProteins
        {
            get { return dailyProteins; }
            set
            {
                dailyProteins = value;
                OnPropertyChanged("DailyProteins");
            }
        }
        public double DailyProteinProgress { get { return DailyProteins / TargetProteins; } }

        // Daily Carbs
        private double dailyCarbs;
        public double DailyCarbs
        {
            get { return dailyCarbs; }
            set
            {
                dailyCarbs = value;
                OnPropertyChanged("DailyCarbs");
            }
        }
        public double DailyCarbsProgress { get { return DailyCarbs / TargetCarbs; } }

        // Daily Fats
        private double dailyFats;
        public double DailyFats
        {
            get { return dailyFats; }
            set
            {
                dailyFats = value;
                OnPropertyChanged("DailyFats");
            }
        }
        public double DailyFatsProgress { get { return DailyFats / TargetFats; } }

        public string DailyProteinsRatio { get { return $"{Math.Round(DailyProteins, 0)}  /  {Math.Round(TargetProteins, 0)}"; } }
        public string DailyCarbsRatio { get { return $"{Math.Round(DailyCarbs, 0)}  /  {Math.Round(TargetCarbs, 0)}"; } }
        public string DailyFatsRatio { get { return $"{Math.Round(DailyFats, 0)}  /  {Math.Round(TargetFats, 0 )}"; } }


        /// <summary>
        /// OnPropertyChanged : DailyCaloriesProgress, DailyProteinProgress, DailyCarbsProgress, DailyFatsProgress
        /// </summary>
        public void NotifyProgressBars()
        {
            OnPropertyChanged("DailyCaloriesProgress");
            OnPropertyChanged("DailyProteinProgress");
            OnPropertyChanged("DailyCarbsProgress");
            OnPropertyChanged("DailyFatsProgress");
        }

        [Ignore]
        public List<string> PhysicalActivityLevels { get; set; }

        private string selectedPhysicalActivityLevel;
        public string SelectedPhysicalActivityLevel { get { return selectedPhysicalActivityLevel; } set { selectedPhysicalActivityLevel = value; Calcul(); } }

        [Ignore]
        public List<string> BMRFormulas { get; set; }
        private string selectedBMRFormula;
        public string SelectedBMRFormula { get { return selectedBMRFormula; } set { selectedBMRFormula = value; Calcul(); } }
        private Dictionary<string, double> palValue;

        [Ignore]
        public List<string> Objectifs { get; set; }
        private string selectedObjectif;
        public string SelectedObjectif { get { return selectedObjectif; } set { selectedObjectif = value; Calcul(); } }
        private Dictionary<string, double> objectifValue;

        private TypeOfRegimeItem selectedTypeOfRegime;
        [Ignore]
        public TypeOfRegimeItem SelectedTypeOfRegime 
        {
            get { return selectedTypeOfRegime; }
            set
            {
                selectedTypeOfRegime = value; OnPropertyChanged("SelectedTypeOfRegime");
                OnPropertyChanged("TargetFats");
                OnPropertyChanged("TargetCarbs");
                OnPropertyChanged("TargetFats");
            }
        }
        public TypesOfRegimeEnum SelectedTypeOfRegimeDB { get; set; }


        public User()
        {
            PhysicalActivityLevels = new List<string>();
            BMRFormulas = new List<string>();
            palValue = new Dictionary<string, double>();
            Objectifs = new List<string>();
            objectifValue = new Dictionary<string, double>();


            // PAL
            PhysicalActivityLevels.Add("little / no exercise(sedentary lifestyle)");
            palValue.Add("little / no exercise(sedentary lifestyle)", 1.2);

            PhysicalActivityLevels.Add("light exercise 1 - 2 times / week)");
            palValue.Add("light exercise 1 - 2 times / week)", 1.375);

            PhysicalActivityLevels.Add("moderate exercise 2 - 3 times / week)");
            palValue.Add("moderate exercise 2 - 3 times / week)", 1.55);

            PhysicalActivityLevels.Add("hard exercise 4 - 5 times / week)");
            palValue.Add("hard exercise 4 - 5 times / week)", 1.725);

            PhysicalActivityLevels.Add("physical job or hard exercise 6 - 7 times / week)");
            palValue.Add("physical job or hard exercise 6 - 7 times / week)", 1.9);

            PhysicalActivityLevels.Add("professional athlete)");
            palValue.Add("professional athlete)", 2.4);


            // BMR FOrmulas
            BMRFormulas.Add("Mifflin - St Jeor");
            BMRFormulas.Add("Harris-Benedict");
            BMRFormulas.Add("Revised Harris-Benedict");
            BMRFormulas.Add("Katch-McArdle");
            BMRFormulas.Add("Schofield");


            // Objectifs
            Objectifs.Add("Lose Weight 20%");
            objectifValue.Add("Lose Weight 20%", 0.8);

            Objectifs.Add("Lose Weight slowly 10%");
            objectifValue.Add("Lose Weight slowly 10%", 0.9);

            Objectifs.Add("Maintain Weight");
            objectifValue.Add("Maintain Weight", 1);

            Objectifs.Add("Gain Weight slowly 10%");
            objectifValue.Add("Gain Weight slowly 10%", 1.1);

            Objectifs.Add("Gain Weight 20%");
            objectifValue.Add("Gain Weight 20%", 1.2);
        }

        private void Calcul()
        {
            BMR = 0;

            if (SelectedBMRFormula == null || string.IsNullOrEmpty(Name) || SelectedObjectif == null)
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

            TDEE = BMR * palValue[SelectedPhysicalActivityLevel] * objectifValue[SelectedObjectif];
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
            None,
            Standard,
            Balanced,
            LowInFats,
            RichInProteins,
            Keto
        }
    }
}
