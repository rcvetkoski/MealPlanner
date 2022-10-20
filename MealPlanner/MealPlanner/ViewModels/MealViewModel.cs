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
    public class MealViewModel : BaseViewModel
    {
        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        private double servingSize;
        private string imageSourcePath;
        public string ImageSourcePath { get { return imageSourcePath; } set { imageSourcePath = value; OnPropertyChanged("ImageSourcePath"); } }
        private ImageSource imageSource;
        public ImageSource ImageSource { get { return imageSource; } set { imageSource = value; OnPropertyChanged("ImageSource"); } }
        public double ServingSize { get { return servingSize; } set { servingSize = value; OnPropertyChanged("ServingSize"); } }
        private string description;
        public string Description { get { return description; } set { description = value; OnPropertyChanged("Description"); } }
        private AlimentUnitEnum unit;
        public AlimentUnitEnum Unit { get { return unit; } set { unit = value; OnPropertyChanged("Unit"); } }
        private bool isNew;
        public bool IsNew { get { return isNew; } set { isNew = value; OnPropertyChanged("IsNew"); } }

        private Meal currentMeal;
        public Meal CurrentMeal { get { return currentMeal; } set { currentMeal = value; OnPropertyChanged("CurrentMeal"); } }

        public ObservableCollection<Food> Foods { get; set; }


        public MealViewModel()
        {
            CurrentMeal = new Meal();
            Foods = new ObservableCollection<Food>();
            IsNew = true;
            AddFoodCommand = new Command(AddFood);
            SaveCommand = new Command(SaveMeal);
            UpdateCommand = new Command(UpdateMeal);
            AddImageCommand = new Command(AddImage);    
            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
            DelettedMealFoods = new List<MealFood>();
        }

        public void FillMealProperties(Meal existingMeal)
        {
            Name = existingMeal.Name;
            ServingSize = existingMeal.ServingSize;
            Unit = existingMeal.Unit;
            Description = existingMeal.Description;
            ImageSourcePath = existingMeal.ImageSourcePath;
            ImageSource = existingMeal.ImageSource;
            foreach(Food food in existingMeal.Foods)
                this.Foods.Add(food);   
        }

        public ICommand SaveCommand { get; set; }

        private async void SaveMeal()
        {
            CurrentMeal.Name = this.Name;
            CurrentMeal.ImageSourcePath = this.ImageSourcePath;
            CurrentMeal.ServingSize = this.ServingSize;
            CurrentMeal.Unit = this.Unit;
            CurrentMeal.Description = this.Description;
            CurrentMeal.OriginalServingSize = CurrentMeal.ServingSize;
            CurrentMeal.Foods = this.Foods;
            RefData.Meals.Add(CurrentMeal);
            RefData.Aliments.Add(CurrentMeal);
            RefData.FilteredAliments.Add(CurrentMeal);
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

            // Update meal values
            RefData.UpdateMealValues(CurrentMeal);

            await Shell.Current.GoToAsync("..");
            //await Application.Current.MainPage.Navigation.PopAsync();
        }


        public ICommand UpdateCommand { get; set; }

        private async void UpdateMeal()
        {
            if (CurrentMeal == null)
                return;

            CurrentMeal.Name = this.Name;
            CurrentMeal.ImageSourcePath = this.ImageSourcePath;
            CurrentMeal.ServingSize = this.ServingSize;
            CurrentMeal.Unit = this.Unit;
            CurrentMeal.Description = this.Description;
            CurrentMeal.OriginalServingSize = CurrentMeal.ServingSize;
            CurrentMeal.Foods = this.Foods;


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
                        meal.ImageSourcePath = CurrentMeal.ImageSourcePath;
                        meal.ImageBlob = CurrentMeal.ImageBlob;
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
            await Shell.Current.GoToAsync("..");
            //await Application.Current.MainPage.Navigation.PopAsync();
        }

        public ICommand AddFoodCommand { get; set; }
        public void AddFood()
        {
            AddAlimentPage addAlimentPage = new AddAlimentPage();
            (addAlimentPage.BindingContext as AddAlimentViewModel).CurrentMeal = this.CurrentMeal;
            (addAlimentPage.BindingContext as AddAlimentViewModel).MealSwitchVisibility = false;
            (addAlimentPage.BindingContext as AddAlimentViewModel).CurrentMealTempFoods = this.Foods;
            App.Current.MainPage.Navigation.PushAsync(addAlimentPage);
        }


        public ICommand DeletteAlimentCommand { get; set; }
        private void DeletteAliment(object[] objects)
        {
            Meal meal = objects[0] as Meal;
            Food food = objects[1] as Food;

            Foods.Remove(food);

            var mealFood = RefData.MealFoods.Where(x => x.Id == food.MealFoodId).FirstOrDefault();

            DelettedMealFoods.Add(mealFood);
        }

        private List<MealFood> DelettedMealFoods { get; set; }


        public ICommand AddImageCommand { get; set; }
        private async void AddImage()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo, CurrentMeal);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {ImageSourcePath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        async Task LoadPhotoAsync(FileResult photo, Aliment aliment)
        {
            // canceled
            if (photo == null)
            {
                ImageSourcePath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);


            var resizedFile = Path.Combine(FileSystem.CacheDirectory, $"{aliment.Name}{aliment.Id}");
            App.ImageService.ResizeImage(newFile, resizedFile, 30);
            ImageSourcePath = resizedFile;

            CurrentMeal.ImageBlob = File.ReadAllBytes(resizedFile);
            ImageSource = CurrentMeal.ImageSource;

            if (File.Exists(ImageSourcePath))
                File.Delete(ImageSourcePath);
        }
    }
}
