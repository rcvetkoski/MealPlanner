using MealPlanner.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Forms.Internals.GIFBitmap;
using Color = Xamarin.Forms.Color;

namespace MealPlanner.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));

            Meals = new ObservableCollection<Meal>();

            Meals.Add(new Meal() { Name = "Cheesy Vraps with eggs and jambon", Calories = 808, Proteins = 56, Carbs = 256, Fats = 45 });

            for (int i = 0; i < 5; i++)
                Meals.Add(new Meal() { Name = "Salmon Roll Ups", Calories = 256, Proteins = 56, Carbs = 256, Fats = 36 });
        }

        public ObservableCollection<Meal> Meals { get; set; }

        public ICommand OpenWebCommand { get; }
    }
}