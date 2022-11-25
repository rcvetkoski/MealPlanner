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
    public partial class JournalTemplatePage : ContentPage
    {
        public JournalTemplatePage()
        {
            InitializeComponent();
        }

        private async void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var vm = (BindingContext as JournalTemplateViewModel);

            if (!e.Value)
            {
                vm.RefData.User.AutoGenerateJournalEnabled = false;
                await App.DataBaseRepo.UpdateUserAsync(vm.RefData.User);
                return;
            }
            // Don't do anything because this is the default setter for the switch when it is initialised
            if (vm.RefData.User.AutoGenerateJournalEnabled && e.Value)
                return;

            bool result = await DisplayAlert("", "Auto generate will be activated.\r\nThis will delette all existing logs startig from today !!!", "ok", "cancel");

            if(result)
            {
                vm.AutoGenerateJournal();
            }
            else
            {
                (sender as Switch).IsToggled = false;
            }
        }

        private async void RSPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var vm = (BindingContext as JournalTemplateViewModel);

            // When picker is first initialized
            // Ignore in this case
            if (!autoGenerateJournalSwitch.IsToggled && vm.RefData.User.AutoGenerateJournalEnabled)
                return;

            vm.RefData.User.CurrentJournalTemplateId = vm.RefData.CurrentJournalTemplate != null ? vm.RefData.CurrentJournalTemplate.Id : -1;
            vm.RefData.User.AutoGenerateJournalEnabled = false;
            await App.DataBaseRepo.UpdateUserAsync(vm.RefData.User);
            this.autoGenerateJournalSwitch.IsToggled = false;
        }
    }
}