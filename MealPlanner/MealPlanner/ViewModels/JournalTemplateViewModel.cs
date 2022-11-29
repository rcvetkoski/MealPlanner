using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.RSControls.Controls;

namespace MealPlanner.ViewModels
{
    public class JournalTemplateViewModel : BaseViewModel
    {
        public JournalTemplateViewModel()
        {
            Title = "Journal Template";
            AddNewJournalTemplateCommand = new Command(AddNewJournalTemplate);
            DayOfWeeks = new List<DayOfWeek>();
            foreach (DayOfWeek dayOfWeek in (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek)))
                DayOfWeeks.Add(dayOfWeek);

            DayOptionsCommand = new Command<DayOfWeekHelper>(DayOptions);
        }

        public List<DayOfWeek> DayOfWeeks { get; set; }
        public ICommand AddNewJournalTemplateCommand { get; set; }
        private async void AddNewJournalTemplate()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Create new Journal Template", "Enter Journal Template Name");

            if (string.IsNullOrEmpty(result))
                return;


            // Check if one with same name already exists
            var existingJournalTemplate = RefData.JournalTemplates.FirstOrDefault(x=> x.Name == result);
            if (existingJournalTemplate != null)
            {
                await Application.Current.MainPage.DisplayAlert("", $"{result} already exists", "Ok");
                return;
            }


            // Create new JournalTemplate
            JournalTemplate journalTemplate = new JournalTemplate() { Name = result};  
            await App.DataBaseRepo.AddJournalTemplateAsync(journalTemplate);
            RefData.JournalTemplates.Add(journalTemplate);

            // Create meals for every day in the week
            foreach(DayOfWeek dayOfWeek in DayOfWeeks)
            {
                foreach (TemplateMeal templateMeal in RefData.TemplateMeals)
                {
                    Meal meal = new Meal() { Name = templateMeal.Name, Order = templateMeal.Order };
                    await App.DataBaseRepo.AddMealAsync(meal);
                    RefData.AllMeals.Add(meal);
                    journalTemplate.DaysOfWeek.FirstOrDefault(x=> x.DayOfWeek == dayOfWeek).Meals.Add(meal);

                    JournalTemplateMeal journalTemplateMeal = new JournalTemplateMeal() { JournalTemplateId = journalTemplate.Id, MealId = meal.Id, DayOfWeek = dayOfWeek };
                    await App.DataBaseRepo.AddJournalTemplateMealAsync(journalTemplateMeal);
                    RefData.JournalTemplateMeals.Add(journalTemplateMeal);
                }
            }
        }

        /// <summary>
        /// Opens new Journal day template for edit
        /// </summary>
        /// <param name="dayOfWeekHelper"></param>
        /// <param name="rSPopup"></param>
        private async void EditJournalTemplate(DayOfWeekHelper dayOfWeekHelper, RSPopup rSPopup)
        {
            HomePage homePage = new HomePage(Helpers.Enums.HomePageTypeEnum.JournalTemplate, dayOfWeekHelper.DayOfWeek);
            (homePage.BindingContext as HomeViewModel).Title = dayOfWeekHelper.DayOfWeek.ToString();
            Shell.SetTitleView(homePage, null);
            Shell.SetTabBarIsVisible(homePage, false);
            await Shell.Current.Navigation.PushAsync(homePage);
            (homePage.BindingContext as HomeViewModel).RefData.UpdateDailyValues();
            rSPopup.Close();
        }
        public ICommand DayOptionsCommand { get; set; }
        private void DayOptions(DayOfWeekHelper dayOfWeek)
        {
            Log currentLog = RefData.GetLog(RefData.CurrentDay);

            RSPopup rSPopup = new RSPopup("", "", Xamarin.RSControls.Enums.RSPopupPositionEnum.Bottom);
            rSPopup.Style = Application.Current.Resources["RSPopup"] as Style;
            rSPopup.SetPopupSize(Xamarin.RSControls.Enums.RSPopupSizeEnum.MatchParent, Xamarin.RSControls.Enums.RSPopupSizeEnum.WrapContent);
            rSPopup.SetPopupAnimation(Xamarin.RSControls.Enums.RSPopupAnimationEnum.BottomToTop);

            StackLayout stackLayout = new StackLayout()
            {
                Margin = 20,
                Spacing = 20
            };

            var labelStyle = Application.Current.Resources["LabelPopup"] as Style;

            Label label = new Label()
            {
                Text = $"{RefData.CurrentJournalTemplate.Name} / {dayOfWeek.DayOfWeek.ToString()}",
                Style = labelStyle,
                FontAttributes = FontAttributes.Bold 
            };

            Label label1 = new Label() 
            { 
                Text = $"Import day to {currentLog.Date.ToString("dd MMM yyyy")}",
                Style = labelStyle
            };
            label1.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    RefData.Meals.Clear();


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


                    foreach (Meal copiedMeal in dayOfWeek.Meals)
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

                    rSPopup.Close();
                })
            });

            Label label2 = new Label()
            {
                Style = labelStyle,
                Text = "Edit"
            };
            label2.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    EditJournalTemplate(dayOfWeek, rSPopup);
                })
            });

            Label label3 = new Label()
            {
                Text = "Cancel",
                TextColor = Color.Red 
            };
            label3.GestureRecognizers.Add(new TapGestureRecognizer()
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
            rSPopup.SetCustomView(stackLayout);

            rSPopup.Show();
        }

        /// <summary>
        /// Sets flag to autogenerate journal based on template
        /// Deletes all journal logs from today and up to 7 days in the future
        /// </summary>
        public async void AutoGenerateJournal()
        {
            // Delete all logs from today up to 7 days ahead
            DateTime dateTime = DateTime.Now;
            for(int i = 0; i < 7; i++)
            {
                // get log for specified date
                Log log = RefData.GetLog(dateTime);

                if (log == null)
                {
                    // go to next day
                    dateTime = dateTime.AddDays(1);
                    continue;
                }

                // Delete log form db
                await App.DataBaseRepo.DeleteLogAsync(log);
                RefData.Logs.Remove(log);

                // Delete meal form logs as well as the linked aliments
                foreach(Meal meal in log.Meals)
                {
                    LogMeal logMeal = RefData.LogMeals.FirstOrDefault(x => x.LogId == log.Id && x.MealId == meal.Id);
                    await App.DataBaseRepo.DeleteLogMealAsync(logMeal);
                    RefData.LogMeals.Remove(logMeal);
                    await App.DataBaseRepo.DeleteMealAsync(meal);
                    RefData.AllMeals.Remove(meal);

                    // Delete aliments
                    foreach(Aliment aliment in meal.Aliments)
                    {
                        MealAliment mealAliment = RefData.MealAliments.FirstOrDefault(x => x.AlimentId == aliment.Id && x.MealId == meal.Id);
                        await App.DataBaseRepo.DeleteMealAlimentAsync(mealAliment);
                        RefData.MealAliments.Remove(mealAliment);
                    }
                }

                // go to next day
                dateTime = dateTime.AddDays(1);
            }


            // Set AutoGenerate flag to true and currentJournalId in User table
            RefData.User.AutoGenerateJournalEnabled = true; 
            RefData.User.CurrentJournalTemplateId = RefData.CurrentJournalTemplate.Id;
            await App.DataBaseRepo.UpdateUserAsync(RefData.User);
            RefData.LastUsedHomePageType = HomePageTypeEnum.JournalTemplate;
        }

        ~JournalTemplateViewModel()
        {

        }
    }
}
