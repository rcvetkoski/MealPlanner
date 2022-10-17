using MealPlanner.ViewModels;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MealPlanner
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(FoodPage), typeof(FoodPage));
            Routing.RegisterRoute(nameof(AddAlimentPage), typeof(AddAlimentPage));
        }
    }
}
