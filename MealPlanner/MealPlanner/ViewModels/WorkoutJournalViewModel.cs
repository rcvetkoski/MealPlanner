using MealPlanner.Models;
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
            UpdateExerciceCommand = new Command<Exercice>(UpdateExercice);
        }

        public ICommand AddExerciceCommand { get; set; }
        private async void AddExercice()
        {
            //AddExercicePage addExercicePage = new AddExercicePage();
            //var vm = addExercicePage.BindingContext as AddExerciceViewModel;
            //vm.SelectedWorkout = RefData.CurrentWorkout;

            await Shell.Current.GoToAsync(nameof(AddExercicePage));
        }

        public ICommand UpdateExerciceCommand { get; set; }
        private async void UpdateExercice(Exercice exercice)
        {
            ExercicePage exercicePage = new ExercicePage();
            var vm = exercicePage.BindingContext as ExerciceViewModel;
            vm.CurrentExercice = exercice;
            foreach (Set set in exercice.Sets)
            {
                vm.CopiedSets.Add(set);
            }
            vm.CanDeleteItem = true;
            vm.CanUpdateItem = true;

            await Shell.Current.Navigation.PushAsync(exercicePage);
        }
    }
}
