using IntegrantFirstTask.Helpers;
using IntegrantFirstTask.Interfaces;
using IntegrantFirstTask.Models;
using IntegrantFirstTask.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IntegrantFirstTask.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        private NetworkAccess _ConnectionStatus = Connectivity.NetworkAccess;
        public NetworkAccess ConnectionStatus
        {
            get { return Connectivity.NetworkAccess; }
            set { SetValue(ref _ConnectionStatus, value); }
        }

        private IEnumerable<ConnectionProfile> _ConnectionProfiles = Connectivity.ConnectionProfiles;
        public IEnumerable<ConnectionProfile> ConnectionProfiles
        {
            get { return Connectivity.ConnectionProfiles; }
            set { SetValue(ref _ConnectionProfiles, value); }
        }

        private bool _IsLoading = false;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { SetValue(ref _IsLoading, value); }
        }

        public bool InternetConnected
        {
            get { return ConnectionStatus == NetworkAccess.Internet; }
        }

        public bool InternetNotConnected
        {
            get { return !(ConnectionStatus == NetworkAccess.Internet); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand NavigatetoHomeCommand { get; set; }
        public ICommand NavigateToFilterCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand NavigateToShoppingCartCommand { get; set; }

        public User SharedUser
        {
            get { return (User)Application.Current.Properties["User"]; }
        }

        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set
            {
                SetValue<ObservableCollection<Item>>(ref _items, value);
            }
        }

        public BaseViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            NavigateToFilterCommand = new Command(() => { NavigationHelper.NavigateToModelAsync(new FiltersPage()); });
            NavigatetoHomeCommand = new Command(() => { NavigationHelper.NavigateToPageAsync(new HomePage()); });
            LogoutCommand = new Command(() => { NavigationHelper.NavigateToRootAsync(); });
            NavigateToShoppingCartCommand = new Command(() => { NavigationHelper.NavigateToPageAsync(new ShoppingCartPage()); });
        }
        public void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        protected void SetValue<T>(ref T OldValue, T NewValue, [CallerMemberName] string PropertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(OldValue, NewValue))
                return;
            OldValue = NewValue;
            OnPropertyChanged(PropertyName);
        }
        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            ConnectionStatus = e.NetworkAccess;
            ConnectionProfiles = e.ConnectionProfiles;
        }
    }
}
