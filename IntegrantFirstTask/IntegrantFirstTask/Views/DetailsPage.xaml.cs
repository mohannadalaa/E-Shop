using IntegrantFirstTask.Helpers;
using IntegrantFirstTask.Models;
using IntegrantFirstTask.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IntegrantFirstTask.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailsPage : ContentPage
	{
         DetailsPageViewModel VM { get; set; }
        ShoppingCartViewModel SCVM { get; set; }
        public Item Item { get; set; }
        public DetailsPage (Item _item)
		{
			InitializeComponent ();
            Item = _item;
            VM = new DetailsPageViewModel(_item);
            BindingContext = VM;
		}

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            VM.StepperValue = e.NewValue;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            VM.IsLoading = true;
           await NavigationHelper.NavigateToPageAsync(new HomePage());
            Item.Count = VM.StepperValue;
            VM.IsLoading = false;
            VM.AddToLocalStore(Item);
        }
    }
}