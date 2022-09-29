using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
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
        public string Name { get; set; }    
        public double ServingSize { get; set; } 
        public string Description { get; set; }
        public AlimentUnitEnum Unit { get; set; }


        public Meal CurrentMeal { get; set; }

        public MealViewModel()
        {
            CurrentMeal = new Meal();

            // Add id
            var lastMeal = RefData.Meals.OrderByDescending(x => x.Id).FirstOrDefault();
            if (lastMeal != null)
                CurrentMeal.Id = lastMeal.Id + 1;
            else
            {
                CurrentMeal.Id = 1;
            }

            SaveCommand = new Command(SaveFood);
        }


        public ICommand SaveCommand { get; set; }

        private async void SaveFood()
        {
            CurrentMeal.Name = Name;
            CurrentMeal.OriginalServingSize = ServingSize;
            CurrentMeal.ServingSize = ServingSize;
            CurrentMeal.Unit = Unit;
            CurrentMeal.Description = Description;

            App.RefData.Meals.Add(CurrentMeal);
            App.RefData.Aliments.Add(CurrentMeal);
            await App.DataBaseRepo.AddMealAsync(CurrentMeal);
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
