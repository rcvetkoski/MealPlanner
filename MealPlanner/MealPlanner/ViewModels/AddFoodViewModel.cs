using System;
using System.Collections.Generic;
using System.Text;

namespace MealPlanner.ViewModels
{
    public class AddFoodViewModel : BaseViewModel
    {
        public AddFoodViewModel()
        {
            Title = "AddFood";
            IsFoodChecked = true;
        }

        private bool isFoodChecked;
        public bool IsFoodChecked { get { return isFoodChecked; } set { isFoodChecked = value; OnPropertyChanged("IsFoodChecked"); } }


        private bool isMealChecked;
        public bool IsMealChecked { get { return isMealChecked; } set { isMealChecked = value; OnPropertyChanged("IsMealChecked"); } }


        public bool isLibraryChecked;
        public bool IsLibraryChecked { get { return isLibraryChecked; } set { isLibraryChecked = value; OnPropertyChanged("IsLibraryChecked"); } }
    }
}
