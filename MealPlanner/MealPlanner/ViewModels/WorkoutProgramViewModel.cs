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
            SelectWorkoutRoutineCommand = new Command<Workout>(SelectWorkoutRoutine);
        }

        public ICommand CreateNewWorkoutRoutineCommand { get; set; }
        private async void CreateNewWorkoutRoutine()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("New routine", null, "Add", "Cancel", "Name");
            if (string.IsNullOrEmpty(result))
                return;

            // Create routine
            Workout workoutRoutine = new Workout()
            {
                Name = result
            };

            // Add workoutRoutine to db
            await App.DataBaseRepo.AddWorkoutAsync(workoutRoutine);

            // Create new ling WorkoutRoutine / Program 
            WorkoutProgramRoutine workoutProgramRoutine = new WorkoutProgramRoutine
            {
                WorkoutProgramId = CurrentWorkoutProgram.Id,
                WorkoutId = workoutRoutine.Id
            };

            // Add workoutProgramRoutine to db
            await App.DataBaseRepo.AddWorkoutProgramRoutineAsync(workoutProgramRoutine);

            // Add workoutProgramRoutine to collection
            RefData.WorkoutProgramRoutines.Add(workoutProgramRoutine);

            // Add workoutRoutine to collection
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
        private async void SelectWorkoutRoutine(Workout workoutRoutine)
        {
            WorkoutRoutinePage workoutRoutinePage = new WorkoutRoutinePage();
            var vm = workoutRoutinePage.BindingContext as WorkoutRoutineViewModel;
            vm.CurrentWorkout = workoutRoutine;

            await Shell.Current.Navigation.PushAsync(workoutRoutinePage);
        }
    }
}
