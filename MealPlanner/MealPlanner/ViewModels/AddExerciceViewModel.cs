using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class AddExerciceViewModel : BaseViewModel
    {
        public AddExerciceViewModel()
        {
            Title = "Add Exercice";
            CreateNewExerciceCommand = new Command(CreateNewExercice);
        }

        public ICommand CreateNewExerciceCommand { get; set; }
        private async void CreateNewExercice()
        {
            ExercicePage exercicePage = new ExercicePage();
            var vm = exercicePage.BindingContext as ExerciceViewModel;
            vm.CurrentExercice = new Exercice();
            vm.IsNew = true;

            await Shell.Current.Navigation.PushAsync(exercicePage);
        }
    }
}
