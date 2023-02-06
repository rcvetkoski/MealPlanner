using MealPlanner.Helpers;
using MealPlanner.Helpers.Extensions;
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
            SliderX = 0;
            FoodButtonAtributtes = FontAttributes.Bold;
            RecipeButtonAtributtes = FontAttributes.None;
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
            FilterAlimentsCommand = new Command(FilterAliments);

            //FilteredAlimentsRefresh();
        }

        private double sliderX;
        public double SliderX
        {
            get 
            {
                return sliderX;
            }
            set
            {
                if(sliderX != value)
                {
                    sliderX = value;
                    OnPropertyChanged(nameof(SliderX));
                }
            }
        }
        public FontAttributes FoodButtonAtributtes { get; set; }
        public FontAttributes RecipeButtonAtributtes { get; set; }


        private ObservableCollection<Aliment> filteredAliments;
        public ObservableCollection<Aliment> FilteredAliments
        { 
            get
            {
                return filteredAliments;
            }
            set
            {
                if(filteredAliments != value)
                {
                    filteredAliments = value;
                    OnPropertyChanged(nameof(FilteredAliments));
                }
            }
        }

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
            //var item = RefData.Aliments.Where(x => x.Id == existingAliment.Id && x.AlimentType == existingAliment.AlimentType).FirstOrDefault();
            var getitemByName = RefData.Aliments.FirstOrDefault(x => x.Name == existingAliment.Name && !x.Archived);
            bool exists = getitemByName != null ? RefData.IsAlimentEqual(getitemByName, existingAliment) : false;

            if (!exists)
            {
                FoodPage foodPage = new FoodPage();
                FoodViewModel foodPageVm = foodPage.BindingContext as FoodViewModel;
                foodPageVm.IsNew = true;
                foodPageVm.CurrentAliment = existingAliment;
                foodPageVm.SelectedRecipe = CurrentRecipe;
                foodPageVm.SelectedMeal = SelectedMeal;
                foodPageVm.CurrentAliment.ServingSize = existingAliment.ServingSize;
                foodPageVm.CopyOfFilteredAliments = FilteredAliments;
                foodPageVm.CanDeleteItem = false;
                foodPageVm.CanAddItem = !RecipeSwitchVisibility || SelectedMeal != null ? true : false;
                foodPageVm.CanSaveItem = !foodPageVm.CanAddItem && SelectedMeal == null ? true : false;

                //await Shell.Current.GoToAsync($"{nameof(EditFoodPage)}");
                await Application.Current.MainPage.Navigation.PushAsync(foodPage);
                return;
            }
            else
            {
                FoodPage foodPage = new FoodPage();
                FoodViewModel foodPageVm = foodPage.BindingContext as FoodViewModel;
                foodPageVm.CurrentAliment = existingAliment;
                foodPageVm.IsNew = false;
                foodPageVm.SelectedMeal = SelectedMeal;
                foodPageVm.CopyOfFilteredAliments = FilteredAliments;
                foodPageVm.SelectedRecipe = CurrentRecipe;
                foodPageVm.CanDeleteItem = true;
                foodPageVm.CanAddItem = !RecipeSwitchVisibility || SelectedMeal != null ? true : false; 
                await Application.Current.MainPage.Navigation.PushAsync(foodPage);
                return;
            }
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
                var getitemByName = RefData.Aliments.FirstOrDefault(x => x.Name == aliment.Name && !x.Archived);
                bool exists = getitemByName != null ? RefData.IsAlimentEqual(getitemByName, aliment) : false;

                if (!exists)
                {
                    FoodPage foodPage = new FoodPage();
                    FoodViewModel foodPageVm = foodPage.BindingContext as FoodViewModel;
                    foodPageVm.IsNew = true;
                    foodPageVm.CurrentAliment = aliment;
                    foodPageVm.SelectedRecipe = CurrentRecipe;
                    foodPageVm.SelectedMeal = SelectedMeal;
                    foodPageVm.CurrentAliment.ServingSize = aliment.ServingSize;
                    foodPageVm.CopyOfFilteredAliments = FilteredAliments;
                    foodPageVm.CanDeleteItem = false;
                    foodPageVm.CanAddItem = !RecipeSwitchVisibility || SelectedMeal != null ? true : false;
                    foodPageVm.CanSaveItem = !foodPageVm.CanAddItem && SelectedMeal == null ? true : false;

                    //await Shell.Current.GoToAsync($"{nameof(EditFoodPage)}");
                    await Application.Current.MainPage.Navigation.PushAsync(foodPage);
                    return;
                }
                else
                {
                    FoodPage foodPage = new FoodPage();
                    FoodViewModel foodPageVm = foodPage.BindingContext as FoodViewModel;
                    foodPageVm.CurrentAliment = aliment;
                    foodPageVm.IsNew = false;
                    foodPageVm.SelectedMeal = SelectedMeal;
                    foodPageVm.CopyOfFilteredAliments = FilteredAliments;
                    foodPageVm.SelectedRecipe = CurrentRecipe;
                    foodPageVm.CanDeleteItem = true;
                    foodPageVm.CanAddItem = !RecipeSwitchVisibility || SelectedMeal != null ? true : false;
                    await Application.Current.MainPage.Navigation.PushAsync(foodPage);
                    return;
                }

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

                FillFilteredAliments(aliments);

                //FilteredAliments.Clear();
                //foreach (Aliment aliment in aliments)
                //{
                //    FilteredAliments.Add(aliment);
                //}
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
            var searchedList = RefData.Aliments.Where(x => x.AlimentType == type && !x.Archived && x.Name.ToLower().Contains(Query.ToLower())).OrderBy(x=> x.Name).ToList();
            FillFilteredAliments(searchedList);
        }

        public void Search()
        {
            Helpers.Enums.AlimentTypeEnum type = IsFoodChecked ? Helpers.Enums.AlimentTypeEnum.Food : Helpers.Enums.AlimentTypeEnum.Recipe;
            var searchedList = RefData.Aliments.Where(x => x.AlimentType == type && !x.Archived && x.Name.ToLower().Contains(Query.ToLower())).ToList();
            FillFilteredAliments(searchedList);
        }

        public ICommand ClearSearchCommand { get; set; }
        private void ClearSearch()
        {
            if (string.IsNullOrEmpty(Query))
                return;

            Query = string.Empty;
            FilteredAlimentsRefresh();
        }

        public ICommand FilterAlimentsCommand { get; set; }
        private async void FilterAliments()
        {
            var actionSheet = await Shell.Current.CurrentPage.DisplayActionSheet(null, "Cancel", null, "Sort by Name", "Sort by Calories", "Sort by Proteins", "Sort by Carbs", "Sort by Fats");

            switch (actionSheet)
            {
                case "Sort by Name":
                    FillFilteredAliments(FilteredAliments.OrderBy(x => x.Name).ToList());
                    break;
                case "Sort by Calories":
                    FillFilteredAliments(FilteredAliments.OrderByDescending(x => x.Calories).ToList());
                    break;
                case "Sort by Proteins":
                    FillFilteredAliments(FilteredAliments.OrderByDescending(x => x.Proteins).ToList());
                    break;
                case "Sort by Carbs":
                    FillFilteredAliments(FilteredAliments.OrderByDescending(x => x.Carbs).ToList());
                    break;
                case "Sort by Fats":
                    FillFilteredAliments(FilteredAliments.OrderByDescending(x => x.Fats).ToList());
                    break;
            }
        }

        private void FillFilteredAliments(List<Aliment> sortedList)
        {
            //FilteredAliments.Clear();
            FilteredAliments = null;
            FilteredAliments = sortedList.ToObservableCollection();
            
            //foreach (Aliment aliment in sortedList)
            //    FilteredAliments.Add(aliment);
        }

        ~AddAlimentViewModel()
        {

        }
    }
}
