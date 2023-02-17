using MealPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        Rectangle bounds;
        public TestPage(Rectangle bounds)
        {
            InitializeComponent();

            this.bounds = bounds;
            //(BindingContext as TestViewModel).Rect = new Rectangle(bounds.X, bounds.Y, this.popup.Width, this.popup.Height);

            //for (int i = 0; i < 1000; i++)
            //{
            //    Entry entry = new Entry();
            //    entry.SetBinding(Entry.TextProperty, "TestPropertie");
            //    this.stack.Children.Add(entry);
            //}
        }
    }
}
