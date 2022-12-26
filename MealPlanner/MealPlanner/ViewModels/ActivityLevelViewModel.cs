using System;
using System.Collections.Generic;
using System.Text;
using static MealPlanner.Models.User;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class ActivityLevelViewModel : BaseViewModel
    {
        public ActivityLevelViewModel()
        {
            Title = "Activity level";
            SelectActivityLevelCommand = new Command<RadioButton>(SelectActivityLevel);
        }

        public ICommand SelectActivityLevelCommand { get; set; }
        private async void SelectActivityLevel(RadioButton radioButton)
        {
            var palItem = radioButton.BindingContext as PALItem;
            radioButton.IsChecked = true;
            RefData.User.SelectedPhysicalActivityLevel = palItem;
            await App.DataBaseRepo.UpdateUserAsync(RefData.User);
        }
    }
}
