using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.RSControls.Controls;

namespace MealPlanner.ViewModels
{
    public class FoodViewModel : BaseViewModel
    {
        public int Id { get; set; } 
        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        private double servingSize;
        public double ServingSize { get { return servingSize; } set { servingSize = value; OnPropertyChanged("ServingSize"); } }

        private AlimentUnitEnum unit;
        public AlimentUnitEnum Unit { get { return unit; } set { unit = value; OnPropertyChanged("Unit"); } }

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

        private bool isNew;
        public bool IsNew { get { return isNew; } set { isNew = value; OnPropertyChanged("IsNew"); } }

        public FoodViewModel()
        {
            Title = "Food";
            SaveCommand = new Command(SaveFood);
            UpdateCommand = new Command(UpdateFood);    
            IsNew = true;
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

        public ICommand UpdateCommand { get; set; }

        private async void UpdateFood()
        {
            Food food = RefData.Foods.Where(x => x.Id == Id).FirstOrDefault();

            if (food == null)
                return;

            food.Name = Name;
            food.Proteins = Proteins;
            food.Carbs = Carbs;
            food.Fats = Fats;
            food.Calories = Calories;
            food.OriginalServingSize = ServingSize;
            food.ServingSize = ServingSize;
            food.Unit = Unit;

            await App.DataBaseRepo.UpdateFoodAsync(food);

            // Refresh food if used
            foreach(DayMeal dayMeal in RefData.DayMeals)
            {
                foreach (Aliment aliment in dayMeal.Aliments)
                {
                    dayMeal.Calories -= aliment.Calories;

                    if (aliment.AlimentType == AlimentTypeEnum.Food && aliment.Id == Id)
                    {
                        double ratio = aliment.ServingSize / food.ServingSize;


                        aliment.Name = food.Name;
                        aliment.Proteins = food.Proteins * ratio;
                        aliment.Carbs = food.Carbs * ratio;
                        aliment.Fats = food.Fats * ratio;
                        aliment.Calories = food.Calories * ratio;
                        aliment.OriginalServingSize = food.ServingSize;
                        aliment.Unit = food.Unit;
                    }

                    dayMeal.Calories += aliment.Calories;
                }
            }

            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
