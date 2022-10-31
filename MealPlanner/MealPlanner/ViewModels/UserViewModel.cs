using MealPlanner.Models;
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
            Title = RefData.User != null ? RefData.User.Name : "User";
            SaveUserDataCommand = new Command<UserPage>(SaveUserData);
        }

        public ICommand SaveUserDataCommand { get; set; }
        private async void SaveUserData(UserPage contentPage)
        {
            if(contentPage.CheckFields())
            {
                User currentUser = await App.DataBaseRepo.GetUserAsync();

                if (currentUser == null)
                {
                    await App.DataBaseRepo.AddUserAsync(RefData.User);
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
                else
                {
                    await App.DataBaseRepo.UpdateUserAsync(RefData.User);
                    await Shell.Current.Navigation.PopAsync();
                }
            }
        }
    }
}
