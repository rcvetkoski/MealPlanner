﻿using MealPlanner.Helpers.Extensions;
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

        private void InitDB()
        {
            // User
            try
            {
                User =  App.DataBaseRepo.GetUserAsync().Result;
            }
            catch(Exception ex)
            {
                User = new User() { Age = 32, Height = 180, Weight = 69, TargetCalories = 2986, TargetProteins = 300, TargetCarbs = 323, TargetFats = 89 };
            }


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
                    DailyProteins += aliment.Proteins;
                    DailyCarbs += aliment.Carbs;
                    DailyFats += aliment.Fats;
                    DailyCalories += aliment.Calories;
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
                DailyCaloriesProgress = dailyCalories / User.TargetCalories;
                OnPropertyChanged("DailyCalories");
                OnPropertyChanged("DailyCaloriesProgress");
            }
        }
        public double DailyCaloriesProgress { get; set; }


        private double dailyProteins;
        public double DailyProteins
        {
            get { return dailyProteins; }
            set
            {
                dailyProteins = value;
                DailyProteinProgress = dailyProteins / User.TargetProteins;
                OnPropertyChanged("DailyProteins");
                OnPropertyChanged("DailyProteinProgress");
            }
        }
        public double DailyProteinProgress { get; set; }


        private double dailyCarbs;
        public double DailyCarbs
        {
            get { return dailyCarbs; }
            set
            {
                dailyCarbs = value;
                DailyCarbsProgress = dailyCarbs / User.TargetCarbs;
                OnPropertyChanged("DailyCarbs");
                OnPropertyChanged("DailyCarbsProgress");
            }
        }
        public double DailyCarbsProgress { get; set; }


        private double dailyFats;
        public double DailyFats
        {
            get { return dailyFats; }
            set
            {
                dailyFats = value;
                DailyFatsProgress = dailyFats / User.TargetFats;
                OnPropertyChanged("DailyFats");
                OnPropertyChanged("DailyFatsProgress");
            }
        }
        public double DailyFatsProgress { get; set; }


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

            DailyProteins = proteins;
            DailyCarbs = carbs;
            DailyFats = fats;
            DailyCalories = calories;
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
