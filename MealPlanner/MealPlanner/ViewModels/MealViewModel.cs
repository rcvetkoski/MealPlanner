using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            AddFoodCommand = new Command(AddFood);
            SaveCommand = new Command(SaveFood);
            UpdateCommand = new Command(UpdateMeal);
            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
            DelettedMealFoods = new List<MealFood>();
        }

        public ICommand SaveCommand { get; set; }

        private async void SaveFood()
        {
            CurrentMeal.OriginalServingSize = CurrentMeal.ServingSize;
            App.RefData.Meals.Add(CurrentMeal);
            App.RefData.Aliments.Add(CurrentMeal);
            await App.DataBaseRepo.AddMealAsync(CurrentMeal);

            //Save foods in db
            foreach (Food food in CurrentMeal.Foods)
            {
                MealFood mealFood = new MealFood();
                mealFood.MealId = CurrentMeal.Id;
                mealFood.FoodId = food.Id;
                mealFood.ServingSize = food.ServingSize;
                await App.DataBaseRepo.AddMealFoodAsync(mealFood);
                food.MealFoodId = mealFood.Id;
                RefData.MealFoods.Add(mealFood);
            }

            await Application.Current.MainPage.Navigation.PopAsync();
        }


        public ICommand UpdateCommand { get; set; }

        private async void UpdateMeal()
        {
            if (CurrentMeal == null)
                return;

            CurrentMeal.OriginalServingSize = CurrentMeal.ServingSize;

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
            RefData.UpdateMealValues(CurrentMeal);

            // Update meal to db
            await App.DataBaseRepo.UpdateMealAsync(CurrentMeal);

            // Add food to db if any new
            var newFoods = CurrentMeal.Foods.Where(x=> x.MealFoodId == 0);
            foreach(var food in newFoods)
            {
                MealFood mealFood = new MealFood();
                mealFood.MealId = CurrentMeal.Id;
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
                    if (meal.AlimentType == AlimentTypeEnum.Meal && meal.Id == CurrentMeal.Id)
                    {
                        ratio = meal.ServingSize / CurrentMeal.OriginalServingSize;

                        meal.Name = CurrentMeal.Name;
                        meal.OriginalServingSize = CurrentMeal.OriginalServingSize;
                        meal.Unit = CurrentMeal.Unit;

                        meal.Calories = CurrentMeal.Calories * ratio;
                        meal.Proteins = CurrentMeal.Proteins * ratio; 
                        meal.Carbs = CurrentMeal.Carbs * ratio;
                        meal.Fats = CurrentMeal.Fats * ratio;
                    }
                }


                RefData.UpdateDayMealValues(dayMeal);   
            }

            // Update daily values
            RefData.UpdateDailyValues();
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


        public ICommand DeletteAlimentCommand { get; set; }
        private async void DeletteAliment(object[] objects)
        {
            Meal meal = objects[0] as Meal;
            Food food = objects[1] as Food;

            meal.Foods.Remove(food);

            var mealFood = RefData.MealFoods.Where(x => x.Id == food.MealFoodId).FirstOrDefault();

            DelettedMealFoods.Add(mealFood);
        }

        private List<MealFood> DelettedMealFoods { get; set; }
    }
}
