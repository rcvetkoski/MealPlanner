using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RSControls.Validators;

namespace MealPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
        }

        public bool CheckFields()
        {
            return ((nameEntry.CheckIsValid() || heightEntry.CheckIsValid() || weightEntry.CheckIsValid() || ageEntry.CheckIsValid()) == false) ? false : true;
        }
    }
}