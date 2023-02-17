using MealPlanner.ViewModels;
using MealPlanner.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace MealPlanner
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddExercicePage), typeof(AddExercicePage));
            Routing.RegisterRoute(nameof(EditExercicePage), typeof(EditExercicePage));
            Routing.RegisterRoute(nameof(ExerciceGroupPage), typeof(ExerciceGroupPage));
            Routing.RegisterRoute(nameof(ExerciceSatisticsPage), typeof(ExerciceSatisticsPage)); 
            Routing.RegisterRoute(nameof(WorkoutProgramsPage), typeof(WorkoutProgramsPage));
            Routing.RegisterRoute(nameof(ExercicePage), typeof(ExercicePage));
            Routing.RegisterRoute(nameof(EditFoodPage), typeof(EditFoodPage));
            Routing.RegisterRoute(nameof(FoodPage), typeof(FoodPage));
            Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));
            Routing.RegisterRoute(nameof(AddAlimentPage), typeof(AddAlimentPage));
            Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
            Routing.RegisterRoute(nameof(UnitsPage), typeof(UnitsPage));
            Routing.RegisterRoute(nameof(CustomizeMealsPage), typeof(CustomizeMealsPage));
            Routing.RegisterRoute(nameof(ObjectifsPage), typeof(ObjectifsPage));
            Routing.RegisterRoute(nameof(ActivityLevelPage), typeof(ActivityLevelPage));
            Routing.RegisterRoute(nameof(MacrosPage), typeof(MacrosPage));
            Routing.RegisterRoute(nameof(EditMacrosPage), typeof(EditMacrosPage));
            Routing.RegisterRoute(nameof(EditJournalTemplatePage), typeof(EditJournalTemplatePage));
            Routing.RegisterRoute(nameof(JournalTemplatePage), typeof(JournalTemplatePage));
            Routing.RegisterRoute(nameof(StatisticsPage), typeof(StatisticsPage));
            Routing.RegisterRoute(nameof(TestPage), typeof(TestPage));
        }
    }
}
