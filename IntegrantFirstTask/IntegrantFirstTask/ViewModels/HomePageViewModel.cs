using IntegrantFirstTask.Clients;
using IntegrantFirstTask.Helpers;
using IntegrantFirstTask.Interfaces;
using IntegrantFirstTask.Models;
using IntegrantFirstTask.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IntegrantFirstTask.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public IAzureManager Client { get; set; }
        public ICommand SyncCommand { get; set; }
        public IOfflineSyncManager OfflineManager { get; set; }

        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }

        public HomePageViewModel(double MinValue = 0, double MaxValue = 0)
        {
            MinPrice = MinValue;
            MaxPrice = MaxValue;
            Client = AzureManager.GetAzureManager(Constants.AzureUrl);
            OfflineManager = OfflineSyncManager.GetOfflineSyncManager(Constants.AzureUrl);
            SyncCommand = new Command(() => { SyncItems(); });
        }

        //GetAllFromAzureDB
        public async void GetAllItems()
        {
            try
            {
                if (ConnectionStatus == NetworkAccess.Internet)
                {
                    IsLoading = true;
                    var table = Client.GetTableReference<Item>();
                    var result = await Client.GetAllItemsAsync<Item>(table);
                    Filter(result);
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
            }
        }

        public async Task GetAllItemsFromLocalDBAsync()
        {
            try
            {
                IsLoading = true;
                var Table = OfflineManager.GetOfflineSyncTableReference<Item>();
                var result = await OfflineManager.GetAllOfflineSyncItemsAsync<Item>(Table);
                for (int i = 0; i < result?.Count; i++)
                {
                    result[i].SmallDetails = result[i].Details.Length > 51 ? result[i].Details.Substring(0, 50) : result[i].Details;
                }
                Filter(result);
                IsLoading = false;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                await PopUpsHelper.DisplayMessage("Sync Error", "Error While Getting all offline products", "OK");
            }
        }

        public async Task SyncItems()
        {
            try
            {
                IsLoading = true;
                var Table = OfflineManager.GetOfflineSyncTableReference<Item>();
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    await OfflineManager.PushAsync();
                    await OfflineManager.PullAsync<Item>(Table);
                    await NavigationHelper.PopPageAsync();
                    await NavigationHelper.NavigateToPageAsync(new HomePage());
                }
                else
                {
                    await PopUpsHelper.DisplayMessage("Connection Error", "Please Connect your phone to the internet", "OK");
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                IsLoading = false;
                await PopUpsHelper.DisplayMessage("Sync Error", "Error While Syncing new products", "OK");
            }
        }

        private void Filter(ObservableCollection<Item> Products)
        {
            try
            {
                if (MaxPrice == 0 && MinPrice == 0)
                    Items = Products;
                else
                    Items = new ObservableCollection<Item>(Products.Where(i => i.Price > MinPrice && i.Price < MaxPrice));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
            }
        }
    }
}
