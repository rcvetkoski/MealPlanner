using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class WorkoutProgramsViewModel : BaseViewModel
    {
        public WorkoutProgramsViewModel()
        {
            Title = "Programs";
            CreateNewWorkoutProgramCommand = new Command(CreateNewWorkoutProgram);
            SelectWorkoutProgramCommand = new Command<WorkoutProgram>(SelectWorkoutProgram);
        }

        public ICommand CreateNewWorkoutProgramCommand { get; set; }
        private async void CreateNewWorkoutProgram()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("New program", null, "Add", "Cancel", "Name");
            if (string.IsNullOrEmpty(result))
                return;

            WorkoutProgram workoutProgram = new WorkoutProgram()
            {
                Name = result
            };


            //EditWorkoutProgramPage editWorkoutProgramPage = new EditWorkoutProgramPage();
            //var vm = editWorkoutProgramPage.BindingContext as EditWorkoutProgramViewModel;
            //vm.CurrentWorkoutProgram = workoutProgram;


            RefData.WorkoutPrograms.Add(workoutProgram);

            //await Shell.Current.Navigation.PushAsync(editWorkoutProgramPage);
        }

        public ICommand SelectWorkoutProgramCommand { get; set; }
        private async void SelectWorkoutProgram(WorkoutProgram workoutProgram)
        {
            WorkoutProgramPage workoutProgramPage = new WorkoutProgramPage();
            var vm = workoutProgramPage.BindingContext as WorkoutProgramViewModel;
            vm.CurrentWorkoutProgram = workoutProgram;

            await Shell.Current.Navigation.PushAsync(workoutProgramPage);
        }
    }
}
