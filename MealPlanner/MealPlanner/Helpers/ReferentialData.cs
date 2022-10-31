using MealPlanner.Helpers.Extensions;
using MealPlanner.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static MealPlanner.Models.User;

namespace MealPlanner.Helpers
{
    public class ReferentialData : INotifyPropertyChanged
    {
        public User User { get; set; }
        public ObservableCollection<DayMeal> DayMeals { get; set; }
        public ObservableCollection<Meal> Meals { get; set; }
        public ObservableCollection<Food> Foods { get; set; }
        public ObservableCollection<MealFood> MealFoods { get; set; }
        public ObservableCollection<Aliment> Aliments { get; set; }
        public ObservableCollection<Aliment> FilteredAliments { get; set; }
        public ObservableCollection<DayMealAliment> DayMealAliments { get; set; }

        // User
        public List<TypeOfRegimeItem> TypesOfRegime { get; set; }
        public List<PALItem> PhysicalActivityLevels { get; set; }
        public List<ObjectifItem> Objectifs { get; set; }
        public List<string> BMRFormulas { get; set; }


        public ReferentialData()
        {
            //ResetDB()
            ResetDBCommand = new Command(ResetDB);
            InitDB();
        }

        public void ResetDB()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var path = Path.Combine(basePath, "MealPlanner.db3");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public ICommand ResetDBCommand { get; set; }

        private void InitUser()
        {
            // Bmr formulas
            BMRFormulas = new List<string>();
            BMRFormulas.Add("Mifflin - St Jeor");
            BMRFormulas.Add("Harris-Benedict");
            BMRFormulas.Add("Revised Harris-Benedict");
            BMRFormulas.Add("Katch-McArdle");
            BMRFormulas.Add("Schofield");

            // Type of regimes
            TypesOfRegime = new List<TypeOfRegimeItem>();
            TypesOfRegime.Add(new TypeOfRegimeItem() 
            { 
                TypeOfRegime = TypesOfRegimeEnum.Standard,
                Name = "Standard",
                Description = "Carbs 50%, Proteins 20%, Fats 30%",
                CarbsPercentage = 0.5,
                ProteinPercentage = 0.2, 
                FatsPercentage = 0.3
            });
            TypesOfRegime.Add(new TypeOfRegimeItem()
            { 
                TypeOfRegime = TypesOfRegimeEnum.Balanced,
                Name = "Balanced",
                Description = "Carbs 50%, Proteins 25%, Fats 25%",
                CarbsPercentage = 0.5,
                ProteinPercentage = 0.25,
                FatsPercentage = 0.35
            });
            TypesOfRegime.Add(new TypeOfRegimeItem() 
            { 
                TypeOfRegime = TypesOfRegimeEnum.LowInFats,
                Name = "Low in fats",
                Description = "Carbs 60%, Proteins 25%, Fats 15%",
                CarbsPercentage = 0.6,
                ProteinPercentage = 0.25,
                FatsPercentage = 0.15
            });
            TypesOfRegime.Add(new TypeOfRegimeItem()
            {
                TypeOfRegime = TypesOfRegimeEnum.RichInProteins, 
                Name = "Rich in proteins",
                Description = "Carbs 25%, Proteins 40%, Fats 35%",
                CarbsPercentage = 0.25,
                ProteinPercentage = 0.4,
                FatsPercentage = 0.35
            });
            TypesOfRegime.Add(new TypeOfRegimeItem()
            { 
                TypeOfRegime = TypesOfRegimeEnum.Keto, 
                Name = "Keto", 
                Description = "Carbs 5%, Proteins 30%, Fats 65%",
                CarbsPercentage = 0.05,
                ProteinPercentage = 0.3,
                FatsPercentage = 0.65
            });

            // PAL
            PhysicalActivityLevels = new List<PALItem>();
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "little / no exercise",
                Description = "sedentary lifestyle",
                PALItemType = PALItemTypeEnum.Little_none_exercise,
                Ratio = 1.2
            });
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "light exercise",
                Description = "1 - 2 times / week",
                PALItemType = PALItemTypeEnum.Light_exercise,
                Ratio = 1.375
            });
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "moderate exercise",
                Description = "4 - 5 times / week",
                PALItemType = PALItemTypeEnum.Moderate_exercise,
                Ratio = 1.55
            });
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "hard exercise",
                Description = "4 - 5 times / week",
                PALItemType = PALItemTypeEnum.Hard_exercise,
                Ratio = 1.725
            });
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "physical job or hard exercise",
                Description = "6 - 7 times / week",
                PALItemType = PALItemTypeEnum.PhysicalJob_hard_exercise,
                Ratio = 1.9
            });
            PhysicalActivityLevels.Add(new PALItem()
            {
                Name = "professional athlete",
                Description = "",
                PALItemType = PALItemTypeEnum.Professional_athelete,
                Ratio = 2.4
            });

            // Objectifs
            Objectifs = new List<ObjectifItem>();
            Objectifs.Add(new ObjectifItem()
            {
                Name = "Lose Weight 20%",
                ObjectifType = ObjectifTypeEnum.Lose_Weight_20,
                Ratio = 0.8
            });
            Objectifs.Add(new ObjectifItem()
            {
                Name = "Lose Weight slowly 10%",
                ObjectifType = ObjectifTypeEnum.Lose_Weight_slowly_10,
                Ratio = 0.9
            });
            Objectifs.Add(new ObjectifItem()
            {
                Name = "Maintain Weight",
                ObjectifType = ObjectifTypeEnum.Maintain_Weight,
                Ratio = 1
            });
            Objectifs.Add(new ObjectifItem()
            {
                Name = "Gain Weight slowly 10%",
                ObjectifType = ObjectifTypeEnum.Gain_Weight_slowly_10,
                Ratio = 1.1
            });
            Objectifs.Add(new ObjectifItem()
            {
                Name = "Gain Weight 20%",
                ObjectifType = ObjectifTypeEnum.Gain_Weight_20,
                Ratio = 1.2
            });


            User = App.DataBaseRepo.GetUserAsync().Result;

            if (User == null)
            {
                User = new User();
                //User = new User() {Name = "Rade", Age = 32, Height = 180, Weight = 69, BodyFat = 14.5, Gender = Enums.GenderEnum.Male, TargetCalories = 2986, TargetProteins = 300, TargetCarbs = 323, TargetFats = 89 };
            }
            else
            {
                User.SelectedTypeOfRegime = TypesOfRegime.Where(x => x.TypeOfRegime == User.SelectedTypeOfRegimeDB).FirstOrDefault();
                User.SelectedPhysicalActivityLevel = PhysicalActivityLevels.Where(x => x.PALItemType == User.SelectedPhysicalActivityLevelDB).FirstOrDefault();
                User.SelectedObjectif = Objectifs.Where(x => x.ObjectifType == User.SelectedObjectiflDB).FirstOrDefault();
            }
        }

        private void InitDB()
        {
            // User
            InitUser();

            // Foods
            Foods = App.DataBaseRepo.GetAllFoodsAsync().Result.ToObservableCollection();

            // Meals
            Meals = App.DataBaseRepo.GetAllMealsAsync().Result.ToObservableCollection();

            // DayMeals
            var dayMeals = App.DataBaseRepo.GetAllDayMealsAsync().Result;
            if (dayMeals.Any())
            {
                DayMeals = dayMeals.OrderBy(x=> x.Order).ToList().ToObservableCollection();
            }
            else
            {
                DayMeals = new ObservableCollection<DayMeal>();

                // Breakfast
                var breakfast = new DayMeal() { Name = "Breakfast", Order = 1 };
                DayMeals.Add(breakfast);

                // Lunch
                var lunch = new DayMeal() { Name = "Lunch", Order = 2 };
                DayMeals.Add(lunch);

                // Dinner
                var dinner = new DayMeal() { Name = "Dinner", Order = 3 };
                DayMeals.Add(dinner);

                // Snacks
                var snack = new DayMeal() { Name = "Snack", Order = 4 };
                DayMeals.Add(snack);

                App.DataBaseRepo.AddDayMealAsync(breakfast);
                App.DataBaseRepo.AddDayMealAsync(lunch);
                App.DataBaseRepo.AddDayMealAsync(dinner);
                App.DataBaseRepo.AddDayMealAsync(snack);
            }


            // Aliments
            Aliments = new ObservableCollection<Aliment>();
            FilteredAliments = new ObservableCollection<Aliment>();

            foreach (Meal meal in Meals)
                Aliments.Add(meal as Aliment);

            foreach (Food food in Foods)
                Aliments.Add(food as Aliment);


            // Add foods to Meal
            MealFoods = App.DataBaseRepo.GetAllMealFoodsAsync().Result.ToObservableCollection();
            foreach (MealFood mealFood in MealFoods)
            {
                Meal meal = Aliments.Where(x=> x.Id == mealFood.MealId && x.AlimentType == Enums.AlimentTypeEnum.Meal).FirstOrDefault() as Meal;
                Food existingFood = Aliments.Where(x => x.Id == mealFood.FoodId && x.AlimentType == Enums.AlimentTypeEnum.Food).FirstOrDefault() as Food;

                var ratio = mealFood.ServingSize / existingFood.OriginalServingSize;
                Food food = CreateAndCopyAlimentProperties(existingFood, ratio) as Food;
                food.ServingSize = mealFood.ServingSize;
                food.MealFoodId = mealFood.Id;  

                if (meal != null)
                    meal.Foods.Add(food);
            }

            // Add aliments to DayMeal if any
            PopulateDayMeals();
        }

        private void PopulateDayMeals()
        {
            DayMealAliments = App.DataBaseRepo.GetAllDayMealAlimentsAsync().Result.ToObservableCollection();
            foreach (DayMealAliment dayMealAliment in DayMealAliments)
            {
                DayMeal dayMeal = DayMeals.Where(x => x.Id == dayMealAliment.DayMealId).FirstOrDefault();
                Aliment existingAliment = Aliments.Where(x => x.Id == dayMealAliment.AlimentId && x.AlimentType == dayMealAliment.AlimentType).FirstOrDefault();

                if (existingAliment != null)
                {
                    var ratio = dayMealAliment.ServingSize / existingAliment.OriginalServingSize;
                    Aliment aliment = CreateAndCopyAlimentProperties(existingAliment, ratio);
                    aliment.DayMealAlimentId = dayMealAliment.Id;
                    aliment.ServingSize = dayMealAliment.ServingSize;

                    dayMeal?.Aliments.Add(aliment);

                    // Update dayMeal values
                    UpdateDayMealValues(dayMeal);

                    // Update daily values
                    User.DailyProteins += aliment.Proteins;
                    User.DailyCarbs += aliment.Carbs;
                    User.DailyFats += aliment.Fats;
                    User.DailyCalories += aliment.Calories;
                }
            }
        }

        public Aliment CreateAndCopyAlimentProperties(Aliment existingAliment, double ratio = 1)
        {
            Aliment aliment;

            if (existingAliment.AlimentType == Enums.AlimentTypeEnum.Meal)
            {
                aliment = new Meal();
                (aliment as Meal).Description = (existingAliment as Meal).Description;
                foreach(Food food in (existingAliment as Meal).Foods)
                    (aliment as Meal).Foods.Add(food);
            }
            else
            {
                aliment = new Food();
            }


            // Fill properties
            aliment.Id = existingAliment.Id;
            aliment.Name = existingAliment.Name;
            aliment.ImageSourcePath = existingAliment.ImageSourcePath;
            aliment.ImageBlob = existingAliment.ImageBlob;
            aliment.Unit = existingAliment.Unit;
            aliment.Proteins = existingAliment.Proteins * ratio;
            aliment.ServingSize = existingAliment.ServingSize;
            aliment.OriginalServingSize = existingAliment.OriginalServingSize;
            aliment.Carbs = existingAliment.Carbs * ratio;
            aliment.Fats = existingAliment.Fats * ratio;
            aliment.Calories = existingAliment.Calories * ratio;


            return aliment;
        }

        public void UpdateDailyValues()
        {
            double proteins = 0;
            double carbs = 0;
            double fats = 0;
            double calories = 0;

            foreach (DayMeal dayMeal in DayMeals)
            {
                proteins += dayMeal.Proteins;
                carbs += dayMeal.Carbs;
                fats += dayMeal.Fats;
                calories += dayMeal.Calories;
            }

            User.DailyProteins = proteins;
            User.DailyCarbs = carbs;
            User.DailyFats = fats;
            User.DailyCalories = calories;

            User.NotifyProgressBars();
        }

        public void UpdateDayMealValues(DayMeal dayMeal, double ratio = 1)
        {
            double proteins = 0;
            double carbs = 0;
            double fats = 0;
            double calories = 0;

            foreach (Aliment aliment in dayMeal.Aliments)
            {
                proteins += aliment.Proteins;
                carbs += aliment.Carbs;
                fats += aliment.Fats;
                calories += aliment.Calories;
            }

            dayMeal.Proteins = proteins * ratio;
            dayMeal.Carbs = carbs * ratio;
            dayMeal.Fats = fats * ratio;
            dayMeal.Calories = calories * ratio;
        }

        public void UpdateMealValues(Meal meal, double ratio = 1)
        {
            double proteins = 0;
            double carbs = 0;
            double fats = 0;
            double calories = 0;

            foreach (Food food in meal.Foods)
            {
                proteins += food.Proteins;
                carbs += food.Carbs;
                fats += food.Fats;
                calories += food.Calories;
            }

            meal.Proteins = proteins * ratio;
            meal.Carbs = carbs * ratio;
            meal.Fats = fats * ratio;
            meal.Calories = calories * ratio;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
