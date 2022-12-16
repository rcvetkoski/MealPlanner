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
    public class AddAlimentViewModel : BaseViewModel
    {
        public AddAlimentViewModel()
        {
            SetTitle();
            FilteredAliments = new ObservableCollection<Aliment>();
            Query = string.Empty;
            RecipeSwitchVisibility = true;
            IsFoodChecked = true;
            searchResults = new List<Food>();
            foreach (var item in RefData.Foods)
                searchResults.Add(item);

            CreateNewAlimentCommand = new Command(CreateNewAliment);
            CreateFoodCommand = new Command(CreateFood);
            SelectAlimentCommand = new Command<Aliment>(SelectAliment);
            CreateRecipeCommand = new Command(CreateRecipe);
            ScanBarCodeCommand = new Command(ScanBarCode);
            SearchAlimentsCommand = new Command<string>(SearchAliments);
            OpenFiltersCommand = new Command(openFIlters);
            ClearSearchCommand = new Command(ClearSearch);

            //FilteredAlimentsRefresh();
        }


        public ObservableCollection<Aliment> FilteredAliments { get; set; }

        private RSPopup rSPopupFilter;

        private string query;
        public string Query
        {
            get
            {
                return query;
            }
            set
            {
                if(value != query)
                {
                    query = value;
                    OnPropertyChanged(nameof(Query));
                }
            }
        }

        private bool recipeSwitchVisibility;
        public bool RecipeSwitchVisibility
        {
            get
            {
                return recipeSwitchVisibility;
            }
            set
            {
                if (recipeSwitchVisibility != value)
                {
                    recipeSwitchVisibility = value;
                    OnPropertyChanged("RecipeSwitchVisibility");
                }
            }
        }

        public Recipe CurrentRecipe { get; set; }

        private bool isFoodChecked;
        public bool IsFoodChecked
        {
            get
            {
                return isFoodChecked;
            }
            set
            {
                if (isFoodChecked != value)
                {
                    isFoodChecked = value;
                    OnPropertyChanged("IsFoodChecked");

                    if(isFoodChecked)
                        FilteredAlimentsRefresh();
                }
            }
        }

        private bool isRecipeChecked;
        public bool IsRecipeChecked
        {
            get
            {
                return isRecipeChecked;
            }
            set
            {
                if (isRecipeChecked != value)
                {
                    isRecipeChecked = value;
                    OnPropertyChanged("IsRecipeChecked");

                    if(IsRecipeChecked) 
                        FilteredAlimentsRefresh();
                }
            }
        }

        private List<Food> searchResults;
        public List<Food> SearchResults
        {
            get
            {
                return searchResults;
            }
            set
            {
                if(searchResults != value)
                {
                    searchResults = value;
                    OnPropertyChanged("SearchResults");
                }
            }
        }

        public void SetTitle()
        {
            if (IsFoodChecked)
                Title = $"Foods";
            else
                Title = $"Recipes";
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
                foodPageVm.CurrentAliment = existingAliment;
                foodPageVm.InitProperties();
                foodPageVm.Title = $"{existingAliment.Name}";
                foodPageVm.SelectedMeal = SelectedMeal; foodPageVm.CurrentAliment.ServingSize = 100;
                foodPageVm.CopyOfFilteredAliments = FilteredAliments;

                //await Shell.Current.GoToAsync($"{nameof(EditFoodPage)}");
                await Application.Current.MainPage.Navigation.PushAsync(foodPage);
                return;
            }
            else
            {
                FoodPage foodPage = new FoodPage();
                FoodViewModel foodPageVm = foodPage.BindingContext as FoodViewModel;
                foodPageVm.CurrentAliment = existingAliment;
                foodPageVm.InitProperties();
                foodPageVm.Title = $"{existingAliment.Name}";
                foodPageVm.SelectedMeal = SelectedMeal;
                foodPageVm.CopyOfFilteredAliments = FilteredAliments;
                await Application.Current.MainPage.Navigation.PushAsync(foodPage);
                return;
            }





            RSPopup rSPopup = new RSPopup("", "", Xamarin.RSControls.Enums.RSPopupPositionEnum.Bottom);
            rSPopup.SetPopupAnimation(Xamarin.RSControls.Enums.RSPopupAnimationEnum.BottomToTop);
            rSPopup.Title = existingAliment.Name;
            rSPopup.Style = Application.Current.Resources["RSPopup"] as Style;
            AlimentPopUpViewModel rsPopupBindingContext;
            RSPopupAlimentDetailPage rSPopupAlimentDetailPage = new RSPopupAlimentDetailPage();
            rSPopupAlimentDetailPage.BindingContext = new AlimentPopUpViewModel(existingAliment);
            rsPopupBindingContext = rSPopupAlimentDetailPage.BindingContext as AlimentPopUpViewModel;
            rSPopup.SetCustomView(rSPopupAlimentDetailPage);
            rSPopup.SetMargin(20, 20, 20, 20);

            // Add
            if (SelectedMeal != null)
            {
                rSPopup.AddAction("Add", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Positive, new Command(async () =>
                {
                    if (RecipeSwitchVisibility)
                    {
                        var ratio = rsPopupBindingContext.AlimentServingSize / existingAliment.OriginalServingSize;
                        Aliment aliment = RefData.CreateAndCopyAlimentProperties(existingAliment, ratio);
                        aliment.ServingSize = rsPopupBindingContext.AlimentServingSize;

                        // Add aliment
                        RefData.AddAliment(aliment, SelectedMeal);
                    }
                    else // When adding food to recipe
                    {
                        var ratio = rsPopupBindingContext.AlimentServingSize / existingAliment.OriginalServingSize;
                        Food food = RefData.CreateAndCopyAlimentProperties(existingAliment, ratio) as Food;
                        food.ServingSize = rsPopupBindingContext.AlimentServingSize;

                        // Set RecipeFoodId to 0
                        food.RecipeFoodId = 0;

                        CurrentRecipe.Foods.Add(food);
                    }

                    //rSPopup.Close();
                    //await Shell.Current.GoToAsync("..");
                    await Shell.Current.Navigation.PopAsync();
                    //await Application.Current.MainPage.Navigation.PopAsync();
                }));
            }
            else if(CurrentRecipe != null)
            {
                rSPopup.AddAction("Add", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Positive, new Command(async () =>
                {
                    var ratio = rsPopupBindingContext.AlimentServingSize / existingAliment.OriginalServingSize;
                    Food food = RefData.CreateAndCopyAlimentProperties(existingAliment, ratio) as Food;
                    food.ServingSize = rsPopupBindingContext.AlimentServingSize;

                    // Set RecipeFoodId to 0
                    food.RecipeFoodId = 0;

                    
                    CurrentRecipe.Foods.Add(food);

                    //rSPopup.Close();
                    //await Shell.Current.GoToAsync("..");
                    await Shell.Current.Navigation.PopAsync();
                    //await Application.Current.MainPage.Navigation.PopAsync();
                }));
            }

            // Edit
            rSPopup.AddAction("Edit", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Neutral, new Command(async () =>
            {
                if (existingAliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                {
                    EditFoodPage foodPage = new EditFoodPage();
                    (foodPage.BindingContext as EditFoodViewModel).CurrentAliment = RefData.CreateAndCopyAlimentProperties(existingAliment);
                    (foodPage.BindingContext as EditFoodViewModel).IsNew = false;
                    (foodPage.BindingContext as EditFoodViewModel).CopyOfFilteredAliments = FilteredAliments;

                    await Shell.Current.Navigation.PushAsync(foodPage);
                    //await App.Current.MainPage.Navigation.PushAsync(foodPage);
                }
                else
                {
                    RecipePage recipePage = new RecipePage();
                    var recipePageBindingContext = recipePage.BindingContext as RecipeViewModel;
                    recipePageBindingContext.SelectedMeal = SelectedMeal;
                    recipePageBindingContext.CurrentAliment = RefData.CreateAndCopyAlimentProperties(existingAliment);
                    recipePageBindingContext.IsNew = false;
                    recipePageBindingContext.CopyOfFilteredAliments = FilteredAliments;

                    await Shell.Current.Navigation.PushAsync(recipePage);
                    //await App.Current.MainPage.Navigation.PushAsync(recipePage);
                }
                rSPopup.Close();
            }));
            rSPopup.Show();
        }

        public ICommand CreateNewAlimentCommand { get; set; }
        private void CreateNewAliment()
        {
            if (IsFoodChecked)
                CreateFoodCommand.Execute(null);
            else
                CreateRecipeCommand.Execute(null);
        }

        public ICommand CreateFoodCommand { get; set; }
        private async void CreateFood()
        {
            rSPopupFilter?.Close();
            EditFoodPage foodPage = new EditFoodPage();
            (foodPage.BindingContext as EditFoodViewModel).CurrentAliment = new Food();
            (foodPage.BindingContext as EditFoodViewModel).IsNew = true;
            (foodPage.BindingContext as EditFoodViewModel).CopyOfFilteredAliments = FilteredAliments;
            await Shell.Current.Navigation.PushAsync(foodPage);
            //await App.Current.MainPage.Navigation.PushAsync(foodPage);
            //await Shell.Current.GoToAsync($"{nameof(EditFoodPage)}");
        }

        public ICommand CreateRecipeCommand { get; set; }
        private async void CreateRecipe()
        {
            rSPopupFilter?.Close();
            RecipePage recipePage = new RecipePage();
            (recipePage.BindingContext as RecipeViewModel).CurrentAliment = new Recipe();
            (recipePage.BindingContext as RecipeViewModel).CopyOfFilteredAliments = FilteredAliments;
            await Shell.Current.Navigation.PushAsync(recipePage);
            //await Shell.Current.GoToAsync($"{nameof(RecipePage)}");
            //await App.Current.MainPage.Navigation.PushAsync(recipePage);
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

                EditFoodPage foodPage = new EditFoodPage();
                EditFoodViewModel foodPageVm = foodPage.BindingContext as EditFoodViewModel;
                foodPageVm.CurrentAliment = RefData.CreateAndCopyAlimentProperties(aliment);
                foodPageVm.CurrentAliment.ServingSize = 100;
                foodPageVm.IsNew = true;
                foodPageVm.CopyOfFilteredAliments = FilteredAliments;

                //await Shell.Current.GoToAsync($"{nameof(EditFoodPage)}");
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

                FilteredAliments.Clear();

                foreach (Aliment aliment in aliments)
                {
                    FilteredAliments.Add(aliment);
                }
            }
            catch (Exception ex)
            {
                RSPopup rSPopup = new RSPopup("", ex.Message);
                rSPopup.Show();
            }
        }

        public void FilteredAlimentsRefresh()
        {
            Helpers.Enums.AlimentTypeEnum type = IsFoodChecked ? Helpers.Enums.AlimentTypeEnum.Food : Helpers.Enums.AlimentTypeEnum.Recipe;

            var searchedList = RefData.Aliments.Where(x => x.AlimentType == type && x.Name.ToLower().Contains(Query.ToLower())).ToList();

            FilteredAliments.Clear();

            foreach (Aliment aliment in searchedList)
            {
                if (IsRecipeChecked && aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Recipe)
                    FilteredAliments.Add(aliment);
                else if (!IsRecipeChecked && aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                    FilteredAliments.Add(aliment);
            }
        }

        public void Search()
        {
            Helpers.Enums.AlimentTypeEnum type = IsFoodChecked ? Helpers.Enums.AlimentTypeEnum.Food : Helpers.Enums.AlimentTypeEnum.Recipe;

            var searchedList = RefData.Aliments.Where(x => x.AlimentType == type && x.Name.ToLower().Contains(Query.ToLower())).ToList();

            FilteredAliments.Clear();

            foreach (var item in searchedList)
                FilteredAliments.Add(item);
        }

        public ICommand ClearSearchCommand { get; set; }
        private void ClearSearch()
        {
            if (string.IsNullOrEmpty(Query))
                return;

            Query = string.Empty;
            FilteredAlimentsRefresh();
        }

        ~AddAlimentViewModel()
        {

        }
    }
}
