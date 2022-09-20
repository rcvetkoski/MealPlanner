using MealPlanner.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MealPlanner.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}