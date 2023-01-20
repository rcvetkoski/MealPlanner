using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class EditExerciceViewModel : BaseViewModel
    {
        public EditExerciceViewModel()
        {
            SaveOrEditExerciceCommand = new Command(SaveOrEditExercice);
            AddSetCommand = new Command(AddSet);
        }

        public ICommand SaveOrEditExerciceCommand { get; set; }
        private async void SaveOrEditExercice()
        {
            if (IsNew)
                SaveExercice();
            else
                EditExercice();
        }

        private async void SaveExercice()
        {
            RefData.Exercices.Add(CurrentExercice);
            await App.DataBaseRepo.AddExerciceAsync(CurrentExercice);
            await Shell.Current.Navigation.PopAsync();
        }

        private async void EditExercice()
        {
            bool isOriginalExercice = RefData.Exercices.Contains(CurrentExercice);

            // Update Workout exercices in this case
            if(isOriginalExercice)
                RefData.GetWorkoutAtDay(RefData.CurrentDay);
            else
            {
                // Updateoriginal exercice in ReFData.Exercices
                var exerciceFromWorkout = RefData.Exercices.FirstOrDefault(x => x.Id == CurrentExercice.Id);
                exerciceFromWorkout.Name = CurrentExercice.Name;
                exerciceFromWorkout.ImageSourcePath = CurrentExercice.ImageSourcePath;
                exerciceFromWorkout.ImageBlob = CurrentExercice.ImageBlob;
                exerciceFromWorkout.Description = CurrentExercice.Description;
                exerciceFromWorkout.MuscleGroup = CurrentExercice.MuscleGroup;
                exerciceFromWorkout.MuscleGroupId = CurrentExercice.MuscleGroupId;
            }

            // Save Changes to db
            await App.DataBaseRepo.UpdateExerciceAsync(CurrentExercice);
        }

        public ICommand AddSetCommand { get; set; }
        private async void AddSet()
        {

        }
    }
}
