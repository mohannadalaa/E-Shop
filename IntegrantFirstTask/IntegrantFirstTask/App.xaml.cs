using IntegrantFirstTask.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IntegrantFirstTask.Clients;
using IntegrantFirstTask.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using IntegrantFirstTask.Models;
using SQLite;
using IntegrantFirstTask.SQLiteModels;
using System.Threading.Tasks;
using Xamarin.Essentials;
using IntegrantFirstTask.Helpers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace IntegrantFirstTask
{
    public partial class App : Application
    {
        IOfflineSyncManager OfflineManager;
        private SQLiteAsyncConnection _connection;
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage( new LoginPage());
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            try
            {
                _connection = DependencyService.Get<ISQLiteDB>().GetConnection();
                var res2 = await _connection.CreateTableAsync(typeof(ShoppingCartItem));

                await InitiateAzure();
                await SyncAllTables();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} , StackTrace:{ex.StackTrace}");
            }
        }

        private async Task InitiateAzure()
        {
            OfflineManager = OfflineSyncManager.GetOfflineSyncManager(Constants.AzureUrl);
            var store = OfflineManager.InitiateSqlLiteStore(Constants.LocalDBName);

            OfflineManager.CreateSqlLiteTable<Item>(store);
            OfflineManager.CreateSqlLiteTable<User>(store);
            OfflineManager.CreateSqlLiteTable<Order>(store);
            OfflineManager.CreateSqlLiteTable<OrderItems>(store);

            await OfflineManager.Client.SyncContext.InitializeAsync(store);
        }

        private async Task SyncAllTables()
        {
            var ItemsTable = OfflineManager.GetOfflineSyncTableReference<Item>();
            var UsersTable = OfflineManager.GetOfflineSyncTableReference<User>();
            var OrdersTable = OfflineManager.GetOfflineSyncTableReference<Order>();
            var OrderItemsTable = OfflineManager.GetOfflineSyncTableReference<OrderItems>();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await OfflineManager.PushAsync();
                await OfflineManager.PullAsync<Item>(ItemsTable);
                await OfflineManager.PullAsync<User>(UsersTable);
                await OfflineManager.PullAsync<Order>(OrdersTable);
                await OfflineManager.PullAsync<OrderItems>(OrderItemsTable);

                //var result = await OfflineManager.GetAllOfflineSyncItemsAsync<Item>(ItemsTable);
                //var result1 = await OfflineManager.GetAllOfflineSyncItemsAsync<User>(UsersTable);
                //var result2 = await OfflineManager.GetAllOfflineSyncItemsAsync<Order>(OrdersTable);
                //var result4 = await OfflineManager.GetAllOfflineSyncItemsAsync<OrderItems>(OrderItemsTable);
            }
            else
                await PopUpsHelper.DisplayMessage("Connection Error", "Please Connect your phone to the internet to sync items", "OK");
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
