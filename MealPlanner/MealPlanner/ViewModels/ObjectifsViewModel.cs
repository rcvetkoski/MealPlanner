using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static MealPlanner.Models.User;

namespace MealPlanner.ViewModels
{
    public class ObjectifsViewModel : BaseViewModel
    {
        public ObjectifsViewModel()
        {
            Title = "Objectifs";
            SelectObjectifCommand = new Command<RadioButton>(SelectObjectif);
            var originalTDEE = Math.Round(RefData.User.BMR * RefData.User.SelectedPhysicalActivityLevel.Ratio * RefData.User.SelectedObjectif.Ratio, 0);
            SliderValue = RefData.User.AdjustedCalories != 0 ? (RefData.User.AdjustedCalories / originalTDEE) * 100 : 0;
        }

        public ICommand SelectObjectifCommand { get; set; }
        private async void SelectObjectif(RadioButton radioButton)
        {
            var objectifItem = radioButton.BindingContext as ObjectifItem;
            radioButton.IsChecked = true;
            RefData.User.SelectedObjectif = objectifItem;
            RefData.User.AdjustedCalories = 0;
            SliderValue = 0;
            OnPropertyChanged(nameof(SliderValue));
            await App.DataBaseRepo.UpdateUserAsync(RefData.User);
        }

        public double SliderValue { get; set; }
    }
}
