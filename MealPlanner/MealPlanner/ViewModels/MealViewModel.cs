using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.Services;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class MealViewModel : BaseViewModel
    {
        public MealViewModel()
        {
            IsNew = true;
            AddFoodCommand = new Command(AddFood);
            SaveCommand = new Command(SaveMeal);
            UpdateCommand = new Command(UpdateMeal);
            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
            DelettedMealFoods = new List<MealFood>();
        }


        /// <summary>
        /// Save Meal
        /// </summary>
        public ICommand SaveCommand { get; set; }
        private async void SaveMeal()
        {
            CurrentAliment.OriginalServingSize = CurrentAliment.ServingSize;
            RefData.Meals.Add(CurrentAliment as Meal);
            RefData.Aliments.Add(CurrentAliment as Meal);
            RefData.FilteredAliments.Add(CurrentAliment as Meal);
            await App.DataBaseRepo.AddMealAsync(CurrentAliment as Meal);

            //Save foods in db
            foreach (Food food in (CurrentAliment as Meal).Foods)
            {
                MealFood mealFood = new MealFood();
                mealFood.MealId = CurrentAliment.Id;
                mealFood.FoodId = food.Id;
                mealFood.ServingSize = food.ServingSize;
                await App.DataBaseRepo.AddMealFoodAsync(mealFood);
                food.MealFoodId = mealFood.Id;
                RefData.MealFoods.Add(mealFood);
            }

            // Update meal values
            RefData.UpdateMealValues(CurrentAliment as Meal);

            //await Shell.Current.GoToAsync("..");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Update Meal
        /// </summary>
        public ICommand UpdateCommand { get; set; }
        private async void UpdateMeal()
        {
            // Get real meal to update
            Meal originalMeal = RefData.Meals.Where(x => x.Id == CurrentAliment.Id).FirstOrDefault();

            if (originalMeal == null)
                return;

            originalMeal.Name = CurrentAliment.Name;
            originalMeal.Proteins = CurrentAliment.Proteins;
            originalMeal.ImageSourcePath = CurrentAliment.ImageSourcePath;
            originalMeal.ImageBlob = CurrentAliment.ImageBlob;
            originalMeal.Carbs = CurrentAliment.Carbs;
            originalMeal.Fats = CurrentAliment.Fats;
            originalMeal.Calories = CurrentAliment.Calories;
            originalMeal.OriginalServingSize = CurrentAliment.ServingSize;
            originalMeal.ServingSize = CurrentAliment.ServingSize;
            originalMeal.Unit = CurrentAliment.Unit;
            originalMeal.Foods = (CurrentAliment as Meal).Foods;

            // Remove deletted MealFoods
            foreach (var mealFood in DelettedMealFoods)
            {
                if (mealFood != null)
                {
                    RefData.MealFoods.Remove(mealFood);
                    await App.DataBaseRepo.DeleteMealFoodAsync(mealFood);
                }
            }
            DelettedMealFoods.Clear();


            // Update meal values
            RefData.UpdateMealValues(originalMeal);

            // Update meal to db
            await App.DataBaseRepo.UpdateMealAsync(originalMeal);

            // Add food to db if any new
            var newFoods = originalMeal.Foods.Where(x=> x.MealFoodId == 0);
            foreach(var food in newFoods)
            {
                MealFood mealFood = new MealFood();
                mealFood.MealId = originalMeal.Id;
                mealFood.FoodId = food.Id;
                mealFood.ServingSize = food.ServingSize;

                await App.DataBaseRepo.AddMealFoodAsync(mealFood);
                food.MealFoodId = mealFood.Id;
                RefData.MealFoods.Add(mealFood);
            }


            // TODO Refresh meal in DayMeals
            foreach (DayMeal dayMeal in RefData.DayMeals)
            {
                double ratio = 1;

                foreach (Aliment meal in dayMeal.Aliments)
                {
                    if (meal.AlimentType == AlimentTypeEnum.Meal && meal.Id == originalMeal.Id)
                    {
                        ratio = meal.ServingSize / originalMeal.OriginalServingSize;

                        meal.Name = originalMeal.Name;
                        meal.OriginalServingSize = originalMeal.OriginalServingSize;
                        meal.Unit = originalMeal.Unit;
                        meal.ImageSourcePath = originalMeal.ImageSourcePath;
                        meal.ImageBlob = originalMeal.ImageBlob;
                        meal.Calories = originalMeal.Calories * ratio;
                        meal.Proteins = originalMeal.Proteins * ratio; 
                        meal.Carbs = originalMeal.Carbs * ratio;
                        meal.Fats = originalMeal.Fats * ratio;
                    }
                }

                RefData.UpdateDayMealValues(dayMeal);   
            }

            // Update daily values
            RefData.UpdateDailyValues();
            //await Shell.Current.GoToAsync("..");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Add Food
        /// </summary>
        public ICommand AddFoodCommand { get; set; }
        public void AddFood()
        {
            AddAlimentPage addAlimentPage = new AddAlimentPage();
            (addAlimentPage.BindingContext as AddAlimentViewModel).CurrentMeal = this.CurrentAliment as Meal;
            (addAlimentPage.BindingContext as AddAlimentViewModel).MealSwitchVisibility = false;
            App.Current.MainPage.Navigation.PushAsync(addAlimentPage);
        }

        /// <summary>
        /// Deletee Food
        /// </summary>
        public ICommand DeletteAlimentCommand { get; set; }
        private void DeletteAliment(object[] objects)
        {
            Meal meal = objects[0] as Meal;
            Food food = objects[1] as Food;

            (CurrentAliment as Meal).Foods.Remove(food);
            var mealFood = RefData.MealFoods.Where(x => x.Id == food.MealFoodId).FirstOrDefault();
            DelettedMealFoods.Add(mealFood);
        }
        private List<MealFood> DelettedMealFoods { get; set; }
    }
}
