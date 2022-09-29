using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class FoodViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public double ServingSize { get; set; }
        public AlimentUnitEnum Unit { get; set; }
        public string Description { get; set; }

        private double proteins;
        public double Proteins { get { return proteins; } set { proteins = value; ProteinsProgress = proteins / RefData.User.TargetProteins; OnPropertyChanged("Proteins"); OnPropertyChanged("ProteinsProgress"); CalculateCalories(); } }
        public double ProteinsProgress { get; set; }

        private double carbs;
        public double Carbs { get { return carbs; } set { carbs = value; CarbsProgress = carbs / RefData.User.TargetCarbs; OnPropertyChanged("Carbs"); OnPropertyChanged("CarbsProgress"); CalculateCalories(); } }
        public double CarbsProgress { get; set; }


        private double fats;
        public double Fats { get { return fats; } set { fats = value; FatsProgress = fats / RefData.User.TargetFats; OnPropertyChanged("Fats"); OnPropertyChanged("FatsProgress"); CalculateCalories(); } }
        public double FatsProgress { get; set; }


        private double calories;
        public double Calories { get { return calories; } set { calories = value; CaloriesProgress = calories / RefData.User.TargetCalories; OnPropertyChanged("Calories"); OnPropertyChanged("CaloriesProgress"); } }
        public double CaloriesProgress { get; set; }

        private void CalculateCalories()
        {
            Calories = Proteins * 4 + Carbs * 4 + Fats * 9;
        }

        public FoodViewModel()
        {
            Title = "Food";
            SaveCommand = new Command(SaveFood);
        }


        public ICommand SaveCommand { get; set; }

        private async void SaveFood()
        {
            Food food = new Food();
            food.Name = Name;
            food.Proteins = Proteins;
            food.Carbs = Carbs;
            food.Fats = Fats;
            food.Calories = Calories;
            food.OriginalServingSize = ServingSize;
            food.ServingSize = ServingSize;
            food.Unit = Unit;

            await App.DataBaseRepo.AddFoodAsync(food);
            App.RefData.Foods.Add(food);
            App.RefData.Aliments.Add(food);
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
