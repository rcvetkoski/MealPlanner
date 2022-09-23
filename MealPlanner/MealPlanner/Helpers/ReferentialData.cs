﻿using MealPlanner.Helpers.Extensions;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MealPlanner.Helpers
{
    public class ReferentialData : INotifyPropertyChanged
    {
        public User User { get; set; }
        public ObservableCollection<DayMeal> DayMeals { get; set; }
        public ObservableCollection<Food> Foods { get; set; }
        public ObservableCollection<MealFood> MealFoods { get; set; }



        public ReferentialData()
        {
            InitDB();
        }

        private void InitDB()
        {
            try
            {
                User =  App.DataBaseRepo.GetUserAsync().Result;
            }
            catch(Exception ex)
            {
                User = new User() { Age = 32, Height = 180, Weight = 69, TargetCalories = 2986, TargetProteins = 300, TargetCarbs = 323, TargetFats = 89 };
            }


            Foods = App.DataBaseRepo.GetAllFoodsAsync().Result.ToObservableCollection();

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



        private double daylyCalories;
        public double DaylyCalories
        {
            get
            {
                return daylyCalories;
            }
            set
            {
                daylyCalories = value;
                DaylyCaloriesProgress = daylyCalories / User.TargetCalories;
                OnPropertyChanged("DaylyCalories");
                OnPropertyChanged("DaylyCaloriesProgress");
            }
        }
        public double DaylyCaloriesProgress { get; set; }


        private double daylyProteins;
        public double DaylyProteins
        {
            get { return daylyProteins; }
            set
            {
                daylyProteins = value;
                DaylyProteinProgress = daylyProteins / User.TargetProteins;
                OnPropertyChanged("DaylyProteins");
                OnPropertyChanged("DaylyProteinProgress");
            }
        }
        public double DaylyProteinProgress { get; set; }


        private double daylyCarbs;
        public double DaylyCarbs
        {
            get { return daylyCarbs; }
            set
            {
                daylyCarbs = value;
                DaylyCarbsProgress = daylyCarbs / User.TargetCarbs;
                OnPropertyChanged("DaylyCarbs");
                OnPropertyChanged("DaylyCarbsProgress");
            }
        }
        public double DaylyCarbsProgress { get; set; }


        private double daylyFats;
        public double DaylyFats
        {
            get { return daylyFats; }
            set
            {
                daylyFats = value;
                DaylyFatsProgress = daylyFats / User.TargetFats;
                OnPropertyChanged("DaylyFats");
                OnPropertyChanged("DaylyFatsProgress");
            }
        }
        public double DaylyFatsProgress { get; set; }
    }
}
