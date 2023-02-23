using MealPlanner.Helpers;
using MealPlanner.Helpers.Enums;
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
using Xamarin.Forms.PlatformConfiguration;

namespace MealPlanner.ViewModels
{
    public class WorkoutRoutineViewModel : BaseViewModel
    {
        public WorkoutRoutineViewModel()
        {
            AddExerciceCommand = new Command(AddExercice);
            UpdateExerciceCommand = new Command<Exercice>(UpdateExercice);
            EditWorkoutRoutineCommand = new Command<Workout>(EditWorkoutRoutine);
            SelectionChangedCommand = new Command<CollectionView>(SelectionChanged);
            AddExercicesFromRoutineCommand = new Command<Button>(AddExercicesFromRoutine); 
            SelectedExercices = new List<object>();
        }

        public List<object> SelectedExercices { get; set; }

        private string selectedExerciceCounter;
        public string SelectedExerciceCounter 
        { 
            get
            {
                return selectedExerciceCounter;
            }
            set
            {
                if(selectedExerciceCounter != value)
                {
                    selectedExerciceCounter = value;
                    OnPropertyChanged(nameof(SelectedExerciceCounter));
                }
            }
        } 

        public ICommand AddExerciceCommand { get; set; }
        private async void AddExercice()
        {
            ExerciceGroupPage exerciceGroupPage = new ExerciceGroupPage();
            var vm = exerciceGroupPage.BindingContext as ExerciceGroupViewModel;
            vm.CurrentWorkout = CurrentWorkout;

            await Shell.Current.Navigation.PushAsync(exerciceGroupPage);
        }

        public ICommand UpdateExerciceCommand { get; set; }
        private async void UpdateExercice(Exercice exercice)
        {
            ExercicePage exercicePage = new ExercicePage();
            var vm = exercicePage.BindingContext as ExerciceViewModel;
            vm.CurrentWorkout = CurrentWorkout;
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

        public ICommand EditWorkoutRoutineCommand { get; set; }
        private async void EditWorkoutRoutine(Workout workoutRoutine)
        {

        }

        public ICommand SelectionChangedCommand { get; set; }
        private void SelectionChanged(CollectionView collectionView)
        {
            SelectedExerciceCounter = $"Add - {SelectedExercices.Count}";
        }

        public ICommand AddExercicesFromRoutineCommand { get; set; }
        private async void AddExercicesFromRoutine(Button button)
        {

            //App.StatusBarColor.SetStatusBarColor(Color.FromHex("#545456"), false);
            //await Shell.Current.GoToAsync(nameof(TestPage));
            await Shell.Current.Navigation.PushModalAsync(new TestPage(button.Bounds));
            //await Shell.Current.GoToAsync("../../../..");
        }
    }
}
