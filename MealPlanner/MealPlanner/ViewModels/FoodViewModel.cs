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
        public double Portion { get; set; }
        public string Description { get; set; }

        private double proteins;
        public double Proteins { get { return proteins; } set { proteins = value; OnPropertyChanged("Proteins"); }
        }

        private double carbs;
        public double Carbs { get { return carbs; } set { carbs = value; OnPropertyChanged("Carbs"); } }

        private double fats;
        public double Fats { get { return fats; } set { fats = value; OnPropertyChanged("Fats"); } }

        private double calories;
        public double Calories { get { return calories; } set { calories = value; OnPropertyChanged("Calories"); } }





        public ICommand SaveCommand { get; set; }

        public FoodViewModel()
        {
            SaveCommand = new Command(SaveFood);
        }

        private async void SaveFood()
        {
            Food food = new Food();
            food.Name = Name;
            food.Proteins = Proteins;
            food.Carbs = Carbs;
            food.Fats = Fats;
            food.Calories = Calories;

            await App.DataBaseRepo.AddFoodAsync(food);
            App.RefData.Foods.Add(food);
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
