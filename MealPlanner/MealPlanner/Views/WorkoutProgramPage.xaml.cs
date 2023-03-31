using MealPlanner.Controls;
using MealPlanner.Models;
using MealPlanner.ViewModels;
using Microcharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using ZXing.Common.Detector;
using static MealPlanner.Views.WorkoutProgramPage;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutProgramPage : ContentPage
    {
        public WorkoutProgramPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Met();
        }

        private async void Met()
        {
            //mehe.ItemsSource = null;
            //mehe.ItemsSource = (BindingContext as WorkoutProgramViewModel).CurrentWorkoutProgram.WorkoutWeeks;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            (BindingContext as WorkoutProgramViewModel).CurrentWorkoutProgram.WorkoutWeeks.Add(new WorkoutWeek() { Name = "Week99" });
        }
    }
}