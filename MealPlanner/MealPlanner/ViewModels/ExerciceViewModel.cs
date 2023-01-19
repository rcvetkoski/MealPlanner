using MealPlanner.Helpers.Enums;
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
            UpdateExerciceCommand = new Command<Exercice>(UpdateExercice);
            CopiedSets = new ObservableCollection<Set>();
        }

        public ObservableCollection<Set> CopiedSets { get; set; }

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

            // Save sets to db
            foreach(Set set in exercice.Sets)
            {
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
            await Shell.Current.Navigation.PopAsync();
        }

        public ICommand UpdateExerciceCommand { get; set; }
        private async void UpdateExercice(Exercice exercice)
        {

        }

        public ICommand EditExerciceCommand { get; set; }
        private async void EditExercice()
        {
            EditExercicePage editExercicePage = new EditExercicePage();
            var vm = editExercicePage.BindingContext as EditExerciceViewModel;
            vm.CurrentExercice = CurrentExercice;

            await Shell.Current.Navigation.PushAsync(editExercicePage);
        }

        public ICommand AddSetCommand { get; set; }
        private void AddSet()
        {
            Set set = new Set()
            {
                ExerciceId = CurrentExercice.Id,
                Order = CopiedSets.Count() + 1
            };

            CopiedSets.Add(set);
        }

        public ICommand DeleteSetCommand { get; set; }
        private void DeleteSet(Set set)
        {
            CopiedSets.Remove(set);
        }
    }
}
