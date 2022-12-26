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
        }

        public ICommand SelectObjectifCommand { get; set; }
        private async void SelectObjectif(RadioButton radioButton)
        {
            var objectifItem = radioButton.BindingContext as ObjectifItem;
            radioButton.IsChecked = true;
            RefData.User.SelectedObjectif = objectifItem;
            await App.DataBaseRepo.UpdateUserAsync(RefData.User);
        }
    }
}
