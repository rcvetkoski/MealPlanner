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


            // TODO Refresh meal in DayMeals
            foreach (DayMeal dayMeal in RefData.DayMeals)
            {
                dayMeal.Calories = 0;
                dayMeal.Proteins = 0;
                dayMeal.Carbs = 0;
                dayMeal.Fats = 0;
                double ratio = 1;

                foreach (Aliment aliment in dayMeal.Aliments)
                {
                    if (aliment.AlimentType == AlimentTypeEnum.Meal && aliment.Id == CurrentMeal.Id)
                    {
                        ratio = aliment.OriginalServingSize / CurrentMeal.OriginalServingSize;

                        aliment.Name = CurrentMeal.Name;
                        aliment.OriginalServingSize = CurrentMeal.ServingSize;
                        aliment.Unit = CurrentMeal.Unit;


                        aliment.Calories = aliment.Calories * ratio;
                        aliment.Proteins = aliment.Proteins * ratio;
                        aliment.Carbs = aliment.Carbs * ratio;
                        aliment.Fats = aliment.Fats * ratio;
                    }

                    dayMeal.Calories += aliment.Calories;
                    dayMeal.Proteins += aliment.Proteins;
                    dayMeal.Carbs += aliment.Carbs;
                    dayMeal.Fats += aliment.Fats;
                }
            }

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
