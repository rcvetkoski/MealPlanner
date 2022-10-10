using MealPlanner.Models;
using MealPlanner.Views;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.RSControls.Controls;
using static MealPlanner.Models.TestModel;
using static System.Net.WebRequestMethods;

namespace MealPlanner.ViewModels
{
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

            FilteredAliments = new ObservableCollection<Aliment>();
            FilteredAlimentsRefresh();
        }


        public ObservableCollection<Aliment> FilteredAliments { get; set; } 
        public void FilteredAlimentsRefresh()
        {
            FilteredAliments.Clear();

            foreach (Aliment aliment in RefData.Aliments)
            {
                if(IsMealChecked && aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Meal)
                    FilteredAliments.Add(aliment);
                else if(!IsMealChecked && aliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                    FilteredAliments.Add(aliment);
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

        public ObservableCollection<Food> CurrentMealTempFoods { get; set; }



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


        public ICommand SelectAlimentCommand { get;set; }
        private void SelectAliment(Aliment existingAliment)
        {
            RSPopup rSPopup = new RSPopup();
            rSPopup.SetTitle(existingAliment.Name);


            AlimentPopUpViewModel rsPopupBindingContext;
            RSPopupAlimentDetailPage rSPopupAlimentDetailPage = new RSPopupAlimentDetailPage();
            rSPopupAlimentDetailPage.BindingContext = new AlimentPopUpViewModel(existingAliment);
            rsPopupBindingContext = rSPopupAlimentDetailPage.BindingContext as AlimentPopUpViewModel;

            rSPopup.SetCustomView(rSPopupAlimentDetailPage);

            // Add
            rSPopup.AddAction("Add", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Neutral, new Command(async () =>
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

                    CurrentMealTempFoods.Add(food);
                    //CurrentMeal.Calories += food.Calories;
                    //CurrentMeal.Proteins += food.Proteins;
                    //CurrentMeal.Carbs += food.Carbs;
                    //CurrentMeal.Fats += food.Fats;
                }

                rSPopup.Close();
                await Application.Current.MainPage.Navigation.PopAsync();
            }));

            // Edit
            rSPopup.AddAction("Edit", Xamarin.RSControls.Enums.RSPopupButtonTypeEnum.Positive, new Command(() =>
            {
                if (existingAliment.AlimentType == Helpers.Enums.AlimentTypeEnum.Food)
                {
                    FoodPage foodPage = new FoodPage();
                    var foodPageBindingContext = foodPage.BindingContext as FoodViewModel;

                    // Fill informations
                    foodPageBindingContext.IsNew = false;
                    foodPageBindingContext.Id = existingAliment.Id;
                    foodPageBindingContext.ImageSourcePath = existingAliment.ImageSourcePath;
                    foodPageBindingContext.Name = existingAliment.Name;
                    foodPageBindingContext.ServingSize = existingAliment.ServingSize;
                    foodPageBindingContext.Unit = existingAliment.Unit;
                    foodPageBindingContext.Proteins = existingAliment.Proteins;
                    foodPageBindingContext.Carbs = existingAliment.Carbs;
                    foodPageBindingContext.Fats = existingAliment.Fats;
                    foodPageBindingContext.Calories = existingAliment.Calories;


                    App.Current.MainPage.Navigation.PushAsync(foodPage);
                }
                else
                {
                    MealPage mealPage = new MealPage();
                    var mealPageBindingContext = mealPage.BindingContext as MealViewModel;

                    // Fill informations
                    mealPageBindingContext.IsNew = false;
                    mealPageBindingContext.CurrentMeal = existingAliment as Meal;
                    mealPageBindingContext.FillMealProperties(existingAliment as Meal);

                    App.Current.MainPage.Navigation.PushAsync(mealPage);
                }
            }));
            rSPopup.Show();
        }


        public ICommand CreateFoodCommand { get; set; }
        private void CreateFood()
        {
            App.Current.MainPage.Navigation.PushAsync(new FoodPage());
        }

        public ICommand CreateMealCommand { get; set; }
        private void CreateMeal()
        {
            App.Current.MainPage.Navigation.PushAsync(new MealPage());
        }


        public class Jsontest
        {
            public string code { get; set; }

            public Product product { get; set; }
            public string status { get; set; }

            public string status_erbose { get; set; }
        }



        private string result;
        public string Result { get { return result; } set { result = value; OnPropertyChanged("Result"); } }

        public ICommand ScanBarCodeCommand { get; set; }
        private async void ScanBarCode()
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();

            string uriPath = string.Empty;
            //uriPath = "https://world.openfoodfacts.org/api/v2/product/=4006040004974000&fields=product_name,image_front_url,proteins_100g,proteins_unit,proteins_value,carbohydrates_100g,energy-kcal_100g,carbohydrates_100g,fat_100g,fiber_100g";

            if (result != null)
            {
                uriPath = "https://world.openfoodfacts.org/api/v2/product/code=" + result.Text + "&fields=product_name,image_front_url,proteins_100g,proteins_unit,proteins_value,carbohydrates_100g,energy-kcal_100g,carbohydrates_100g,fat_100g,fiber_100g";
            }


            HttpClient client = new HttpClient();
            Uri uri = new Uri(string.Format(uriPath, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Jsontest>(content, new JsonSerializerSettings() { Culture = System.Globalization.CultureInfo.InvariantCulture});

                FoodPage foodPage = new FoodPage();
                FoodViewModel foodPageVm = foodPage.BindingContext as FoodViewModel;
                foodPageVm.Name = product.product.product_name;
                foodPageVm.ImageSourcePath = product.product.image_front_url;
                foodPageVm.Proteins = product.product.proteins_100g;
                foodPageVm.Carbs = product.product.carbohydrates_100g;
                foodPageVm.Fats = product.product.fat_100g;
                foodPageVm.ServingSize = 100;

                await Application.Current.MainPage.Navigation.PushAsync(foodPage);


                //Path = product.product.image_front_url;
                //OnlineName = product.product.product_name;
            }
            else
            {
                RSPopup rSPopup = new RSPopup("", "Not found");
                rSPopup.Show();
            }
        }

        public IObservable<Product> Products { get; set; }



        private string path;
        public string Path { get { return path; } set { path = value; OnPropertyChanged("Path"); } }

        private string onlineName;
        public string OnlineName { get { return onlineName; } set { onlineName = value; OnPropertyChanged("OnlineName"); } }
    }
}
