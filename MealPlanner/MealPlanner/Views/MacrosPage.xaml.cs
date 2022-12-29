using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using SkiaSharp.Views.Forms;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MacrosPage : ContentPage
    {
        public MacrosPage()
        {
            InitializeComponent();

            var entries = new[]
            {
                new ChartEntry(212)
                {
                    Label = "Proteins",
                    ValueLabel = "112",
                    Color = SKColor.Parse("#2c3e50")
                },
                new ChartEntry(248)
                {
                    Label = "Carbs",
                    ValueLabel = "648",
                    Color = SKColor.Parse("#77d065")
                },
                new ChartEntry(128)
                {
                    Label = "Fats",
                    ValueLabel = "428",
                    Color = SKColor.Parse("#b455b6")
                }
            };

            var chart = new PieChart()
            {
                Entries = entries,
                BackgroundColor = Color.Transparent.ToSKColor(),
                LabelMode = LabelMode.RightOnly,
                LabelTextSize = 36

            };
            chartView.Chart = chart;
        }
    }
}