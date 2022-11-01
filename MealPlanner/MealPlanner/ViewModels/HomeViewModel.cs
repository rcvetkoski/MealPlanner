using MealPlanner.Models;
using MealPlanner.Views;
using MealPlanner.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
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
            Title = RefData.CurrentDay.Day == DateTime.Now.Day ? "Today" : RefData.CurrentDay.ToString(("d MMMM"));

            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
            UpdateAlimentCommand = new Command<object[]>(UpdateAliment);
            AddAlimentCommand = new Command<Meal>(AddAliment);
            OpenUserPageCommand = new Command(OpenUserPage);    
        }

        public ICommand DeletteAlimentCommand { get; set; } 
        private async void DeletteAliment(object[] objects)
        {
            if (objects == null)
                return;

            if (objects.Count() < 2)
                return;

            if (!(objects[0] is Meal) || !(objects[1] is Aliment))
                return;


            Meal meal = objects[0] as Meal;
            Aliment aliment = objects[1] as Aliment;

            MealAliment mealAliment = await App.DataBaseRepo.GetMealAlimentAsync(aliment.MealAlimentId);

            if(mealAliment != null)
            {
                await App.DataBaseRepo.DeleteMealAlimentAsync(mealAliment);
                var realMealAliment = RefData.MealAliments.Where(x => x.Id == mealAliment.Id).FirstOrDefault();
                if(realMealAliment != null)
                    RefData.MealAliments.Remove(realMealAliment);
            }

            meal.Aliments.Remove(aliment);

            // Update meal values
            RefData.UpdateMealValues(meal);

            // Update daily values
            RefData.UpdateDailyValues();
        }


        public ICommand UpdateAlimentCommand { get; set; }
        private void UpdateAliment(object[] objects)
        {
            var meal = objects[0] as Meal;
            var aliment = objects[1] as Aliment;

            if (aliment is Aliment)
            {
                RSPopup rSPopup = new RSPopup();
                rSPopup.Title = aliment.Name;
                rSPopup.Style = Application.Current.Resources["RSPopup"] as Style;

                RSPopupAlimentDetailPage rSPopupAlimentDetailPage = new RSPopupAlimentDetailPage();
                rSPopupAlimentDetailPage.BindingContext = new AlimentPopUpViewModel(aliment);
                var rSPopupAlimentDetailPageBindingContext = rSPopupAlimentDetailPage.BindingContext as AlimentPopUpViewModel;
                rSPopup.SetCustomView(rSPopupAlimentDetailPage);


                // Update
                rSPopup.AddAction("Update", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Neutral, new Command(async () =>
                {
                    aliment.Proteins = rSPopupAlimentDetailPageBindingContext.AlimentProteins;
                    aliment.Carbs = rSPopupAlimentDetailPageBindingContext.AlimentCarbs;
                    aliment.Fats = rSPopupAlimentDetailPageBindingContext.AlimentFats;
                    aliment.Calories = rSPopupAlimentDetailPageBindingContext.AlimentCalories;
                    aliment.Unit = rSPopupAlimentDetailPageBindingContext.AlimentUnit;
                    aliment.ServingSize = rSPopupAlimentDetailPageBindingContext.AlimentServingSize;

                    // Update meal values
                    RefData.UpdateMealValues(meal);

                    // Update daily values
                    RefData.UpdateDailyValues();

                    var al = aliment;
                    MealAliment mealAliment = await App.DataBaseRepo.GetMealAlimentAsync(aliment.MealAlimentId);
                    mealAliment.ServingSize = rSPopupAlimentDetailPageBindingContext.AlimentServingSize;
                    await App.DataBaseRepo.UpdateMealAliment(mealAliment);

                    rSPopup.Close();
                }));


                // Delette
                rSPopup.AddAction("Delette", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Destructive, new Command(() => 
                {
                    if(aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                    {
                        DeletteAliment(objects);
                        rSPopup.Close();
                    }
                    else
                        App.Current.MainPage.Navigation.PushAsync(new RecipePage());
                }));


                // Close
                rSPopup.Show();
            }
            else if (aliment is Food)
                App.Current.MainPage.Navigation.PushAsync(new FoodPage());
        }


        public ICommand AddAlimentCommand { get; set; }
        private async void AddAliment(Meal meal)
        {
            AddAlimentPage addAlimentPage = new AddAlimentPage();
            (addAlimentPage.BindingContext as AddAlimentViewModel).SelectedMeal = meal;
            await App.Current.MainPage.Navigation.PushAsync(addAlimentPage);        
            //await Shell.Current.GoToAsync($"AddAlimentPage");
        }


        public ICommand OpenUserPageCommand { get; set; }
        private async void OpenUserPage()
        {
            //await Shell.Current.GoToAsync($"{nameof(UserPage)}");
            await App.Current.MainPage.Navigation.PushAsync(new UserPage());        
        }
    }
}