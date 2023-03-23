using MealPlanner.Controls;
using Microcharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}