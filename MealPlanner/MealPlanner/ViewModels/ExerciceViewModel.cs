﻿using MealPlanner.Helpers.Enums;
using MealPlanner.Models;
using MealPlanner.Views;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            AddExerciceCommand = new Command(AddExercice);
            DeleteExerciceCommand = new Command(DeleteExercice);
            EditExerciceCommand = new Command(EditExercice);
            AddSetCommand = new Command(AddSet);
            DeleteSetCommand = new Command<Set>(DeleteSet);
            AddPreviousSetCommand = new Command<Set>(AddPreviousSet);
            UpdateExerciceCommand = new Command(UpdateExercice);
            CopiedSets = new ObservableCollection<Set>();
            AddedSets = new List<Set>();
            DeletedSets = new List<Set>();
            PreviousSets = new ObservableCollection<Set>();
        }

        public ObservableCollection<Set> CopiedSets { get; set; }
        public ObservableCollection<Set> PreviousSets { get; set; }
        private List<Set> AddedSets;
        private List<Set> DeletedSets;


        private bool canAddItem;
        public bool CanAddItem
        {
            get
            {
                return canAddItem;
            }
            set
            {
                if (canAddItem != value)
                {
                    canAddItem = value;
                    OnPropertyChanged(nameof(CanAddItem));
                }
            }
        }

        private bool canDeleteItem;
        public bool CanDeleteItem
        {
            get
            {
                return canDeleteItem;
            }
            set
            {
                if (canDeleteItem != value)
                {
                    canDeleteItem = value;
                    OnPropertyChanged(nameof(CanDeleteItem));
                }
            }
        }

        private bool canUpdateItem;
        public bool CanUpdateItem
        {
            get
            {
                return canUpdateItem;
            }
            set
            {
                if (canUpdateItem != value)
                {
                    canUpdateItem = value;
                    OnPropertyChanged(nameof(CanUpdateItem));
                }
            }
        }

        public ICommand AddExerciceCommand { get; set; }
        private async void AddExercice()
        {
            // Create a copy of exercice
            Exercice exercice = RefData.CreateAndCopyExerciceProperties(CurrentExercice);

            // Add sets
            exercice.Sets = CopiedSets;

            // Create WorkoutExercice link
            WorkoutExercice workoutExercice = new WorkoutExercice()
            {
                WorkoutId = RefData.CurrentWorkout.Id,
                ExerciceId = exercice.Id
            };

            await App.DataBaseRepo.AddWorkoutExerciceAsync(workoutExercice);
            exercice.WorkoutExerciceId = workoutExercice.Id;
            RefData.WorkoutExercices.Add(workoutExercice);

            // Save sets to db
            foreach (Set set in exercice.Sets)
            {
                // Set WorkoutExerciceId
                set.WorkoutExerciceId = workoutExercice.Id;

                await App.DataBaseRepo.AddSetAsync(set);
                RefData.Sets.Add(set);
            }

            // Add exercice to Exercies list
            RefData.CurrentWorkout.Exercices.Add(exercice);

            await Shell.Current.Navigation.PopAsync();
        }

        public ICommand DeleteExerciceCommand { get; set; }
        private async void DeleteExercice()
        {

            if (CurrentExercice.WorkoutExerciceId == 0) // if WorkoutExerciceId == 0 it means it's not linked to a workout so we want to delete it from exercices list
                DeleteOriginalExercice();
            else
                DeleteExerciceFromWorkout();
        }
        private async void DeleteExerciceFromWorkout()
        {
            // Delete link betwen Workout and exercice
            var workoutExercice = RefData.WorkoutExercices.FirstOrDefault(x => x.Id == CurrentExercice.WorkoutExerciceId);

            if (workoutExercice == null)
                return;

            // Remove from list
            RefData.CurrentWorkout.Exercices.Remove(CurrentExercice);

            // Remove sets
            foreach (Set set in CurrentExercice.Sets)
            {
                RefData.Sets.Remove(set);
                await App.DataBaseRepo.DeleteSetAsync(set);
            }

            // Delete fro mdb
            await App.DataBaseRepo.DeleteWorkoutExerciceAsync(workoutExercice);

            // Go back
            await Shell.Current.Navigation.PopAsync();
        }
        private async void DeleteOriginalExercice()
        {
            var response = await Shell.Current.CurrentPage.DisplayAlert("Warning !", "The selected exercice will be archived and will no longer be visible in your exrcices list !!!", "Ok", "Cancel");
            if (!response)
                return;

            // Remove from list
            CurrentExercice.Archived = true;
            RefData.Exercices.Remove(CurrentExercice);

            // Go back
            await Shell.Current.Navigation.PopAsync();
        }

        public ICommand UpdateExerciceCommand { get; set; }
        private async void UpdateExercice()
        {
            // Add sets in db
            foreach (Set set in AddedSets)
            {
                WorkoutExercice workoutExercice = RefData.WorkoutExercices.FirstOrDefault(x => x.Id == CurrentExercice.WorkoutExerciceId);

                // Set WorkoutExerciceId
                set.WorkoutExerciceId = workoutExercice.Id;

                await App.DataBaseRepo.AddSetAsync(set);
                RefData.Sets.Add(set);
            }

            // Delete sets
            foreach (Set set in DeletedSets)
            {
                await App.DataBaseRepo.DeleteSetAsync(set);
                RefData.Sets.Add(set);
            }

            AddedSets.Clear();
            DeletedSets.Clear();

            // Update Sets
            foreach (Set set in CopiedSets)
                await App.DataBaseRepo.UpdateSetAsync(set);

            // Set set to object
            CurrentExercice.Sets = CopiedSets;

            await App.DataBaseRepo.UpdateExerciceAsync(CurrentExercice);

            // Go back
            await Shell.Current.Navigation.PopAsync();
        }

        public ICommand EditExerciceCommand { get; set; }
        private async void EditExercice()
        {
            EditExercicePage editExercicePage = new EditExercicePage();
            var vm = editExercicePage.BindingContext as EditExerciceViewModel;
            vm.CurrentExercice = CurrentExercice;
            vm.IsNew = false;

            await Shell.Current.Navigation.PushAsync(editExercicePage);
        }

        public ICommand AddSetCommand { get; set; }
        private void AddSet()
        {
            double weight = 0;
            int reps = 0;
            var lastSet = CopiedSets.LastOrDefault();

            if(lastSet != null)
            {
                weight = lastSet.Weight;    
                reps = lastSet.Reps;    
            }

            Set set = new Set()
            {
                Order = CopiedSets.Count() + 1,
                Weight = weight,
                Reps = reps
            };

            AddedSets.Add(set);
            CopiedSets.Add(set);
        }

        public ICommand DeleteSetCommand { get; set; }
        private void DeleteSet(Set set)
        {
            DeletedSets.Add(set);
            CopiedSets.Remove(set);
        }

        public ICommand AddPreviousSetCommand { get; set; }
        private void AddPreviousSet(Set set)
        {
            Set newSet = new Set()
            {
                Order = set.Order,
                Weight = set.Weight,
                Reps = set.Reps
            };

            AddedSets.Add(newSet);
            CopiedSets.Add(newSet);
        }
    }
}
