using System;
using System.Collections.Generic;
using System.Text;
using static MealPlanner.Models.User;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class MacrosViewModel : BaseViewModel
    {
        public MacrosViewModel()
        {
            Title = "Macros";
            SelectTypeOfRegimeCommand = new Command<RadioButton>(TypeOfRegime);
        }

        public ICommand SelectTypeOfRegimeCommand { get; set; }
        private async void TypeOfRegime(RadioButton radioButton)
        {
            var typeOfRegimeItem = radioButton.BindingContext as TypeOfRegimeItem;
            radioButton.IsChecked = true;
            RefData.User.SelectedTypeOfRegime = typeOfRegimeItem;
            await App.DataBaseRepo.UpdateUserAsync(RefData.User);
        }
    }
}
