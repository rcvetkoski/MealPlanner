using MealPlanner.Models;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
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
            ClearSearchCommand = new Command<Exercice>(ClearSearch);
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
            vm.CanAddItem = true;
            vm.CanDeleteItem = true;

            await Shell.Current.Navigation.PushAsync(exercicePage);
        }

        public ICommand ClearSearchCommand { get; set; }
        private async void ClearSearch(Exercice exercice)
        {

            //await Shell.Current.Navigation.PushAsync(exercicePage);
        }
    }
}
