﻿using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class ExerciceGroupViewModel : BaseViewModel
    {
        public ExerciceGroupViewModel()
        {
            Title = "Exercices";
            SelectExerciceGroupCommand = new Command<MuscleGroup>(SelectExerciceGroup);
            CreateNewExerciceGroupCommand = new Command(CreateNewExerciceGroup);
            SearchExerciceCommand = new Command(SearchExercice);
            SelectExerciceFromProgramCommand = new Command(SelectExerciceFromProgram);
        }

        public ICommand SelectExerciceGroupCommand { get; set; }
        private async void SelectExerciceGroup(MuscleGroup muscleGroup)
        {
            AddExercicePage addExercicePage = new AddExercicePage();
            var vm = addExercicePage.BindingContext as AddExerciceViewModel;
            vm.CurrentWorkout = CurrentWorkout;
            vm.RefreshFilteredExercices(muscleGroup.Name);
            await Shell.Current.Navigation.PushAsync(addExercicePage);
        }

        public ICommand CreateNewExerciceGroupCommand { get; set; }
        private async void CreateNewExerciceGroup()
        {
            EditExercicePage editExercicePage = new EditExercicePage();
            var vm = editExercicePage.BindingContext as EditExerciceViewModel;
            vm.CurrentExercice = new Exercice();
            vm.IsNew = true;

            await Shell.Current.Navigation.PushAsync(editExercicePage);
        }

        public ICommand SearchExerciceCommand { get; set; }
        private async void SearchExercice()
        {
            AddExercicePage addExercicePage = new AddExercicePage();
            var vm = addExercicePage.BindingContext as AddExerciceViewModel;
            vm.CurrentWorkout = CurrentWorkout;
            vm.IsSearchVisible = true;
            vm.RefreshFilteredExercices(string.Empty);
            await Shell.Current.Navigation.PushAsync(addExercicePage);
            addExercicePage.SearchEntry.Focus();
        }

        public ICommand SelectExerciceFromProgramCommand { get; set; }
        private async void SelectExerciceFromProgram()
        {
            await Shell.Current.GoToAsync(nameof(WorkoutProgramsPage));
        }
    }
}
