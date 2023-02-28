using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class WorkoutProgramViewModel : BaseViewModel
    {
        public WorkoutProgramViewModel()
        {
            Title = "Program";
            EditWorkoutProgramCommand = new Command<WorkoutProgram>(EditWorkoutProgram);
            SelectWorkoutRoutineCommand = new Command<Workout>(SelectWorkoutRoutine);

            Test = new ObservableCollection<string>();
            Test.Add("Trwol wefl ");
            Test.Add("Trl ");
            Test.Add("Trwol wefl tz ztjt");
            Test.Add("Trwol  ");
            Test.Add("T");
            Test.Add("Trwol ergergerg ");
        }

        public ObservableCollection<string> Test { get; set; }

        public ICommand EditWorkoutProgramCommand { get; set; }
        private async void EditWorkoutProgram(WorkoutProgram workoutProgram)
        {
            EditWorkoutProgramPage editWorkoutProgramPage = new EditWorkoutProgramPage();
            var vm = editWorkoutProgramPage.BindingContext as EditWorkoutProgramViewModel;
            vm.CurrentWorkoutProgram = workoutProgram;

            await Shell.Current.Navigation.PushAsync(editWorkoutProgramPage);
        }

        public ICommand SelectWorkoutRoutineCommand { get; set; }
        private async void SelectWorkoutRoutine(Workout workoutRoutine)
        {
            WorkoutRoutinePage workoutRoutinePage = new WorkoutRoutinePage();
            var vm = workoutRoutinePage.BindingContext as WorkoutRoutineViewModel;
            vm.CurrentWorkout = workoutRoutine;
            //foreach (Exercice exercice in workoutRoutine.Exercices)
            //    vm.SelectedExercices.Add(exercice);

            vm.SelectedExerciceCounter = $"Add - {vm.SelectedExercices.Count}";


            await Shell.Current.Navigation.PushAsync(workoutRoutinePage);
        }
    }
}
