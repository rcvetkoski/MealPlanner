using MealPlanner.Models;
using MealPlanner.Views;
using MealPlanner.Views.Popups;
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
            Title = "Home";
            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
            UpdateAlimentCommand = new Command<object[]>(UpdateAliment);
            AddAlimentCommand = new Command<DayMeal>(AddAliment);
            OpenUserPageCommand = new Command(OpenUserPage);    
        }

        public ICommand DeletteAlimentCommand { get; set; } 
        private async void DeletteAliment(object[] objects)
        {
            if (objects == null)
                return;

            if (objects.Count() < 2)
                return;

            if (!(objects[0] is DayMeal) || !(objects[1] is Aliment))
                return;


            DayMeal dayMeal = objects[0] as DayMeal;
            Aliment aliment = objects[1] as Aliment;

            DayMealAliment dayMealAliment = await App.DataBaseRepo.GetDayMealAlimentAsync(aliment.DayMealAlimentId);

            if(dayMealAliment != null)
            {
                await App.DataBaseRepo.DeleteDayMealAlimentAsync(dayMealAliment);
                var realDayMealAliment = RefData.DayMealAliments.Where(x => x.Id == dayMealAliment.Id).FirstOrDefault();
                if(realDayMealAliment != null)
                    RefData.DayMealAliments.Remove(realDayMealAliment);
            }

            dayMeal.Aliments.Remove(aliment);

            // Update dayMeal values
            RefData.UpdateDayMealValues(dayMeal);

            // Update daily values
            RefData.UpdateDailyValues();
        }


        public ICommand UpdateAlimentCommand { get; set; }
        private void UpdateAliment(object[] objects)
        {
            var dayMeal = objects[0] as DayMeal;
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

                    // Update dayMeal values
                    RefData.UpdateDayMealValues(dayMeal);

                    // Update daily values
                    RefData.UpdateDailyValues();

                    var al = aliment;
                    DayMealAliment dayMealAliment = await App.DataBaseRepo.GetDayMealAlimentAsync(aliment.DayMealAlimentId);
                    dayMealAliment.ServingSize = rSPopupAlimentDetailPageBindingContext.AlimentServingSize;
                    await App.DataBaseRepo.UpdateDayMealAliment(dayMealAliment);

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
                        App.Current.MainPage.Navigation.PushAsync(new MealPage());
                }));


                // Close
                rSPopup.Show();
            }
            else if (aliment is Food)
                App.Current.MainPage.Navigation.PushAsync(new FoodPage());
        }


        public ICommand AddAlimentCommand { get; set; }
        private async void AddAliment(DayMeal dayMeal)
        {
            AddAlimentPage addAlimentPage = new AddAlimentPage();
            (addAlimentPage.BindingContext as AddAlimentViewModel).SelectedDayMeal = dayMeal;
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