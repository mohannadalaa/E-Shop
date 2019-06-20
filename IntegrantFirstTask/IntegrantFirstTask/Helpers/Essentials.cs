using IntegrantFirstTask.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace IntegrantFirstTask.Helpers
{
    public  class Essentials : BaseViewModel
    {
        private NetworkAccess _ConnectionStatus = Connectivity.NetworkAccess;
        public  NetworkAccess ConnectionStatus
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

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
             ConnectionStatus = e.NetworkAccess;
             ConnectionProfiles = e.ConnectionProfiles;
        }
    }
}
