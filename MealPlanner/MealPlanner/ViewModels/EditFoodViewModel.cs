using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class EditFoodViewModel : BaseViewModel
    {
        public EditFoodViewModel()
        {
            Title = "Food";
            SaveCommand = new Command<EditFoodPage>(SaveFood);
            UpdateCommand = new Command<EditFoodPage>(UpdateFood);
        }

        public ObservableCollection<Aliment> CopyOfFilteredAliments { get; set; }

        /// <summary>
        /// Save Food
        /// </summary>
        public ICommand SaveCommand { get; set; }
        private async void SaveFood(EditFoodPage foodPage)
        {
            if (!foodPage.CheckFields())
                return;

            CurrentAliment.OriginalServingSize = CurrentAliment.ServingSize;
            await App.DataBaseRepo.AddFoodAsync(CurrentAliment as Food);
            RefData.Foods.Add(CurrentAliment as Food);
            RefData.Aliments.Add(CurrentAliment as Food);
            CopyOfFilteredAliments.Add(CurrentAliment as Food);
            //await Shell.Current.GoToAsync("..");
            await Shell.Current.Navigation.PopAsync();
            //await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Update Food
        /// </summary>
        public ICommand UpdateCommand { get; set; }
        private async void UpdateFood(EditFoodPage foodPage)
        {
            if (!foodPage.CheckFields())
                return;
            
            
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
            await Shell.Current.Navigation.PopAsync();
            //await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
