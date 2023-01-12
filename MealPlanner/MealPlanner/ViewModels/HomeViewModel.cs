using MealPlanner.Helpers;
using MealPlanner.Helpers.Enums;
using MealPlanner.Helpers.Extensions;
using MealPlanner.Models;
using MealPlanner.Views;
using MealPlanner.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
            SetTitle();
            MaximumDate = RefData.CurrentDay.AddDays(7);
            MealOptionsCommand = new Command<Meal>(MealOptions);
            DeletteAlimentCommand = new Command<object[]>(DeletteAliment);
            UpdateAlimentCommand = new Command<object[]>(UpdateAliment);
            AddAlimentCommand = new Command<Meal>(AddAliment);
            DayOptionsCommand = new Command(DayOptions);
            OpenCalendarCommand = new Command<DatePicker>(OpenCalendar);
            PreviousDayCommand = new Command(PreviousDay);
            NextDayCommand = new Command(NextDay);
            ResetCurrentDayCommand = new Command(ResetCurrentDay);
        }

        public int SelectedJournalTemplateDayOfWeek { get; set; }

        public DateTime MaximumDate { get; set; }
        public bool NextDayCommandVisible
        {
            get
            {
                return MaximumDate > RefData.CurrentDay.AddDays(1);
            }
        }

        public void SetTitle()
        {
            Title = RefData.CurrentDay.Day == DateTime.Now.Day ? "Today" : RefData.CurrentDay.ToString(("dd MMM"));
        }

        public ICommand MealOptionsCommand { get; set; }
        private void MealOptions(Meal meal)
        {
            RSPopup rSPopup = new RSPopup("", "", Xamarin.RSControls.Enums.RSPopupPositionEnum.Bottom);
            rSPopup.Style = Application.Current.Resources["RSPopup"] as Style;
            rSPopup.SetPopupSize(Xamarin.RSControls.Enums.RSPopupSizeEnum.MatchParent, Xamarin.RSControls.Enums.RSPopupSizeEnum.WrapContent);
            rSPopup.SetPopupAnimation(Xamarin.RSControls.Enums.RSPopupAnimationEnum.BottomToTop);

            StackLayout stackLayout = new StackLayout() { Margin = 20, Spacing = 20};
            var labelStyle = Application.Current.Resources["LabelPopup"] as Style;

            Label label = new Label()
            { 
                Text = meal.Name,
                Style = labelStyle, 
                FontAttributes = FontAttributes.Bold
            };

            Label label1 = new Label()
            { 
                Text = "Copy aliments",
                IsVisible = meal.Aliments.Any(),
                Style = labelStyle 
            };
            label1.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() => 
                {
                    RefData.CopiedAliments.Clear();
                    foreach(Aliment aliment in meal.Aliments)
                        RefData.CopiedAliments.Add(RefData.CreateAndCopyAlimentProperties(aliment));

                    rSPopup.Close();
                })
            });

            Label label2 = new Label()
            { 
                Text = "Paste aliments",
                IsVisible = RefData.CopiedAliments.Any(),
                Style = labelStyle 
            };
            label2.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    foreach (Aliment aliment in RefData.CopiedAliments)
                        RefData.AddAliment(aliment, meal);

                    rSPopup.Close();
                })
            });

            Label label3 = new Label() 
            { 
                Text = "Set Hour",
                Style = labelStyle 
            };

            Label label4 = new Label()
            { 
                Text = "Save as recipe",
                IsVisible = meal.Aliments.Where(x=> x.AlimentType == AlimentTypeEnum.Food).Count() > 1,
                Style = labelStyle
            };
            label4.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    // Avoid double action invoke
                    label4.IsEnabled = false;

                    // Recipe page
                    RecipePage recipePage = new RecipePage();
                    var vm = (recipePage.BindingContext as RecipeViewModel);
                    Recipe recipe = new Recipe();

                    foreach(Aliment aliment in meal.Aliments)
                    {
                        if (aliment.AlimentType == AlimentTypeEnum.Recipe)
                            continue;

                        recipe.Foods.Add(aliment as Food);

                        // Delete 
                    }
                    vm.CurrentAliment = recipe;
                    await Shell.Current.Navigation.PushAsync(recipePage);
                    rSPopup.Close();
                })
            });

            Label label5 = new Label() 
            { 
                Text = "Cancel",
                TextColor = Color.Red 
            };
            label5.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    rSPopup.Close();
                })
            });

            stackLayout.Children.Add(label);
            stackLayout.Children.Add(label1);
            stackLayout.Children.Add(label2);
            stackLayout.Children.Add(label3);
            stackLayout.Children.Add(label4);
            stackLayout.Children.Add(label5);
            rSPopup.SetCustomView(stackLayout);

            rSPopup.Show(); 
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
                var realMealAliment = RefData.MealAliments.FirstOrDefault(x => x.Id == mealAliment.Id);
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
        private async void UpdateAliment(object[] objects)
        {
            var meal = objects[0] as Meal;
            var aliment = objects[1] as Aliment;

            if (aliment is Aliment)
            {
                FoodPage foodPage = new FoodPage();
                FoodViewModel foodViewModel = foodPage.BindingContext as FoodViewModel;
                foodViewModel.CanEditItem = false;
                foodViewModel.CurrentAliment = RefData.Aliments.FirstOrDefault(x => x.Id == aliment.Id && x.AlimentType == aliment.AlimentType);
                foodViewModel.AlimentToUpdate = aliment;
                foodViewModel.IsInUpdateMode = true;
                foodViewModel.CanAddItem = false;
                foodViewModel.SelectedMeal = meal;
                //foodViewModel.InitProperties(aliment);
                await Shell.Current.Navigation.PushAsync(foodPage);
            }
        }

        //private AddAlimentPage addAlimentPage;
        public ICommand AddAlimentCommand { get; set; }
        private async void AddAliment(Meal meal)
        {
            //if(addAlimentPage == null)
            var addAlimentPage = new AddAlimentPage();

            (addAlimentPage.BindingContext as AddAlimentViewModel).SelectedMeal = meal;
            await App.Current.MainPage.Navigation.PushAsync(addAlimentPage);

            //await Shell.Current.Navigation.PushAsync(new StatisticsPage());

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();
        }


        public ICommand DayOptionsCommand { get; set; }
        private void DayOptions()
        {
            RSPopup rSPopup = new RSPopup("", "", Xamarin.RSControls.Enums.RSPopupPositionEnum.Bottom);
            rSPopup.Style = Application.Current.Resources["RSPopup"] as Style;
            rSPopup.SetPopupSize(Xamarin.RSControls.Enums.RSPopupSizeEnum.MatchParent, Xamarin.RSControls.Enums.RSPopupSizeEnum.WrapContent);
            rSPopup.SetPopupAnimation(Xamarin.RSControls.Enums.RSPopupAnimationEnum.BottomToTop);

            StackLayout stackLayout = new StackLayout() { Margin = 20, Spacing = 20 };

            var labelStyle = Application.Current.Resources["LabelPopup"] as Style;
            var dayOfWeekString = (DayOfWeek)SelectedJournalTemplateDayOfWeek;
            Label label = new Label() 
            { 
                Text = RefData.CurrentDay.ToString("dddd, dd, yyyy"),
                Style = labelStyle, FontAttributes = FontAttributes.Bold
            };

            Label label1 = new Label()
            { 
                Text = "Copy day", 
                Style = labelStyle
            };
            label1.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    var log = RefData.GetLog(RefData.CurrentDay);
                    RefData.CopiedDay = new CopiedDayHelper()
                    {
                        Date = log.Date,
                        HomePageType = HomePageTypeEnum.Normal,
                        DayOfWeek = -1,
                        Meals = log.Meals
                    };

                    rSPopup.Close();
                })
            });

            bool canCopy = false;

            if(RefData.CopiedDay != null)
            {
                canCopy = RefData.CopiedDay.DayOfWeek != SelectedJournalTemplateDayOfWeek;
            }
                
            Label label2 = new Label() { Text = "Paste day", IsVisible = canCopy, Style = labelStyle };
            label2.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    CopyToLogDay();
                    rSPopup.Close();
                })
            });

            Label label3 = new Label() { Text = "Import from saved days", Style = labelStyle};
            label3.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    label3.IsEnabled = false;
                    await Shell.Current.GoToAsync($"{nameof(JournalTemplatePage)}");
                    rSPopup.Close();
                })
            });


            Label label4 = new Label() { Text = "Cancel", TextColor = Color.Red };
            label4.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    rSPopup.Close();
                })
            });

            stackLayout.Children.Add(label);
            stackLayout.Children.Add(label1);
            stackLayout.Children.Add(label2);
            stackLayout.Children.Add(label3);
            stackLayout.Children.Add(label4);
            rSPopup.SetCustomView(stackLayout);

            rSPopup.Show();
        }

        /// <summary>
        /// Used when day is copied to log day and not journal template
        /// </summary>
        private async void CopyToLogDay()
        {
            RefData.Meals.Clear();

            Log currentLog = RefData.GetLog(RefData.CurrentDay);

            // delette existing meals and aliments
            foreach (Meal meal in currentLog.Meals)
            {
                // meals
                var logMeal = RefData.LogMeals.FirstOrDefault(x => x.MealId == meal.Id && x.LogId == currentLog.Id);
                if (logMeal == null)
                    continue;

                // Delete LogMeal from db
                await App.DataBaseRepo.DeleteLogMealAsync(logMeal);
                RefData.LogMeals.Remove(logMeal);

                // Delete Meal from db
                await App.DataBaseRepo.DeleteMealAsync(meal);
                RefData.AllMeals.Remove(meal);

                // aliments
                foreach (Aliment aliment in meal.Aliments)
                {
                    MealAliment mealAliment = RefData.MealAliments.FirstOrDefault(x => x.MealId == meal.Id && x.AlimentId == aliment.Id);
                    if (aliment == null)
                        continue;

                    // Delete MealAliment from db
                    await App.DataBaseRepo.DeleteMealAlimentAsync(mealAliment);
                    RefData.MealAliments.Remove(mealAliment);

                    // Delete Aliment from collection
                    RefData.Aliments.Remove(aliment);
                }
            }
            currentLog.Meals.Clear();

            // Copy and create new meals and aliments
            foreach (Meal copiedMeal in RefData.CopiedDay.Meals)
            {
                Meal meal = new Meal()
                {
                    Name = copiedMeal.Name,
                    Order = copiedMeal.Order
                };
                await App.DataBaseRepo.AddMealAsync(meal);
                RefData.AllMeals.Add(meal);
                RefData.PopulateMeal(meal, copiedMeal);
                RefData.Meals.Add(meal);

                LogMeal logMeal = new LogMeal() 
                {
                    LogId = currentLog.Id,
                    MealId = meal.Id 
                };
                await App.DataBaseRepo.AddLogMealAsync(logMeal);
                RefData.LogMeals.Add(logMeal);
                currentLog.Meals.Add(meal);
            }

            RefData.CopiedDay = null;
            RefData.UpdateDailyValues();
        }

        /// <summary>
        /// Used when day is copied to journal template day
        /// </summary>
        private async void CopyToJournalTemplateDay()
        {
            RefData.Meals.Clear();

            DayOfWeekHelper dayOfWeekHelper = RefData.CurrentJournalTemplate.DaysOfWeek.FirstOrDefault(x => (int)x.DayOfWeek == SelectedJournalTemplateDayOfWeek);
            if(dayOfWeekHelper == null)
                return; 


            // delette existing meals and aliments
            foreach (Meal meal in dayOfWeekHelper.Meals)
            {
                // meals
                var journalTemplateMeal = RefData.JournalTemplateMeals.FirstOrDefault(x => x.JournalTemplateId == RefData.CurrentJournalTemplate.Id && (int)x.DayOfWeek == SelectedJournalTemplateDayOfWeek);
                if (journalTemplateMeal == null)
                    continue;

                // Delete JournalTemplateMeal from db
                await App.DataBaseRepo.DeleteJournalTemplateMealAsync(journalTemplateMeal);
                RefData.JournalTemplateMeals.Remove(journalTemplateMeal);

                // Delete Meal from db
                await App.DataBaseRepo.DeleteMealAsync(meal);
                RefData.AllMeals.Remove(meal);

                // aliments
                foreach (Aliment aliment in meal.Aliments)
                {
                    MealAliment mealAliment = RefData.MealAliments.FirstOrDefault(x => x.MealId == meal.Id && x.AlimentId == aliment.Id);
                    if (aliment == null)
                        continue;

                    // Delete MealAliment from db
                    await App.DataBaseRepo.DeleteMealAlimentAsync(mealAliment);
                    RefData.MealAliments.Remove(mealAliment);

                    // Delete Aliment from collection
                    RefData.Aliments.Remove(aliment);
                }
            }
            dayOfWeekHelper.Meals.Clear();

            // Copy and create new meals and aliments
            foreach (Meal copiedMeal in RefData.CopiedDay.Meals)
            {
                Meal meal = new Meal()
                {
                    Name = copiedMeal.Name,
                    Order = copiedMeal.Order
                };
                await App.DataBaseRepo.AddMealAsync(meal);
                RefData.AllMeals.Add(meal);
                RefData.PopulateMeal(meal, copiedMeal);
                RefData.Meals.Add(meal);

                JournalTemplateMeal journalTemplateMeal = new JournalTemplateMeal()
                {
                    JournalTemplateId = RefData.CurrentJournalTemplate.Id,
                    DayOfWeek = dayOfWeekHelper.DayOfWeek,
                    MealId = meal.Id
                };
                await App.DataBaseRepo.AddJournalTemplateMealAsync(journalTemplateMeal);
                RefData.JournalTemplateMeals.Add(journalTemplateMeal);
                dayOfWeekHelper.Meals.Add(meal);
            }

            RefData.CopiedDay = null;
            RefData.UpdateDailyValues();
        }

        public ICommand OpenCalendarCommand { get; set; }
        private void OpenCalendar(DatePicker datePicker)
        {
            datePicker.Focus();
        }

        public ICommand PreviousDayCommand { get; set; }
        private void PreviousDay()
        {
            RefData.CurrentDay =  RefData.CurrentDay.Subtract(TimeSpan.FromDays(1));
            SetTitle();
            //RefData.GetMealsAtDate(RefData.CurrentDay);
            //RefData.UpdateDailyValues();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        public ICommand NextDayCommand { get; set; }
        private void NextDay()
        {
            RefData.CurrentDay = RefData.CurrentDay.AddDays(1);
            SetTitle();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        public ICommand ResetCurrentDayCommand { get; set; }
        private void ResetCurrentDay()
        {
            RefData.CurrentDay = DateTime.Now;
            SetTitle();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        ~HomeViewModel()
        {
            Console.WriteLine(Title);
        }
    }
}