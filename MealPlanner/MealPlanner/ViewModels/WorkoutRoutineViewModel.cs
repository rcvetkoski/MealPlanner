using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class WorkoutRoutineViewModel : BaseViewModel
    {
        public WorkoutRoutineViewModel()
        {
            AddExerciceCommand = new Command(AddExercice);
            UpdateExerciceCommand = new Command<Exercice>(UpdateExercice);
        }

        public ICommand AddExerciceCommand { get; set; }
        private async void AddExercice()
        {
            Exercice exercice = new Exercice()
            {
                Name = "Exercice"
            };

            CurrentWorkoutRoutine.Exercices.Add(exercice);
        }

        public ICommand UpdateExerciceCommand { get; set; }
        private async void UpdateExercice(Exercice exercice)
        {
            ExercicePage exercicePage = new ExercicePage();
            var vm = exercicePage.BindingContext as ExerciceViewModel;
            vm.CurrentExercice = exercice;
            vm.SelectedMinutes = exercice.RestTimeBetweenSets.Minutes;
            vm.SelectedSecondes = exercice.RestTimeBetweenSets.Seconds;
            foreach (Set set in exercice.Sets)
            {
                vm.CopiedSets.Add(set);
            }

            // Fill Previous sets if any
            vm.LastTimePerformance = RefData.GetExerciceHistoryLastTime(exercice);
            vm.CurrentExercice.History = RefData.GetExerciceHistory((int)PeriodEnum.AllTime, exercice);

            vm.CanDeleteItem = true;
            vm.CanUpdateItem = true;

            // Update fields
            RefData.CurrentWorkout.SetAndNotifyMainProperties();

            await Shell.Current.Navigation.PushAsync(exercicePage);
        }
    }
}
