using MealPlanner.Helpers.Enums;
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
    public class WorkoutJournalViewModel : BaseViewModel
    {
        public WorkoutJournalViewModel()
        {
            SetTitle();
            MaximumDate = RefData.CurrentDay.AddDays(7);
            AddExerciceCommand = new Command(AddExercice);
            UpdateExerciceCommand = new Command<Exercice>(UpdateExercice);
            OpenCalendarCommand = new Command<DatePicker>(OpenCalendar);
            PreviousDayCommand = new Command(PreviousDay);
            NextDayCommand = new Command(NextDay);
            ResetCurrentDayCommand = new Command(ResetCurrentDay);
        }

        public void SetTitle()
        {
            Title = RefData.CurrentDay.Day == DateTime.Now.Day ? "Today" : RefData.CurrentDay.ToString(("dd MMM"));
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        public DateTime MaximumDate { get; set; }
        public bool NextDayCommandVisible
        {
            get
            {
                return MaximumDate > RefData.CurrentDay.AddDays(1);
            }
        }

        public ICommand OpenCalendarCommand { get; set; }
        private void OpenCalendar(DatePicker datePicker)
        {
            datePicker.Focus();
        }

        public ICommand PreviousDayCommand { get; set; }
        private void PreviousDay()
        {
            RefData.CurrentDay = RefData.CurrentDay.Subtract(TimeSpan.FromDays(1));
            SetTitle();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        public ICommand NextDayCommand { get; set; }
        private void NextDay()
        {
            RefData.CurrentDay = RefData.CurrentDay.AddDays(1);
            SetTitle();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        public ICommand ResetCurrentDayCommand { get; set; }
        private void ResetCurrentDay()
        {
            RefData.CurrentDay = DateTime.Now;
            SetTitle();
            OnPropertyChanged(nameof(NextDayCommandVisible));
        }

        public ICommand AddExerciceCommand { get; set; }
        private async void AddExercice()
        {
            //AddExercicePage addExercicePage = new AddExercicePage();
            //var vm = addExercicePage.BindingContext as AddExerciceViewModel;
            //vm.SelectedWorkout = RefData.CurrentWorkout;

            await Shell.Current.GoToAsync(nameof(ExerciceGroupPage));
        }

        public ICommand UpdateExerciceCommand { get; set; }
        private async void UpdateExercice(Exercice exercice)
        {
            ExercicePage exercicePage = new ExercicePage();
            var vm = exercicePage.BindingContext as ExerciceViewModel;
            vm.CurrentExercice = exercice;
            foreach (Set set in exercice.Sets)
            {
                vm.CopiedSets.Add(set);
            }

            // Fill Previous sets if any
            vm.LastTimePerformance = RefData.GetExerciceHistoryLastTime(exercice);
            vm.CurrentExercice.History = RefData.GetExerciceHistory((int)PeriodEnum.AllTime, exercice);

            vm.CanDeleteItem = true;
            vm.CanUpdateItem = true;

            await Shell.Current.Navigation.PushAsync(exercicePage);
        }
    }
}
