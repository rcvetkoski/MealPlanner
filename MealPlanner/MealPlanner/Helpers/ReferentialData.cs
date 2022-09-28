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
        public ObservableCollection<IAliment> Aliments { get; set; }




        public ReferentialData()
        {
            //var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //var path = Path.Combine(basePath, "MealPlanner.db3");
            //if (File.Exists(path))
            //{
            //    File.Delete(path);
            //}

            InitDB();
        }

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
            Aliments = new ObservableCollection<IAliment>();

            foreach (Meal meal in Meals)
                Aliments.Add(meal as IAliment);

            foreach (Food food in Foods)
                Aliments.Add(food as IAliment);


            // Add aliments to DayMeal if any
            var dayMealAliments = App.DataBaseRepo.GetAllDayMealAlimentsAsync().Result;
            foreach(DayMealAliment item in dayMealAliments)
            {
                DayMeal dayMeal = DayMeals.Where(x => x.Id == item.DayMealId).FirstOrDefault();
                IAliment existingAliment = Aliments.Where(x => x.Id == item.AlimentId && x.AlimentType == item.AlimentType).FirstOrDefault();

                if(existingAliment != null)
                {
                    IAliment aliment = CreateAndCopyAlimentProperties(existingAliment);
                    aliment.ServingSize = item.ServingSize;

                    dayMeal?.Aliments.Add(aliment);

                    DaylyProteins += aliment.Proteins;
                    DaylyCarbs += aliment.Carbs;
                    DaylyFats += aliment.Fats;
                    DaylyCalories += aliment.Calories;
                }
            }

            // Add foods to Meal
            var MealFoods = App.DataBaseRepo.GetAllMealFoodsAsync().Result;
            foreach (MealFood mealFood in MealFoods)
            {
                Meal meal = Meals.Where(x=> x.Id == mealFood.MealId).FirstOrDefault();
                Food food = Foods.Where(x => x.Id == mealFood.FoodId).FirstOrDefault();

                if(meal != null)
                    meal.Foods.Add(food);
            }
        }

        public IAliment CreateAndCopyAlimentProperties(IAliment existingAliment)
        {
            IAliment aliment;

            if (existingAliment.AlimentType == Enums.AlimentTypeEnum.Meal)
                aliment = new Meal();
            else
                aliment = new Food();


            // Fill properties
            aliment.Id = existingAliment.Id;
            aliment.Name = existingAliment.Name;
            aliment.Unit = existingAliment.Unit;
            aliment.Proteins = existingAliment.Proteins;
            aliment.Carbs = existingAliment.Carbs;
            aliment.Fats = existingAliment.Fats;
            aliment.Calories = existingAliment.Calories;

            return aliment;
        }


        private void DayMeals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DaylyProteins += (e.NewItems[0] as DayMeal).Proteins;
            DaylyCarbs += (e.NewItems[0] as DayMeal).Carbs;
            DaylyFats += (e.NewItems[0] as DayMeal).Fats;
            DaylyCalories += (e.NewItems[0] as DayMeal).Calories;
        }



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
