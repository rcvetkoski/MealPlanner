using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class EditExerciceViewModel : BaseViewModel
    {
        public EditExerciceViewModel()
        {
            SaveExerciceCommand = new Command(SaveExercice);
            EditExerciceCommand = new Command(EditExercice);
            AddSetCommand = new Command(AddSet);
        }

        public ICommand SaveExerciceCommand { get; set; }
        private async void SaveExercice()
        {
            RefData.Exercices.Add(CurrentExercice);
            await App.DataBaseRepo.AddExerciceAsync(CurrentExercice);
            await Shell.Current.Navigation.PopAsync();
        }

        public ICommand EditExerciceCommand { get; set; }
        private async void EditExercice()
        {

        }

        public ICommand AddSetCommand { get; set; }
        private async void AddSet()
        {

        }
    }
}
