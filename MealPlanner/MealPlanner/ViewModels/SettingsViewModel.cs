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
    public class SettingsViewModel : BaseViewModel
    {
        public ICommand OpenUserPageCommand { get; set; }
        private async void OpenUserPage()
        {
            //await Shell.Current.GoToAsync($"{nameof(UserPage)}");
            await App.Current.MainPage.Navigation.PushAsync(new UserPage());
        }


        public ICommand OpenJournalTemplatePageCommand { get; set; }
        private async void OpenJournalTemplatePage()
        {
            await Shell.Current.GoToAsync($"{nameof(JournalTemplatePage)}");
        }

        public ICommand OpenCustomizeMealsPageCommand { get; set; }
        private async void OpenCustomizeMealsPage()
        {
            await Shell.Current.GoToAsync($"{nameof(CustomizeMealsPage)}");
        }

        public ICommand OpenUnitsPageCommand { get; set; }
        private async void OpenUnitsPage()
        {
            await Shell.Current.GoToAsync($"{nameof(UnitsPage)}");
        }

        public SettingsViewModel()
        {
            Title = "Settings";
            OpenUserPageCommand = new Command(OpenUserPage);
            OpenJournalTemplatePageCommand = new Command(OpenJournalTemplatePage);
            OpenCustomizeMealsPageCommand = new Command(OpenCustomizeMealsPage);
            OpenUnitsPageCommand = new Command(OpenUnitsPage);
        }

        ~SettingsViewModel()
        {

        }
    }
}
