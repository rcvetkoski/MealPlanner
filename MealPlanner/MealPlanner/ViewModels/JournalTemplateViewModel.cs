using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

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

            DistributionCommand = new Command<DayOfWeek>(Distribution);
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
            JournalTemplate journalTemplate = new JournalTemplate() { Name = result, Meals = new System.Collections.ObjectModel.ObservableCollection<Meal>() };  
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
                    journalTemplate.Meals.Add(meal);

                    JournalTemplateMeal journalTemplateMeal = new JournalTemplateMeal() { JournalTemplateId = journalTemplate.Id, MealId = meal.Id, DayOfWeek = dayOfWeek };
                    await App.DataBaseRepo.AddJournalTemplateMealAsync(journalTemplateMeal);
                    RefData.JournalTemplateMeals.Add(journalTemplateMeal);
                }
            }
        }


        public ICommand DistributionCommand { get; set; }
        private void Distribution(DayOfWeek dayOfWeek)
        {
            RefData.CreateJournalTemplates(dayOfWeek);
            HomePage homePage = new HomePage();
            Shell.SetTitleView(homePage, null);
            (homePage.BindingContext as HomeViewModel).Title = dayOfWeek.ToString();
            Shell.Current.Navigation.PushAsync(homePage);
        }
    }
}
