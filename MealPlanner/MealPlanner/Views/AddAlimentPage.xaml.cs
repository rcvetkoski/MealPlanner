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
                foreach (ToolbarItem item in TempToolbarItems)
                    ToolbarItems.Add(item);

                title.IsVisible = true;
                entry.IsVisible = false;
                entry.TranslationX = entry.Width;
            }
            else
                await Shell.Current.GoToAsync("..", true);
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                (BindingContext as AddAlimentViewModel).FilteredAlimentsRefresh();

                if(!this.ToolbarItems.Any())
                {
                    foreach (ToolbarItem item in TempToolbarItems)
                        ToolbarItems.Add(item);

                    title.IsVisible = true;
                    entry.IsVisible = false;
                    entry.TranslationX = entry.Width;
                }
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            this.ToolbarItems.Clear();
            title.IsVisible = false;
            entry.IsVisible = true;

            entry.TranslateTo(0, 0);
            entry.Focus();
        }
    }
}