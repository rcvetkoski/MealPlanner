using MealPlanner.Helpers;
using MealPlanner.Models;
using MealPlanner.ViewModels;
using Microcharts;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExercicePage : ContentPage
    {
        public ExercicePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitMaxWeightChart();
        }

        public void InitMaxWeightChart()
        {
            var vm = BindingContext as ExerciceViewModel;
            var proteinColor = Color.LightGray;
            List<ChartEntry> chartEntries = new List<ChartEntry>();

            foreach (ExerciceHistoryHelper exerciceHistoryHelper in vm.RefData.GetExerciceHistory((int)vm.SelectedPeriod, vm.CurrentExercice))
            {
                double maxWeight = 0;
                double totalWeight = 0;
                double maxWeightTemp = 0;
                double totalWeightTemp = 0;

                foreach (Set set in exerciceHistoryHelper.Sets)
                {
                    //vm.PreviousSets.Add(set);
                    maxWeightTemp = maxWeightTemp < set.Weight ? set.Weight : maxWeightTemp;
                    totalWeightTemp += set.Weight;
                }

                maxWeight = maxWeight < maxWeightTemp ? maxWeightTemp : maxWeight;
                totalWeight = totalWeight < totalWeightTemp ? totalWeightTemp : totalWeight;


                //Label = workoutExercice.Date.ToString("dd/MM"),
                // Max Weight
                ChartEntry chartEntry = new ChartEntry((float)maxWeight)
                {
                    Label = exerciceHistoryHelper.Date.ToString("MMM dd"),
                    ValueLabel = $"{(float)maxWeight} {vm.RefData.User.WeightUnit}",
                    ValueLabelColor = proteinColor.ToSKColor(),
                    TextColor = proteinColor.ToSKColor(),
                    Color = proteinColor.ToSKColor()
                };
                chartEntries.Add(chartEntry);
            }

            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            // Screen density
            var density = mainDisplayInfo.Density;
            var size = (Device.GetNamedSize(NamedSize.Small, typeof(Label), useOldSizes: false)) * density;

            if(chartEntries.Any())
            {
                var chart = new LineChart()
                {
                    Entries = chartEntries,
                    BackgroundColor = Color.Transparent.ToSKColor(),
                    LabelTextSize = (float)size,
                    LabelOrientation = Orientation.Horizontal,
                    LineMode = LineMode.Spline,
                    PointMode = PointMode.Circle,
                    SerieLabelTextSize = (float)size,
                };

                chartViewMaxWeight.Chart = chart;
                chartViewMaxWeight.IsVisible = true;
                chartViewMaxReps.IsVisible = false;
            }
        }

        public void InitMaxRepsChart()
        {
            var vm = BindingContext as ExerciceViewModel;
            var proteinColor = Color.LightGray;
            double maxReps = 0;
            double totalReps = 0;
            List<ChartEntry> chartEntries = new List<ChartEntry>();

            foreach (ExerciceHistoryHelper exerciceHistoryHelper in vm.RefData.GetExerciceHistory((int)vm.SelectedPeriod, vm.CurrentExercice))
            {
                double maxRepsTemp = 0;
                double totalRepsTemp = 0;

                foreach (Set set in exerciceHistoryHelper.Sets)
                {
                    maxRepsTemp = maxRepsTemp < set.Reps ? set.Reps : maxRepsTemp;
                    totalRepsTemp += set.Reps;
                }

                maxReps = maxReps < maxRepsTemp ? maxRepsTemp : maxReps;
                totalReps = totalReps < totalRepsTemp ? totalRepsTemp : totalReps;

                float value = 0;
                string ValueLabel = string.Empty;   
                if (vm.SelectedExerciceStat == Helpers.Enums.ExerciceStatEnum.MaxReps)
                {
                    value = (float)maxReps;
                    ValueLabel = $"{(float)maxReps} Reps";
                }
                else
                {
                    value = (float)totalReps;
                    ValueLabel = $"{(float)totalReps} Reps";
                }

                ChartEntry chartEntry = new ChartEntry(value)
                {
                    Label = exerciceHistoryHelper.Date.ToString("MMM dd"),
                    ValueLabel = ValueLabel,
                    ValueLabelColor = proteinColor.ToSKColor(),
                    TextColor = proteinColor.ToSKColor(),
                    Color = proteinColor.ToSKColor()
                };
                chartEntries.Add(chartEntry);
            }

            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            // Screen density
            var density = mainDisplayInfo.Density;
            var size = (Device.GetNamedSize(NamedSize.Small, typeof(Label), useOldSizes: false)) * density;

            if (chartEntries.Any())
            {
                var chart = new LineChart()
                {
                    Entries = chartEntries,
                    BackgroundColor = Color.Transparent.ToSKColor(),
                    LabelTextSize = (float)size,
                    LabelOrientation = Orientation.Horizontal,
                    LineMode = LineMode.Spline,
                    PointMode = PointMode.Circle,
                    SerieLabelTextSize = (float)size,
                };

                chartViewMaxReps.Chart = chart;
                chartViewMaxReps.IsVisible = true;
                chartViewMaxWeight.IsVisible = false;
            }
        }

        private void RSEnumPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var vm = BindingContext as ExerciceViewModel;
            if (vm.CurrentExercice == null)
                return;

            if (vm.SelectedExerciceStat == Helpers.Enums.ExerciceStatEnum.MaxWeight || vm.SelectedExerciceStat == Helpers.Enums.ExerciceStatEnum.TotalWeight)
                InitMaxWeightChart();
            else
                InitMaxRepsChart();
        }

        private void RSEnumPickerStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            var vm = BindingContext as ExerciceViewModel;

            if (vm.CurrentExercice == null)
                return;

            if(vm.SelectedExerciceStat == Helpers.Enums.ExerciceStatEnum.MaxWeight || vm.SelectedExerciceStat == Helpers.Enums.ExerciceStatEnum.TotalWeight)
                InitMaxWeightChart();
            else
                InitMaxRepsChart();
        }
    }
}