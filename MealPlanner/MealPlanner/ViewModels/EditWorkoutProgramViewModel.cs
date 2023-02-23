using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class EditWorkoutProgramViewModel : BaseViewModel
    {
        public EditWorkoutProgramViewModel()
        {
            EditWorkoutProgramCommand = new Command<WorkoutProgram>(editWorkoutProgramCommand);
            CreateNewWorkoutWeekRoutineCommand = new Command(CreateNewWorkoutWeekRoutine);
            SelectWorkoutRoutineCommand = new Command<Workout>(SelectWorkoutRoutine);
            AddNewWeekCommand = new Command<WorkoutProgram>(AddNewWeek);
            DeleteRoutineCommand = new Command<Workout>(DeleteRoutine);
        }

        public ICommand EditWorkoutProgramCommand { get; set; }
        private void editWorkoutProgramCommand(WorkoutProgram workoutProgram)
        {

        }

        public ICommand CreateNewWorkoutWeekRoutineCommand { get; set; }
        private async void CreateNewWorkoutWeekRoutine()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("New routine", null, "Add", "Cancel", "Name");
            if (string.IsNullOrEmpty(result))
                return;

            // Create routine
            Workout workoutRoutine = new Workout()
            {
                Name = result
            };

            // Add workoutWeekRoutine to db
            await App.DataBaseRepo.AddWorkoutAsync(workoutRoutine);

            // Create new link WorkoutWeekRoutine / Week 
            WorkoutWeekRoutine workoutWeekRoutine = new WorkoutWeekRoutine
            {
                WorkoutWeekId = CurrentWorkoutProgram.SelectedWorkoutWeek.Id,
                WorkoutId = workoutRoutine.Id
            };

            // Add workoutRoutine to db
            await App.DataBaseRepo.AddWorkoutWeekRoutineAsync(workoutWeekRoutine);

            // Add workoutProgramRoutine to collection
            RefData.WorkoutWeekRoutines.Add(workoutWeekRoutine);

            // Add workoutRoutine to collection
            CurrentWorkoutProgram.SelectedWorkoutWeek.Workouts.Add(workoutRoutine);
        }

        public ICommand SelectWorkoutRoutineCommand { get; set; }
        private async void SelectWorkoutRoutine(Workout workoutRoutine)
        {
            WorkoutRoutinePage workoutRoutinePage = new WorkoutRoutinePage();
            var vm = workoutRoutinePage.BindingContext as WorkoutRoutineViewModel;
            vm.CurrentWorkout = workoutRoutine;
            vm.SelectedExerciceCounter = $"Add - {vm.SelectedExercices.Count}";

            await Shell.Current.Navigation.PushAsync(workoutRoutinePage);
        }

        public ICommand AddNewWeekCommand { get; set; }
        private async void AddNewWeek(WorkoutProgram workoutProgram)
        {
            WorkoutWeek workoutWeek = new WorkoutWeek()
            {
                Name = $"WEEK {workoutProgram.WorkoutWeeks.Count + 1}"
            };

            // Add week to program
            workoutProgram.WorkoutWeeks.Add(workoutWeek);

            // Add weeek to main collection
            RefData.WorkoutWeeks.Add(workoutWeek);

            // Add week in db
            await App.DataBaseRepo.AddWorkoutWeekAsync(workoutWeek);

            // Create link 
            WorkoutWeekProgram workoutWeekProgram = new WorkoutWeekProgram()
            {
                WorkoutProgramId = workoutProgram.Id,
                WorkoutWeekId = workoutWeek.Id
            };

            // Add workoutWeekProgram to collection
            RefData.WorkoutWeekPrograms.Add(workoutWeekProgram);

            // add workoutWeekProgram to db
            await App.DataBaseRepo.AddWorkoutWeekProgramAsync(workoutWeekProgram);

            // Set newly created week as SelectedWorkoutWeek
            workoutProgram.SelectedWorkoutWeek = workoutWeek;
        }

        public ICommand DeleteRoutineCommand { get; set; }
        private async void DeleteRoutine(Workout workoutRoutine)
        {
            var workoutWeekRoutine = RefData.WorkoutWeekRoutines.SingleOrDefault(x => x.WorkoutId == workoutRoutine.Id && x.WorkoutWeekId == CurrentWorkoutProgram.SelectedWorkoutWeek.Id);

            if (workoutWeekRoutine == null)
                return;

            // Add workoutRoutine to db
            await App.DataBaseRepo.DeleteWorkoutWeekRoutineAsync(workoutWeekRoutine);

            // Add workoutProgramRoutine to collection
            RefData.WorkoutWeekRoutines.Remove(workoutWeekRoutine);

            // Remove workoutRoutine to collection
            CurrentWorkoutProgram.SelectedWorkoutWeek.Workouts.Remove(workoutRoutine);

            foreach(Exercice exercice in workoutRoutine.Exercices)
            {
                var workoutExercice = RefData.WorkoutExercices.FirstOrDefault(x => x.Id == exercice.WorkoutExerciceId);

                if (workoutExercice == null)
                    continue;

                // Remove sets
                foreach (Set set in exercice.Sets)
                {
                    RefData.Sets.Remove(set);
                    await App.DataBaseRepo.DeleteSetAsync(set);
                }

                // Remove from collection
                RefData.WorkoutExercices.Remove(workoutExercice);

                // Delete fro mdb
                await App.DataBaseRepo.DeleteWorkoutExerciceAsync(workoutExercice);
            }

        }
    }
}
