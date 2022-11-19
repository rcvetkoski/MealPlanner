using MealPlanner.Models;
using MealPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RSControls.Controls;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAlimentPage : ContentPage
    {
        private List<ToolbarItem> TempToolbarItems;
        public AddAlimentPage()
        {
            InitializeComponent();

            Shell.SetBackButtonBehavior(this, new BackButtonBehavior()
            {
                Command = new Command(BackButtonCommand)
            });

            TempToolbarItems = new List<ToolbarItem>();
            foreach (ToolbarItem item in ToolbarItems)
                TempToolbarItems.Add(item);
        }

        private async void BackButtonCommand()
        {
            if(entry.IsVisible)
            {
                if(!string.IsNullOrEmpty(entry.Text))
                {
                    entry.Text = string.Empty;
                    (BindingContext as AddAlimentViewModel).FilteredAlimentsRefresh();
                }

                entry.FadeTo(0);
                await entry.TranslateTo(entry.Width, 0);

                foreach (ToolbarItem item in TempToolbarItems)
                    ToolbarItems.Add(item);

                title.IsVisible = true;
                entry.IsVisible = false;
                cancelButton.IsVisible = false;
                entry.TranslationX = entry.Width;
            }
            else
                await Shell.Current.GoToAsync("..", true);
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {  
            (BindingContext as AddAlimentViewModel).Search(); 
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            this.ToolbarItems.Clear();
            title.IsVisible = false;
            entry.IsVisible = true;
            cancelButton.IsVisible = true;

            entry.FadeTo(1);
            entry.TranslateTo(0, 0);
            entry.Focus();
        }

        private async void FilterSwitch(object sender, EventArgs e)
        {
            double x = 0;

            if (sender == foodButton)
            {
                x = 0;
                foodButton.FontAttributes = FontAttributes.Bold;
                recipeButton.FontAttributes = FontAttributes.None;
                (BindingContext as AddAlimentViewModel).IsRecipeChecked = false;
                (BindingContext as AddAlimentViewModel).IsFoodChecked = true;
            }
            else
            {
                x = (sender as View).Width;
                foodButton.FontAttributes = FontAttributes.None;
                recipeButton.FontAttributes = FontAttributes.Bold;
                (BindingContext as AddAlimentViewModel).IsFoodChecked = false;
                (BindingContext as AddAlimentViewModel).IsRecipeChecked = true;
            }

            await slider.TranslateTo(x, 0);
        }

        private void CreateNew(object sender, EventArgs e)
        {
            if ((BindingContext as AddAlimentViewModel).IsFoodChecked)
                (BindingContext as AddAlimentViewModel).CreateFoodCommand.Execute(null);
            else
                (BindingContext as AddAlimentViewModel).CreateRecipeCommand.Execute(null);
        }
    }
}