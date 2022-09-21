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
        public double Proteins { get;set; } 
        public double Carbs { get; set; }   
        public double Fats { get; set; }    
        public double Calories { get; set; }    

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
