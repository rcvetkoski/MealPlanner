using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.RSControls.Controls;

namespace MealPlanner.ViewModels
{
    public class FoodViewModel : BaseViewModel
    {
        public FoodViewModel()
        {
            Title = "Food";
            SaveCommand = new Command(SaveFood);
            UpdateCommand = new Command(UpdateFood);
        }

        private void CalculateCalories()
        {
            //Calories = Proteins * 4 + Carbs * 4 + Fats * 9;
        }

        /// <summary>
        /// Save Food
        /// </summary>
        public ICommand SaveCommand { get; set; }
        private async void SaveFood()
        {
            CurrentAliment.OriginalServingSize = CurrentAliment.ServingSize;
            await App.DataBaseRepo.AddFoodAsync(CurrentAliment as Food);
            RefData.Foods.Add(CurrentAliment as Food);
            RefData.Aliments.Add(CurrentAliment as Food);
            RefData.FilteredAliments.Add(CurrentAliment as Food);
            //await Shell.Current.GoToAsync("..");
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Update Food
        /// </summary>
        public ICommand UpdateCommand { get; set; }
        private async void UpdateFood()
        {
            // Get real food to update
            Food originalFood = RefData.Foods.Where(x => x.Id == CurrentAliment.Id).FirstOrDefault();

            if (originalFood == null)
                return;

            originalFood.Name = CurrentAliment.Name;
            originalFood.Proteins = CurrentAliment.Proteins;
            originalFood.ImageSourcePath = CurrentAliment.ImageSourcePath;
            originalFood.Carbs = CurrentAliment.Carbs;
            originalFood.Fats = CurrentAliment.Fats;
            originalFood.Calories = CurrentAliment.Calories;
            originalFood.OriginalServingSize = CurrentAliment.ServingSize;
            originalFood.ServingSize = CurrentAliment.ServingSize;
            originalFood.Unit = CurrentAliment.Unit;
            originalFood.ImageBlob = CurrentAliment.ImageBlob;
            //food.ImageBlob = await Helpers.HttpClientHelper.Client.GetByteArrayAsync(ImageSourcePath);


            await App.DataBaseRepo.UpdateFoodAsync(originalFood);

            // Refresh food if used
            foreach(Meal meal in RefData.Meals)
            {
                foreach (Aliment aliment in meal.Aliments)
                {
                    if (aliment.AlimentType == AlimentTypeEnum.Food && aliment.Id == CurrentAliment.Id)
                    {
                        double ratio = aliment.ServingSize / originalFood.ServingSize;

                        aliment.Name = originalFood.Name;
                        aliment.Proteins = originalFood.Proteins * ratio;
                        aliment.Carbs = originalFood.Carbs * ratio;
                        aliment.Fats = originalFood.Fats * ratio;
                        aliment.Calories = originalFood.Calories * ratio;
                        aliment.OriginalServingSize = originalFood.ServingSize;
                        aliment.Unit = originalFood.Unit;
                    }
                }

                // Update meal values
                RefData.UpdateMealValues(meal);
            }

            // Update daily values
            RefData.UpdateDailyValues();

            //await Shell.Current.GoToAsync("..");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
