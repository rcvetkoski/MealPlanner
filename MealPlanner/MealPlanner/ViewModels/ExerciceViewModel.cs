using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class ExerciceViewModel : BaseViewModel
    {
        public ExerciceViewModel()
        {
            Title = "Exercice";
            SaveExerciceCommand = new Command(SaveExercice);
            AddSetCommand = new Command(AddSet);
        }

        public ICommand SaveExerciceCommand { get; set; }
        private async void SaveExercice()
        {

        }

        public ICommand AddSetCommand { get; set; }
        private async void AddSet()
        {

        }
    }
}
