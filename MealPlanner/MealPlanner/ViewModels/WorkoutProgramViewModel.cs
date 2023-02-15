using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
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
            CreateNewWorkoutRoutineCommand = new Command(CreateNewWorkoutRoutine);
            EditWorkoutProgramCommand = new Command<WorkoutProgram>(EditWorkoutProgram);
            SelectWorkoutRoutineCommand = new Command<WorkoutRoutine>(SelectWorkoutRoutine);
        }

        public ICommand CreateNewWorkoutRoutineCommand { get; set; }
        private async void CreateNewWorkoutRoutine()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("New routine", null, "Add", "Cancel", "Name");
            if (string.IsNullOrEmpty(result))
                return;

            WorkoutRoutine workoutRoutine = new WorkoutRoutine()
            {
                Name = result
            };

            CurrentWorkoutProgram.WorkoutRoutines.Add(workoutRoutine);
        }

        public ICommand EditWorkoutProgramCommand { get; set; }
        private async void EditWorkoutProgram(WorkoutProgram workoutProgram)
        {
            EditWorkoutProgramPage editWorkoutProgramPage = new EditWorkoutProgramPage();
            var vm = editWorkoutProgramPage.BindingContext as EditWorkoutProgramViewModel;
            vm.CurrentWorkoutProgram = workoutProgram;

            await Shell.Current.Navigation.PushAsync(editWorkoutProgramPage);
        }

        public ICommand SelectWorkoutRoutineCommand { get; set; }
        private async void SelectWorkoutRoutine(WorkoutRoutine workoutRoutine)
        {
            WorkoutRoutinePage workoutRoutinePage = new WorkoutRoutinePage();
            var vm = workoutRoutinePage.BindingContext as WorkoutRoutineViewModel;
            vm.CurrentWorkoutRoutine = workoutRoutine;

            await Shell.Current.Navigation.PushAsync(workoutRoutinePage);
        }
    }
}
