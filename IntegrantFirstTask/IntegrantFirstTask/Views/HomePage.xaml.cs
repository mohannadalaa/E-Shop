using IntegrantFirstTask.Helpers;
using IntegrantFirstTask.Models;
using IntegrantFirstTask.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IntegrantFirstTask.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        public HomePageViewModel VM { get; set; }
        public HomePage (double MinValue = 0 , double MaxValue = 0)
		{
			InitializeComponent ();
            VM = new HomePageViewModel(MinValue,MaxValue);
            BindingContext = VM;
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
           await VM.GetAllItemsFromLocalDBAsync();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Item;
            NavigationHelper.NavigateToPageAsync(new DetailsPage(item));
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}