using IntegrantFirstTask.Helpers;
using IntegrantFirstTask.Models;
using IntegrantFirstTask.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IntegrantFirstTask.ViewModels
{
    public class FilerPageViewModel : BaseViewModel
    {
        public ICommand ApplyFilterCommand { get; set; }

        public FilerPageViewModel()
        {
            ApplyFilterCommand = new Command(ApplyFilterButtonClicked);
        }

        private double _MinPrice;
        public double MinPrice
        {
            get { return _MinPrice; }
            set { SetValue(ref _MinPrice, value); }
        }

        private double _MaxPrice;
        public double MaxPrice
        {
            get { return _MaxPrice; }
            set { SetValue(ref _MaxPrice, value); }
        }

        private async void ApplyFilterButtonClicked()
        {
            HomePage Home = new HomePage(MinPrice , MaxPrice);
           await NavigationHelper.NavigateToPageAsync(Home);
             Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
