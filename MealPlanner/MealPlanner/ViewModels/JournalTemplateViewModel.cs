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

            DistributionCommand = new Command<DayOfWeekHelper>(Distribution);
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


        public ICommand DistributionCommand { get; set; }
        private async void Distribution(DayOfWeekHelper dayOfWeekHelper)
        {
            HomePage homePage = new HomePage(Helpers.Enums.HomePageTypeEnum.JournalTemplate, dayOfWeekHelper.DayOfWeek);
            Shell.SetTitleView(homePage, null);
            (homePage.BindingContext as HomeViewModel).Title = dayOfWeekHelper.DayOfWeek.ToString();
            await Shell.Current.Navigation.PushAsync(homePage);
            Shell.SetTabBarIsVisible(homePage, false);
        }


        public ICommand DayOptionsCommand { get; set; }
        private void DayOptions(DayOfWeekHelper dayOfWeek)
        {
            Log currentLog = RefData.GetLog(RefData.CurrentDay);

            RSPopup rSPopup = new RSPopup("", "", Xamarin.RSControls.Enums.RSPopupPositionEnum.Bottom);
            rSPopup.Style = Application.Current.Resources["RSPopup"] as Style;
            rSPopup.SetPopupSize(Xamarin.RSControls.Enums.RSPopupSizeEnum.MatchParent, Xamarin.RSControls.Enums.RSPopupSizeEnum.WrapContent);
            rSPopup.SetPopupAnimation(Xamarin.RSControls.Enums.RSPopupAnimationEnum.BottomToTop);

            StackLayout stackLayout = new StackLayout() { Margin = 20, Spacing = 20 };
            var labelStyle = Application.Current.Resources["LabelSmall"] as Style;
            Label label = new Label() { Text = "Monday", Style = labelStyle, FontAttributes = FontAttributes.Bold };

            Label label1 = new Label() { Text = $"Import day to {currentLog.Date.ToString("dd MMM yyyy")}", Style = labelStyle };
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

            Label label3 = new Label() { Text = "Cancel", TextColor = Color.Red };

            label3.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    rSPopup.Close();
                })
            });

            stackLayout.Children.Add(label);
            stackLayout.Children.Add(label1);
            stackLayout.Children.Add(label3);
            rSPopup.SetCustomView(stackLayout);

            rSPopup.Show();
        }
    }
}
