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
            SelectExerciceGroupCommand = new Command(SelectExerciceGroup);
            CreateNewExerciceGroupCommand = new Command(CreateNewExerciceGroup);
            SearchExerciceCommand = new Command(SearchExercice);
        }

        public ICommand SelectExerciceGroupCommand { get; set; }
        private async void SelectExerciceGroup()
        {
            await Shell.Current.GoToAsync(nameof(AddExercicePage));
        }

        public ICommand CreateNewExerciceGroupCommand { get; set; }
        private async void CreateNewExerciceGroup()
        {
            //await Shell.Current.GoToAsync(nameof(AddExercicePage));
        }

        public ICommand SearchExerciceCommand { get; set; }
        private async void SearchExercice()
        {
            AddExercicePage addExercicePage = new AddExercicePage();

            //await Shell.Current.Navigation.PushModalAsync(addExercicePage);
            await Shell.Current.GoToAsync(nameof(AddExercicePage));
        }
    }
}
