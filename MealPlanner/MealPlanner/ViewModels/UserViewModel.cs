using MealPlanner.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MealPlanner.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public UserViewModel()
        {
            Title = "User";
            SaveUserDataCommand = new Command<UserPage>(SaveUserData);
        }

        public ICommand SaveUserDataCommand { get; set; }
        private async void SaveUserData(UserPage contentPage)
        {
            if(contentPage.CheckFields())
                await App.DataBaseRepo.UpdateUserAsync(RefData.User);
        }
    }
}
