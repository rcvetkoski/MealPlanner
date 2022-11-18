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
            CopiedAliments = new List<Aliment>();
        }

        public List<Aliment> CopiedAliments { get; set; }
        public Log CopiedLog { get; set; }

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
            var labelStyle = Application.Current.Resources["LabelSmall"] as Style;
            Label label = new Label() { Text = meal.Name, Style = labelStyle, FontAttributes = FontAttributes.Bold };
            Label label1 = new Label() { Text = "Copy aliments", Style = labelStyle };
            label1.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() => 
                {
                    CopiedAliments.Clear();
                    foreach(Aliment aliment in meal.Aliments)
                        CopiedAliments.Add(RefData.CreateAndCopyAlimentProperties(aliment));

                    rSPopup.Close();
                })
            });

            Label label2 = new Label() { Text = "Paste aliments", IsVisible = CopiedAliments.Any(), Style = labelStyle };
            label2.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    foreach (Aliment aliment in CopiedAliments)
                        RefData.AddAliment(aliment, meal);

                    rSPopup.Close();
                })
            });

            Label label3 = new Label() { Text = "Set Hour", Style = labelStyle };
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
        private void UpdateAliment(object[] objects)
        {
            var meal = objects[0] as Meal;
            var aliment = objects[1] as Aliment;

            if (aliment is Aliment)
            {
                RSPopup rSPopup = new RSPopup();
                rSPopup.SetMargin(20, 20, 20, 20);
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


                    MealAliment mealAliment = await App.DataBaseRepo.GetMealAlimentAsync(aliment.MealAlimentId);
                    mealAliment.ServingSize = rSPopupAlimentDetailPageBindingContext.AlimentServingSize;
                    await App.DataBaseRepo.UpdateMealAliment(mealAliment);

                    var mealAlimentToUpdate = RefData.MealAliments.FirstOrDefault(x => x.Id == mealAliment.Id);
                    if(mealAlimentToUpdate != null)
                    {
                        mealAlimentToUpdate.AlimentType = mealAliment.AlimentType;
                        mealAlimentToUpdate.ServingSize = mealAliment.ServingSize;
                    }

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

            //await Shell.Current.Navigation.PushAsync(new StatisticsPage());
        }


        public ICommand DayOptionsCommand { get; set; }
        private void DayOptions()
        {
            RSPopup rSPopup = new RSPopup("", "", Xamarin.RSControls.Enums.RSPopupPositionEnum.Bottom);
            rSPopup.Style = Application.Current.Resources["RSPopup"] as Style;
            rSPopup.SetPopupSize(Xamarin.RSControls.Enums.RSPopupSizeEnum.MatchParent, Xamarin.RSControls.Enums.RSPopupSizeEnum.WrapContent);
            rSPopup.SetPopupAnimation(Xamarin.RSControls.Enums.RSPopupAnimationEnum.BottomToTop);

            StackLayout stackLayout = new StackLayout() { Margin = 20, Spacing = 20 };
            var labelStyle = Application.Current.Resources["LabelSmall"] as Style;
            Label label = new Label() { Text = RefData.CurrentDay.ToString("dddd, dd, yyyy"), Style = labelStyle, FontAttributes = FontAttributes.Bold };
            Label label1 = new Label() { Text = "Copy day", Style = labelStyle };
            label1.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    CopiedLog = RefData.GetLog(RefData.CurrentDay);
                    rSPopup.Close();
                })
            });

            bool canCopy = CopiedLog != null ? CopiedLog.Date.Day != RefData.CurrentDay.Day || CopiedLog.Date.Month != RefData.CurrentDay.Month || CopiedLog.Date.Year != RefData.CurrentDay.Year : false;
            Label label2 = new Label() { Text = "Paste day", IsVisible = CopiedLog != null && canCopy, Style = labelStyle };
            label2.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    RefData.Meals.Clear();

                    Log currentLog = RefData.GetLog(RefData.CurrentDay);

                    // delette existing meals
                    foreach (Meal currentMeal in currentLog.Meals)
                    {
                        // meals
                        var currentLogMeal = RefData.LogMeals.FirstOrDefault(x => x.MealId == currentMeal.Id && x.LogId == currentLog.Id);
                        if (currentLogMeal == null)
                            continue;

                        await App.DataBaseRepo.DeleteLogMealAsync(currentLogMeal);
                        RefData.LogMeals.Remove(currentLogMeal);

                        // aliments
                        foreach (Aliment aliment in currentMeal.Aliments)
                        {
                            MealAliment mealAliment = RefData.MealAliments.FirstOrDefault(x => x.MealId == currentMeal.Id && x.AlimentId == aliment.Id);
                            if (aliment == null)
                                continue;

                            await App.DataBaseRepo.DeleteMealAlimentAsync(mealAliment);
                            RefData.MealAliments.Remove(mealAliment);
                        }
                    }
                    currentLog.Meals.Clear();


                    foreach (Meal copiedMeal in CopiedLog.Meals)
                    {
                        Meal meal = new Meal() { Name = copiedMeal.Name, Order = copiedMeal.Order };
                        await App.DataBaseRepo.AddMealAsync(meal);
                        RefData.AllMeals.Add(meal);
                        RefData.PopulateMeal(meal, copiedMeal);
                        RefData.Meals.Add(meal);

                        LogMeal logMeal = new LogMeal() { LogId = currentLog.Id, MealId = meal.Id };
                        await App.DataBaseRepo.AddLogMealAsync(logMeal);
                        RefData.LogMeals.Add(logMeal);
                    }

                    CopiedLog = null;
                    rSPopup.Close();
                })
            });

            Label label3 = new Label() { Text = "Import from saved days", Style = labelStyle };
            label3.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
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
            //RefData.GetMealsAtDate(RefData.CurrentDay);
            //RefData.UpdateDailyValues();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        public ICommand ResetCurrentDayCommand { get; set; }
        private void ResetCurrentDay()
        {
            RefData.CurrentDay = DateTime.Now;
            SetTitle();
            //RefData.GetMealsAtDate(RefData.CurrentDay);
            //RefData.UpdateDailyValues();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        ~HomeViewModel()
        {
            Console.WriteLine(Title);
        }
    }
}