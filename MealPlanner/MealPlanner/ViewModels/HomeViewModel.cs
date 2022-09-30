using MealPlanner.Models;
using MealPlanner.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.RSControls.Controls;

namespace MealPlanner.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            Title = "Home";
            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
            UpdateAlimentCommand = new Command<object[]>(UpdateAliment);
        }

        public ICommand DeletteAlimentCommand { get; set; } 
        private void DeletteAliment(object[] objects)
        {
            if (objects == null)
                return;

            if (objects.Count() < 2)
                return;

            if (!(objects[0] is DayMeal) || !(objects[1] is IAliment))
                return;


            DayMeal dayMeal = objects[0] as DayMeal;
            IAliment aliment = objects[1] as IAliment;

            DayMealAliment dayMealAliment = App.DataBaseRepo.GetDayMealAlimentsAsync(aliment.AlimentType, dayMeal.Id, aliment.Id).Result.FirstOrDefault();

            if(dayMealAliment != null)
                App.DataBaseRepo.DeleteDayMealAlimentAsync(dayMealAliment);

            dayMeal.Aliments.Remove(aliment);

            RefData.DaylyProteins -= aliment.Proteins;
            RefData.DaylyCarbs -= aliment.Carbs;
            RefData.DaylyFats -= aliment.Fats;
            RefData.DaylyCalories -= aliment.Calories;
        }

        public ICommand UpdateAlimentCommand { get; set; }
        private void UpdateAliment(object[] objects)
        {
            var dayMeal = objects[0] as DayMeal;
            var aliment = objects[1] as IAliment;

            if (aliment is IAliment)
            {
                RSPopup rSPopup = new RSPopup();
                rSPopup.SetTitle(aliment.Name);


                RSPopupAlimentDetailPage rSPopupAlimentDetailPage = new RSPopupAlimentDetailPage();
                rSPopupAlimentDetailPage.BindingContext = new AlimentPopUpViewModel(aliment);
                var rSPopupAlimentDetailPageBindingContext = rSPopupAlimentDetailPage.BindingContext as AlimentPopUpViewModel;
                rSPopup.SetCustomView(rSPopupAlimentDetailPage);


                // Update
                rSPopup.AddAction("Update", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Neutral, new Command(async () =>
                {
                    // Update daylyProgress
                    RefData.DaylyProteins -= aliment.Proteins;
                    RefData.DaylyCarbs -= aliment.Carbs;
                    RefData.DaylyFats -= aliment.Fats;
                    RefData.DaylyCalories -= aliment.ServingSize;

                    // Update dayMeal
                    dayMeal.Proteins -= aliment.Proteins;
                    dayMeal.Carbs -= aliment.Carbs;
                    dayMeal.Fats -= aliment.Fats;
                    dayMeal.Calories -= aliment.Calories;


                    aliment.Proteins = rSPopupAlimentDetailPageBindingContext.AlimentProteins;
                    aliment.Carbs = rSPopupAlimentDetailPageBindingContext.AlimentCarbs;
                    aliment.Fats = rSPopupAlimentDetailPageBindingContext.AlimentFats;
                    aliment.Calories = rSPopupAlimentDetailPageBindingContext.AlimentCalories;
                    aliment.ServingSize = rSPopupAlimentDetailPageBindingContext.AlimentServingSize;

                    // Update daylyProgress
                    RefData.DaylyProteins += aliment.Proteins;
                    RefData.DaylyCarbs += aliment.Carbs;
                    RefData.DaylyFats += aliment.Fats;
                    RefData.DaylyCalories += aliment.ServingSize;

                    // Update dayMeal
                    dayMeal.Proteins += aliment.Proteins;
                    dayMeal.Carbs += aliment.Carbs;
                    dayMeal.Fats += aliment.Fats;
                    dayMeal.Calories += aliment.Calories;


                    DayMealAliment dayMealAliment = await App.DataBaseRepo.GetDayMealAlimentAsync(aliment.DayMealAlimentID);
                    dayMealAliment.ServingSize = rSPopupAlimentDetailPageBindingContext.AlimentServingSize;
                    await App.DataBaseRepo.UpdateDayMealAliment(dayMealAliment);

                    rSPopup.Close();
                }));


                // Edit
                rSPopup.AddAction("Edit", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Positive, new Command(() => 
                {
                    if(aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                    {
                        FoodPage foodPage = new FoodPage();
                        var foodPageBindingContext = foodPage.BindingContext as FoodViewModel;

                        // Fill informations
                        foodPageBindingContext.IsNew = false;
                        foodPageBindingContext.Name = aliment.Name;
                        foodPageBindingContext.ServingSize = aliment.ServingSize;
                        foodPageBindingContext.Unit = aliment.Unit;
                        foodPageBindingContext.Proteins = aliment.Proteins;
                        foodPageBindingContext.Carbs = aliment.Carbs;
                        foodPageBindingContext.Fats = aliment.Fats;
                        foodPageBindingContext.Calories = aliment.Calories;


                        App.Current.MainPage.Navigation.PushAsync(foodPage);
                    }
                    else
                        App.Current.MainPage.Navigation.PushAsync(new MealPage());
                }));

                // Close
                rSPopup.Show();
            }
            else if (aliment is Food)
                App.Current.MainPage.Navigation.PushAsync(new FoodPage());
        }
    }
}