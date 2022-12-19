using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class FoodViewModel : BaseViewModel
    {
        public FoodViewModel()
        {
            IsServingQuantityVisible = true;
            EditFoodCommand = new Command(EditFood);
            AddFoodCommand = new Command(AddFood);
            UpdateAlimentCommand = new Command(UpdateAliment);
            RemoveAlimentCommand = new Command(RemoveAliment);
        }

        public ObservableCollection<Aliment> CopyOfFilteredAliments { get; set; }

        public Aliment AlimentToUpdate { get; set; }

        public ICommand EditFoodCommand { get; set; }
        private async void EditFood()
        {
            EditFoodPage foodPage = new EditFoodPage();
            (foodPage.BindingContext as EditFoodViewModel).CurrentAliment = RefData.CreateAndCopyAlimentProperties(CurrentAliment);
            (foodPage.BindingContext as EditFoodViewModel).IsNew = IsNew;
            (foodPage.BindingContext as EditFoodViewModel).CopyOfFilteredAliments = CopyOfFilteredAliments;

            await Shell.Current.Navigation.PushAsync(foodPage);
        }

        public ICommand AddFoodCommand { get; set; }
        private async void AddFood()
        {
            var ratio = AlimentServingSize / CurrentAliment.OriginalServingSize;
            Aliment aliment = RefData.CreateAndCopyAlimentProperties(CurrentAliment, ratio);
            aliment.ServingSize = AlimentServingSize;

            var existingAliment = RefData.Aliments.Where(x => x.Id == aliment.Id && x.AlimentType == aliment.AlimentType).FirstOrDefault();

            // Save to db
            if(existingAliment == null)
            {
                await App.DataBaseRepo.AddFoodAsync(aliment as Food);
                RefData.Foods.Add(aliment as Food);
                RefData.Aliments.Add(aliment as Food);
                CopyOfFilteredAliments.Add(aliment as Food);
            }

            // Add aliment
            RefData.AddAliment(aliment, SelectedMeal);

            await Shell.Current.Navigation.PopAsync();
        }

        public ICommand UpdateAlimentCommand { get; set; }
        private async void UpdateAliment()
        {
            AlimentToUpdate.Proteins = AlimentProteins;
            AlimentToUpdate.Carbs = AlimentCarbs;
            AlimentToUpdate.Fats = AlimentFats;
            AlimentToUpdate.Calories = AlimentCalories;
            AlimentToUpdate.Unit = AlimentUnit;
            AlimentToUpdate.ServingSize = AlimentServingSize;

            // Update meal values
            RefData.UpdateMealValues(SelectedMeal);

            // Update daily values
            RefData.UpdateDailyValues();


            MealAliment mealAliment = await App.DataBaseRepo.GetMealAlimentAsync(AlimentToUpdate.MealAlimentId);
            mealAliment.ServingSize = AlimentServingSize;
            await App.DataBaseRepo.UpdateMealAliment(mealAliment);

            var mealAlimentToUpdate = RefData.MealAliments.FirstOrDefault(x => x.Id == mealAliment.Id);
            if (mealAlimentToUpdate != null)
            {
                mealAlimentToUpdate.AlimentType = mealAliment.AlimentType;
                mealAlimentToUpdate.ServingSize = mealAliment.ServingSize;
            }

            //await Shell.Current.Navigation.PopAsync();
        }

        public ICommand RemoveAlimentCommand { get; set; }
        private async void RemoveAliment()
        {
            MealAliment mealAliment = await App.DataBaseRepo.GetMealAlimentAsync(AlimentToUpdate.MealAlimentId);

            if (mealAliment != null)
            {
                await App.DataBaseRepo.DeleteMealAlimentAsync(mealAliment);
                var realMealAliment = RefData.MealAliments.FirstOrDefault(x => x.Id == mealAliment.Id);
                if (realMealAliment != null)
                    RefData.MealAliments.Remove(realMealAliment);
            }

            SelectedMeal.Aliments.Remove(AlimentToUpdate);

            // Update meal values
            RefData.UpdateMealValues(SelectedMeal);

            // Update daily values
            RefData.UpdateDailyValues();

            await Shell.Current.Navigation.PopAsync();
        }

        private bool isInUpdateMode;
        public bool IsInUpdateMode 
        {
            get
            {
                return isInUpdateMode;
            }
            set
            {
                if(isInUpdateMode != value)
                {
                    isInUpdateMode = value;
                    OnPropertyChanged(nameof(IsInUpdateMode));
                }
            }
        }
        public bool IsServingQuantityVisible { get; set; }

        // Calories
        private double alimentCalories;
        public double AlimentCalories
        {
            get
            {
                return alimentCalories;
            }
            set
            {
                if (alimentCalories != value)
                {
                    alimentCalories = value;
                    AlimentCaloriesProgress = alimentCalories / RefData.User.TDEE;
                    OnPropertyChanged("AlimentCalories");
                    OnPropertyChanged("AlimentCaloriesProgress");
                    OnPropertyChanged("AlimentCaloriesRatio");
                }
            }
        }
        public double AlimentCaloriesProgress { get; set; }
        public string AlimentCaloriesRatio
        {
            get
            {
                return $"{Math.Round(alimentCalories, 0)} / {RefData.User.TDEE}";
            }
        }

        // Proteins
        private double alimentProteins;
        public double AlimentProteins
        {
            get
            {
                return alimentProteins;
            }
            set
            {
                if (alimentProteins != value)
                {
                    alimentProteins = value;
                    AlimentProteinProgress = ((alimentProteins * 4) / AlimentCalories);
                    OnPropertyChanged("AlimentProteins");
                    OnPropertyChanged("AlimentProteinProgress");
                    OnPropertyChanged("AlimentProteinPercentage");
                    OnPropertyChanged("AlimentProteinQuantity");
                }
            }
        }
        public double AlimentProteinProgress { get; set; }
        public string AlimentProteinPercentage
        {
            get
            {
                return $"{Math.Round(AlimentProteinProgress * 100, 0)} %";
            }
        }
        public string AlimentProteinQuantity
        {
            get
            {
                return $"Protein ({Math.Round(AlimentProteins, 0)} g)";
            }
        }

        // Carbs
        private double alimentCarbs;
        public double AlimentCarbs
        {
            get
            {
                return alimentCarbs;
            }
            set
            {
                if (alimentCarbs != value)
                {
                    alimentCarbs = value;
                    AlimentCarbsProgress = ((alimentCarbs * 4) / AlimentCalories);
                    OnPropertyChanged("AlimentCarbs");
                    OnPropertyChanged("AlimentCarbsProgress");
                    OnPropertyChanged("AlimentCarbsPercentage");
                    OnPropertyChanged("AlimentCarbsQuantity");
                }
            }
        }
        public double AlimentCarbsProgress { get; set; }
        public string AlimentCarbsPercentage
        {
            get
            {
                return $"{Math.Round(AlimentCarbsProgress * 100, 0)} %";
            }
        }
        public string AlimentCarbsQuantity
        {
            get
            {
                return $"Carbs ({Math.Round(AlimentCarbs, 0)} g)";
            }
        }

        // Fats
        private double alimentFats;
        public double AlimentFats
        {
            get
            {
                return alimentFats;
            }
            set
            {
                if (alimentFats != value)
                {
                    alimentFats = value;
                    AlimentFatsProgress = ((alimentFats * 9) / AlimentCalories);
                    OnPropertyChanged("AlimentFats");
                    OnPropertyChanged("AlimentFatsProgress");
                    OnPropertyChanged("AlimentFatsPercentage");
                    OnPropertyChanged("AlimentFatsQuantity");
                }
            }
        }
        public double AlimentFatsProgress { get; set; }
        public string AlimentFatsPercentage
        {
            get
            {
                return $"{Math.Round(AlimentFatsProgress * 100, 0)} %";
            }
        }
        public string AlimentFatsQuantity
        {
            get
            {
                return $"Fats ({Math.Round(AlimentFats, 0)} g)";
            }
        }


        private double alimentServingSize;
        public double AlimentServingSize
        {
            get
            {
                return alimentServingSize;
            }
            set
            {
                if (alimentServingSize != value)
                {
                    alimentServingSize = value;
                    UpdateNutrimentValues();
                    OnPropertyChanged("AlimentServingSize");
                }
            }
        }

        public AlimentUnitEnum AlimentUnit { get; set; }

        private void UpdateNutrimentValues()
        {
            double ratio = AlimentServingSize / CurrentAliment.OriginalServingSize;

            AlimentCalories = CurrentAliment.Calories * ratio;
            AlimentProteins = CurrentAliment.Proteins * ratio;
            AlimentCarbs = CurrentAliment.Carbs * ratio;
            AlimentFats = CurrentAliment.Fats * ratio;
        }

        public void InitProperties(Aliment aliment)
        {
            AlimentCalories = aliment.Calories;
            AlimentProteins = aliment.Proteins;
            AlimentCarbs = aliment.Carbs;
            AlimentFats = aliment.Fats;
            AlimentServingSize = aliment.ServingSize;
            AlimentUnit = aliment.Unit;

            IsServingQuantityVisible = aliment.ServingQuantity <= 0 ? false : true;
            OnPropertyChanged("IsServingQuantityVisible");
        }
    }
}

