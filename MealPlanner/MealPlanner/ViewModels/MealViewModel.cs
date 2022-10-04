using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class MealViewModel : BaseViewModel
    {
        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        private double servingSize;
        public double ServingSize { get { return servingSize; } set { servingSize = value; OnPropertyChanged("ServingSize"); } }
        private string description;
        public string Description { get { return description; } set { description = value; OnPropertyChanged("Description"); } }
        private AlimentUnitEnum unit;
        public AlimentUnitEnum Unit { get { return unit; } set { unit = value; OnPropertyChanged("Unit"); } }
        private bool isNew;
        public bool IsNew { get { return isNew; } set { isNew = value; OnPropertyChanged("IsNew"); } }

        private Meal currentMeal;
        public Meal CurrentMeal { get { return currentMeal; } set { currentMeal = value; OnPropertyChanged("CurrentMeal"); } }


        public MealViewModel()
        {
            CurrentMeal = new Meal();
            IsNew = true;

            // Add id
            var lastMeal = RefData.Meals.OrderByDescending(x => x.Id).FirstOrDefault();
            if (lastMeal != null)
                CurrentMeal.Id = lastMeal.Id + 1;
            else
                CurrentMeal.Id = 1;

            AddFoodCommand = new Command(AddFood);
            SaveCommand = new Command(SaveFood);
            UpdateCommand = new Command(UpdateMeal);    
        }

        public ICommand SaveCommand { get; set; }

        private async void SaveFood()
        {
            CurrentMeal.OriginalServingSize = CurrentMeal.ServingSize;

            //Save foods in db
            foreach(Food food in CurrentMeal.Foods)
            {
                MealFood mealFood = new MealFood();
                mealFood.MealId = CurrentMeal.Id;
                mealFood.FoodId = food.Id;
                mealFood.ServingSize = food.ServingSize;
                await App.DataBaseRepo.AddMealFoodAsync(mealFood);
            }

            App.RefData.Meals.Add(CurrentMeal);
            App.RefData.Aliments.Add(CurrentMeal);
            await App.DataBaseRepo.AddMealAsync(CurrentMeal);
            await Application.Current.MainPage.Navigation.PopAsync();
        }


        public ICommand UpdateCommand { get; set; }

        private async void UpdateMeal()
        {
            if (CurrentMeal == null)
                return;

            CurrentMeal.OriginalServingSize = CurrentMeal.ServingSize;

            await App.DataBaseRepo.UpdateMealAsync(CurrentMeal);


            // TODO Add food to db if any new
            var lastFood = CurrentMeal.Foods.OrderByDescending(x=> x.MealFoodId + 1).FirstOrDefault();

            var food = lastFood != null ? RefData.MealFoods.Where(f => f.Id == lastFood.MealFoodId).FirstOrDefault() : null;

            MealFood mealFood = null;

            if (food == null)
            {
                mealFood = new MealFood();
                mealFood.MealId = CurrentMeal.Id;
                mealFood.FoodId = lastFood.Id;
                mealFood.ServingSize = lastFood.ServingSize;
            }


            // TODO Refresh meal in DayMeals
            foreach (DayMeal dayMeal in RefData.DayMeals)
            {
                dayMeal.Calories = 0;
                dayMeal.Proteins = 0;
                dayMeal.Carbs = 0;
                dayMeal.Fats = 0;
                double ratio = 1;

                foreach (Meal meal in dayMeal.Aliments)
                {
                    if (meal.Id == CurrentMeal.Id)
                    {
                        ratio = meal.OriginalServingSize / CurrentMeal.OriginalServingSize;

                        meal.Name = CurrentMeal.Name;
                        meal.OriginalServingSize = CurrentMeal.ServingSize;
                        meal.Unit = CurrentMeal.Unit;


                        meal.Calories = meal.Calories * ratio;
                        meal.Proteins = meal.Proteins * ratio;
                        meal.Carbs = meal.Carbs * ratio;
                        meal.Fats = meal.Fats * ratio;
                    }

                    dayMeal.Calories += meal.Calories;
                    dayMeal.Proteins += meal.Proteins;
                    dayMeal.Carbs += meal.Carbs;
                    dayMeal.Fats += meal.Fats;
                }
            }

            // Update daily values
            RefData.UpdateDailyValues();

            if (mealFood != null) { await App.DataBaseRepo.AddMealFoodAsync(mealFood); };
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public ICommand AddFoodCommand { get; set; }
        public void AddFood()
        {
            AddAlimentPage addAlimentPage = new AddAlimentPage();
            (addAlimentPage.BindingContext as AddAlimentViewModel).CurrentMeal = this.CurrentMeal;
            (addAlimentPage.BindingContext as AddAlimentViewModel).MealSwitchVisibility = false;
            App.Current.MainPage.Navigation.PushAsync(addAlimentPage);
        }
    }
}
