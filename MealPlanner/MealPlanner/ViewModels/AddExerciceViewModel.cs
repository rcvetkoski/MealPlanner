using MealPlanner.Helpers;
using MealPlanner.Helpers.Extensions;
using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class AddExerciceViewModel : BaseViewModel
    {
        public AddExerciceViewModel()
        {
            Title = "Add Exercice";
            CreateNewExerciceCommand = new Command(CreateNewExercice);
            SelectExerciceCommand = new Command<Exercice>(SelectExercice);
            ClearSearchCommand = new Command(ClearSearch);
            SearchCommand = new Command<Entry>(Search);
            FilteredExercices = new ObservableCollection<Exercice>();
            TempFilteredExercices = new List<Exercice>();
        }

        private bool isSearchVisible;
        public bool IsSearchVisible
        {
            get
            {
                return isSearchVisible;
            }
            set
            {
                if(isSearchVisible != value)
                {
                    isSearchVisible = value;
                    OnPropertyChanged(nameof(IsSearchVisible));
                }
            }
        }

        private string query;
        public string Query
        {
            get
            {
                return query;
            }
            set
            {
                if (value != query)
                {
                    query = value;
                    OnPropertyChanged(nameof(Query));
                }
            }
        }

        public string MuscleGroupName = string.Empty;

        private ObservableCollection<Exercice> filteredExercices;
        public ObservableCollection<Exercice> FilteredExercices 
        { 
            get
            {
                return filteredExercices;
            }
            set
            {
                if(filteredExercices != value)
                {
                    filteredExercices = value;
                    OnPropertyChanged(nameof(FilteredExercices));
                }
            }
        }
        private List<Exercice> TempFilteredExercices;

        public void RefreshFilteredExercices(string MuscleGroupName)
        {
            List<Exercice> sortedList = null;

            if (string.IsNullOrEmpty(MuscleGroupName))
                sortedList = RefData.Exercices.ToList();
            else
                sortedList = RefData.Exercices.Where(x => x.MuscleGroup?.Name == MuscleGroupName).ToList();

            FillFilteredExercices(sortedList);
            TempFilteredExercices = FilteredExercices.ToList();
        }

        public void SearchExercices()
        {
            var searchedList = TempFilteredExercices.Where(x => !x.Archived && x.Name.ToLower().Contains(Query.ToLower())).ToList();
            FillFilteredExercices(searchedList);
        }

        private void FillFilteredExercices(List<Exercice> sortedList)
        {
            FilteredExercices.Clear();

            FilteredExercices = sortedList.ToObservableCollection();

            //foreach (Exercice exercice in sortedList)
            //    FilteredExercices.Add(exercice);
        }

        public ICommand CreateNewExerciceCommand { get; set; }
        private async void CreateNewExercice()
        {
            EditExercicePage editExercicePage = new EditExercicePage();
            var vm = editExercicePage.BindingContext as EditExerciceViewModel;
            vm.CurrentExercice = new Exercice();
            vm.IsNew = true;

            await Shell.Current.Navigation.PushAsync(editExercicePage);
        }

        public ICommand SelectExerciceCommand { get; set; }
        private async void SelectExercice(Exercice exercice)
        {
            ExercicePage exercicePage = new ExercicePage();
            var vm = exercicePage.BindingContext as ExerciceViewModel;
            vm.CurrentExercice = exercice;
            foreach(Set set in exercice.Sets)
            {
                vm.CopiedSets.Add(set);
            }

            // Fill Previous sets if any
            foreach (Log log in RefData.Logs.Where(x=> x.Date.Date != DateTime.Now.Date).OrderByDescending(x => x.Date))
            {
                var workoutExercice = RefData.WorkoutExercices.SingleOrDefault(x => x.WorkoutId == log.WorkoutId && x.ExerciceId == exercice.Id);

                if (workoutExercice == null)
                    continue;
                else
                {
                    foreach (Set set in RefData.Sets.Where(x => x.WorkoutExerciceId == workoutExercice.Id))
                        vm.PreviousSets.Add(set);

                    break;
                }
            }


            vm.CanAddItem = true;
            vm.CanDeleteItem = true;

            await Shell.Current.Navigation.PushAsync(exercicePage);
        }

        public ICommand ClearSearchCommand { get; set; }
        private void ClearSearch()
        {
            IsSearchVisible = false;
            if(!string.IsNullOrEmpty(Query))
            {
                Query = string.Empty;
                SearchExercices();
            }
        }

        public ICommand SearchCommand { get; set; }
        private async void Search(Entry entry)
        {
            entry.Focus();
            IsSearchVisible = true;
        }
    }
}
