using MealPlanner.Helpers;
using MealPlanner.Models;
using MealPlanner.Services;
using MealPlanner.Views;
using MealPlanner.Views.Popups;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.RSControls.Controls;
using ZXing.QrCode.Internal;
using static MealPlanner.Models.TestModel;
using static System.Net.WebRequestMethods;

namespace MealPlanner.ViewModels
{
    [QueryProperty(nameof(SelectedDayMeal), "daymeal")]

    public class AddAlimentViewModel : BaseViewModel
    {
        public AddAlimentViewModel()
        {
            Title = "AddFood";
            MealSwitchVisibility = true;
            IsFoodChecked = true;
            searchResults = new List<Food>();
            foreach (var item in RefData.Foods)
                searchResults.Add(item);

            CreateFoodCommand = new Command(CreateFood);
            SelectAlimentCommand = new Command<Aliment>(SelectAliment);
            CreateMealCommand = new Command(CreateMeal);
            ScanBarCodeCommand = new Command(ScanBarCode);
            SearchAlimentsCommand = new Command<string>(SearchAliments);
            OpenFiltersCommand = new Command(openFIlters);

            FilteredAlimentsRefresh();
        }

        private RSPopup rSPopupFilter;

        public void FilteredAlimentsRefresh()
        {
            RefData.FilteredAliments.Clear();

            foreach (Aliment aliment in RefData.Aliments)
            {
                if (IsMealChecked && aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Meal)
                    RefData.FilteredAliments.Add(aliment);
                else if (!IsMealChecked && aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                    RefData.FilteredAliments.Add(aliment);
            }
        }

        private bool mealSwitchVisibility;
        public bool MealSwitchVisibility { get { return mealSwitchVisibility; } set { mealSwitchVisibility = value; OnPropertyChanged("MealSwitchVisibility"); } }

        public DayMeal SelectedDayMeal { get; set; }
        public Meal CurrentMeal { get; set; }

        private bool isFoodChecked;
        public bool IsFoodChecked { get { return isFoodChecked; } set { isFoodChecked = value; OnPropertyChanged("IsFoodChecked"); } }

        private bool isMealChecked;
        public bool IsMealChecked { get { return isMealChecked; } set { isMealChecked = value; OnPropertyChanged("IsMealChecked"); FilteredAlimentsRefresh(); } }


        private List<Food> searchResults;
        public List<Food> SearchResults
        {
            get
            {
                return searchResults;
            }
            set
            {
                searchResults = value;
                OnPropertyChanged("SearchResults");
            }
        }


        public ICommand SelectAlimentCommand { get; set; }
        private async void SelectAliment(Aliment existingAliment)
        {
            var item = RefData.Aliments.Where(x => x.Id == existingAliment.Id && x.AlimentType == existingAliment.AlimentType).FirstOrDefault();

            if(item == null)
            {
                FoodPage foodPage = new FoodPage();
                FoodViewModel foodPageVm = foodPage.BindingContext as FoodViewModel;
                foodPageVm.IsNew = true;
                foodPageVm.CurrentAliment = RefData.CreateAndCopyAlimentProperties(existingAliment);
                foodPageVm.CurrentAliment.ServingSize = 100;


                //await Shell.Current.GoToAsync($"{nameof(FoodPage)}");
                await Application.Current.MainPage.Navigation.PushAsync(foodPage);
                return;
            }


            RSPopup rSPopup = new RSPopup();
            rSPopup.Title = existingAliment.Name;
            rSPopup.Style = Application.Current.Resources["RSPopup"] as Style;
            AlimentPopUpViewModel rsPopupBindingContext;
            RSPopupAlimentDetailPage rSPopupAlimentDetailPage = new RSPopupAlimentDetailPage();
            rSPopupAlimentDetailPage.BindingContext = new AlimentPopUpViewModel(existingAliment);
            rsPopupBindingContext = rSPopupAlimentDetailPage.BindingContext as AlimentPopUpViewModel;
            rSPopup.SetCustomView(rSPopupAlimentDetailPage);

            // Add
            rSPopup.AddAction("Add", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Positive, new Command(async () =>
            {
                if (MealSwitchVisibility)
                {
                    var ratio = rsPopupBindingContext.AlimentServingSize / existingAliment.OriginalServingSize;
                    Aliment aliment = RefData.CreateAndCopyAlimentProperties(existingAliment, ratio);
                    aliment.ServingSize = rsPopupBindingContext.AlimentServingSize;
                    SelectedDayMeal.Aliments.Add(aliment);

                    // Update dayMeal values
                    RefData.UpdateDayMealValues(SelectedDayMeal);

                    // Update daily values
                    RefData.UpdateDailyValues();

                    DayMealAliment dayMealAliment = new DayMealAliment();
                    dayMealAliment.DayMealId = SelectedDayMeal.Id;
                    dayMealAliment.AlimentId = aliment.Id;
                    dayMealAliment.ServingSize = rsPopupBindingContext.AlimentServingSize;
                    dayMealAliment.AlimentType = aliment.AlimentType;

                    // Save to db
                    await App.DataBaseRepo.AddDayMealAlimentAsync(dayMealAliment);

                    // Asign DayMealAlimentId to aliment and add it to DayMealAliments
                    var lastItem = App.DataBaseRepo.GetAllDayMealAlimentsAsync().Result.OrderByDescending(x => x.Id).FirstOrDefault();
                    aliment.DayMealAlimentId = lastItem.Id;
                    RefData.DayMealAliments.Add(dayMealAliment);
                }
                else // When adding food to meal
                {
                    var ratio = rsPopupBindingContext.AlimentServingSize / existingAliment.OriginalServingSize;
                    Food food = RefData.CreateAndCopyAlimentProperties(existingAliment, ratio) as Food;
                    food.ServingSize = rsPopupBindingContext.AlimentServingSize;

                    // Set MealFoodId to 0
                    food.MealFoodId = 0;


                    CurrentMeal.Foods.Add(food);
                    //CurrentMeal.Calories += food.Calories;
                    //CurrentMeal.Proteins += food.Proteins;
                    //CurrentMeal.Carbs += food.Carbs;
                    //CurrentMeal.Fats += food.Fats;
                }

                //rSPopup.Close();
                //await Shell.Current.GoToAsync("..");
                await Application.Current.MainPage.Navigation.PopAsync();
            }));

            // Edit
            rSPopup.AddAction("Edit", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Neutral, new Command(() =>
            {
                if (existingAliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                {
                    FoodPage foodPage = new FoodPage();
                    (foodPage.BindingContext as FoodViewModel).CurrentAliment = RefData.CreateAndCopyAlimentProperties(existingAliment);
                    (foodPage.BindingContext as FoodViewModel).IsNew = false;

                    App.Current.MainPage.Navigation.PushAsync(foodPage);
                }
                else
                {
                    MealPage mealPage = new MealPage();
                    var mealPageBindingContext = mealPage.BindingContext as MealViewModel;
                    mealPageBindingContext.CurrentAliment = RefData.CreateAndCopyAlimentProperties(existingAliment);
                    mealPageBindingContext.IsNew = false;

                    App.Current.MainPage.Navigation.PushAsync(mealPage);
                }
                rSPopup.Close();
            }));
            rSPopup.Show();
        }


        public ICommand CreateFoodCommand { get; set; }
        private async void CreateFood()
        {
            rSPopupFilter.Close();
            FoodPage foodPage = new FoodPage();
            (foodPage.BindingContext as FoodViewModel).CurrentAliment = new Food();
            (foodPage.BindingContext as FoodViewModel).IsNew = true;
            await App.Current.MainPage.Navigation.PushAsync(foodPage);
            //await Shell.Current.GoToAsync($"{nameof(FoodPage)}");
        }

        public ICommand CreateMealCommand { get; set; }
        private async void CreateMeal()
        {
            rSPopupFilter.Close();
            MealPage mealPage = new MealPage();
            (mealPage.BindingContext as MealViewModel).CurrentAliment = new Meal();
            //await Shell.Current.GoToAsync($"{nameof(MealPage)}");
            await App.Current.MainPage.Navigation.PushAsync(mealPage);
        }

        public ICommand OpenFiltersCommand { get; set; }
        private void openFIlters()
        {
            rSPopupFilter = new RSPopup();
            rSPopupFilter.SetMargin(0, 10, 0, 0);
            Views.Popups.FilterAddAlimentsPagePopUp filterAddAlimentsPopUp = new Views.Popups.FilterAddAlimentsPagePopUp() { BindingContext = this};
            //rSPopupFilter.SetAppThemeColor(RSPopup.BackgroundColorProperty, Color.FromHex("f2f2f7"), Color.FromHex("#1C1C1E"));
            rSPopupFilter.Style = Application.Current.Resources["RSPopup"] as Style;
            rSPopupFilter.SetCustomView(filterAddAlimentsPopUp); 
            //rSPopupFilter.SetPopupPositionRelativeTo(button, Xamarin.RSControls.Enums.RSPopupPositionSideEnum.Over);
            //rSPopupFilter.SetPopupAnimation(Xamarin.RSControls.Enums.RSPopupAnimationEnum.RightToLeft);
            rSPopupFilter.SetPopupSize(150, Xamarin.RSControls.Enums.RSPopupSizeEnum.WrapContent);
            rSPopupFilter.Show();
        }

        public ICommand ScanBarCodeCommand { get; set; }
        private async void ScanBarCode()
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();

            if (result == null)
                return;

            var code = result.Text;

            //var code = "04963406";

            try
            {
                var aliment = await App.RestService.ScanBarCodeAsync(code);

                FoodPage foodPage = new FoodPage();
                FoodViewModel foodPageVm = foodPage.BindingContext as FoodViewModel;
                foodPageVm.CurrentAliment = RefData.CreateAndCopyAlimentProperties(aliment);
                foodPageVm.CurrentAliment.ServingSize = 100;
                foodPageVm.IsNew = true;


                //await Shell.Current.GoToAsync($"{nameof(FoodPage)}");
                await Application.Current.MainPage.Navigation.PushAsync(foodPage);

            }
            catch (Exception ex)
            {
                RSPopup rSPopup = new RSPopup("", ex.Message);
                rSPopup.Show();
            }
        }


        public ICommand SearchAlimentsCommand { get; set; }
        private async void SearchAliments(string text)
        {
            try
            {
                RSPopup rSPopup = new RSPopup();
                ActivityIndicator activityIndicator = new ActivityIndicator() { IsRunning = true };
                rSPopup.SetCustomView(activityIndicator);
                rSPopup.Show();

                var aliments = await App.RestService.SearchAlimentAsync(text);

                rSPopup.Close();

                RefData.FilteredAliments.Clear();

                foreach (Aliment aliment in aliments)
                {
                    RefData.FilteredAliments.Add(aliment);
                }
            }
            catch (Exception ex)
            {
                RSPopup rSPopup = new RSPopup("", ex.Message);
                rSPopup.Show();
            }
        }
    }
}
