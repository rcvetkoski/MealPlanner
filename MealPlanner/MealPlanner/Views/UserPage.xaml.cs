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
            bool validation = false;
            var lol = ((nameEntry.Behaviors[0] as ValidationBehaviour).Validators[0] as IValidation).Validate("");
            //if (!string.IsNullOrEmpty(nameEntry.Text) && )

            //this.nameEntry.Focus(); 

            return validation;
        }
    }
}