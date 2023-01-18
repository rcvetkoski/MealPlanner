using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class WorkoutJournalViewModel : BaseViewModel
    {
        public WorkoutJournalViewModel()
        {
            Title = "Workout Journal";
            AddExerciceCommand = new Command(AddExercice);
        }

        public ICommand AddExerciceCommand { get; set; }
        private async void AddExercice()
        {
            await Shell.Current.GoToAsync(nameof(AddExercicePage));
        }
    }
}
