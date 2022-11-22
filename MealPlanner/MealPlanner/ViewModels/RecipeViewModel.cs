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
    public class RecipeViewModel : BaseViewModel
    {
        public RecipeViewModel()
        {
            Title = "Recipe";
            IsNew = true;
            AddFoodCommand = new Command(AddFood);
            SaveCommand = new Command<RecipePage>(SaveRecipe);
            UpdateCommand = new Command<RecipePage>(UpdateRecipe);
            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
            DelettedRecipeFoods = new List<RecipeFood>();
        }

        public ObservableCollection<Aliment> CopyOfFilteredAliments { get; set; }


        /// <summary>
        /// Save Recipe
        /// </summary>
        public ICommand SaveCommand { get; set; }
        private async void SaveRecipe(RecipePage recipePage)
        {
            if (!recipePage.CheckFields())
                return;

            CurrentAliment.OriginalServingSize = CurrentAliment.ServingSize;
            RefData.Recipes.Add(CurrentAliment as Recipe);
            RefData.Aliments.Add(CurrentAliment as Recipe);
            CopyOfFilteredAliments?.Add(CurrentAliment as Recipe);
            await App.DataBaseRepo.AddRecipeAsync(CurrentAliment as Recipe);

            //Save foods in db
            foreach (Food food in (CurrentAliment as Recipe).Foods)
            {
                RecipeFood recipeFood = new RecipeFood();
                recipeFood.RecipeId = CurrentAliment.Id;
                recipeFood.FoodId = food.Id;
                recipeFood.ServingSize = food.ServingSize;
                await App.DataBaseRepo.AddRecipeFoodAsync(recipeFood);
                food.RecipeFoodId = recipeFood.Id;
                RefData.RecipeFoods.Add(recipeFood);
            }

            // Update recipe values
            RefData.UpdateRecipeValues(CurrentAliment as Recipe);

            // Update values
            await App.DataBaseRepo.UpdateRecipeAsync(CurrentAliment as Recipe);

            //await Shell.Current.GoToAsync("..");
            await Shell.Current.Navigation.PopAsync();
            //await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Update Recipe
        /// </summary>
        public ICommand UpdateCommand { get; set; }
        private async void UpdateRecipe(RecipePage recipePage)
        {
            if (!recipePage.CheckFields())
                return;


            // Get real recipe to update
            Recipe originalRecipe = RefData.Recipes.Where(x => x.Id == CurrentAliment.Id).FirstOrDefault();

            if (originalRecipe == null)
                return;

            originalRecipe.Name = CurrentAliment.Name;
            originalRecipe.Proteins = CurrentAliment.Proteins;
            originalRecipe.ImageSourcePath = CurrentAliment.ImageSourcePath;
            originalRecipe.ImageBlob = CurrentAliment.ImageBlob;
            originalRecipe.Carbs = CurrentAliment.Carbs;
            originalRecipe.Fats = CurrentAliment.Fats;
            originalRecipe.Calories = CurrentAliment.Calories;
            originalRecipe.OriginalServingSize = CurrentAliment.ServingSize;
            originalRecipe.ServingSize = CurrentAliment.ServingSize;
            originalRecipe.Unit = CurrentAliment.Unit;
            originalRecipe.Foods = (CurrentAliment as Recipe).Foods;

            // Remove deletted RecipeFoods
            foreach (var recipeFood in DelettedRecipeFoods)
            {
                if (recipeFood != null)
                {
                    RefData.RecipeFoods.Remove(recipeFood);
                    await App.DataBaseRepo.DeleteRecipeFoodAsync(recipeFood);
                }
            }
            DelettedRecipeFoods.Clear();


            // Update recipe values
            RefData.UpdateRecipeValues(originalRecipe);

            // Update recipe to db
            await App.DataBaseRepo.UpdateRecipeAsync(originalRecipe);

            // Add food to db if any new
            var newFoods = originalRecipe.Foods.Where(x=> x.RecipeFoodId == 0);
            foreach(var food in newFoods)
            {
                RecipeFood recipeFood = new RecipeFood();
                recipeFood.RecipeId = originalRecipe.Id;
                recipeFood.FoodId = food.Id;
                recipeFood.ServingSize = food.ServingSize;

                await App.DataBaseRepo.AddRecipeFoodAsync(recipeFood);
                food.RecipeFoodId = recipeFood.Id;
                RefData.RecipeFoods.Add(recipeFood);
            }


            // TODO Refresh recipe in Meals
            foreach (Meal meal in RefData.Meals)
            {
                double ratio = 1;

                foreach (Aliment recipe in meal.Aliments)
                {
                    if (recipe.AlimentType == AlimentTypeEnum.Recipe && recipe.Id == originalRecipe.Id)
                    {
                        ratio = recipe.ServingSize / originalRecipe.OriginalServingSize;

                        recipe.Name = originalRecipe.Name;
                        recipe.OriginalServingSize = originalRecipe.OriginalServingSize;
                        recipe.Unit = originalRecipe.Unit;
                        recipe.ImageSourcePath = originalRecipe.ImageSourcePath;
                        recipe.ImageBlob = originalRecipe.ImageBlob;
                        recipe.Calories = originalRecipe.Calories * ratio;
                        recipe.Proteins = originalRecipe.Proteins * ratio; 
                        recipe.Carbs = originalRecipe.Carbs * ratio;
                        recipe.Fats = originalRecipe.Fats * ratio;
                    }
                }

                RefData.UpdateMealValues(meal);   
            }

            // Update daily values
            RefData.UpdateDailyValues();
            //await Shell.Current.GoToAsync("..");
            await Shell.Current.Navigation.PopAsync();
            //await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Add Food
        /// </summary>
        public ICommand AddFoodCommand { get; set; }
        public void AddFood()
        {
            AddAlimentPage addAlimentPage = new AddAlimentPage();
            (addAlimentPage.BindingContext as AddAlimentViewModel).SelectedMeal = SelectedMeal;
            (addAlimentPage.BindingContext as AddAlimentViewModel).CurrentRecipe = this.CurrentAliment as Recipe;
            (addAlimentPage.BindingContext as AddAlimentViewModel).RecipeSwitchVisibility = false;
            App.Current.MainPage.Navigation.PushAsync(addAlimentPage);
        }

        /// <summary>
        /// Deletee Food
        /// </summary>
        public ICommand DeletteAlimentCommand { get; set; }
        private void DeletteAliment(object[] objects)
        {
            Recipe recipe = objects[0] as Recipe;
            Food food = objects[1] as Food;

            (CurrentAliment as Recipe).Foods.Remove(food);
            var recipeFood = RefData.RecipeFoods.Where(x => x.Id == food.RecipeFoodId).FirstOrDefault();
            DelettedRecipeFoods.Add(recipeFood);
        }
        private List<RecipeFood> DelettedRecipeFoods { get; set; }
    }
}
