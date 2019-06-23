using IntegrantFirstTask.Clients;
using IntegrantFirstTask.Helpers;
using IntegrantFirstTask.Interfaces;
using IntegrantFirstTask.Models;
using IntegrantFirstTask.SQLiteModels;
using IntegrantFirstTask.Views;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using User = IntegrantFirstTask.Models.User;

namespace IntegrantFirstTask.ViewModels
{
    class ShoppingCartViewModel : BaseViewModel
    {
        IOfflineSyncManager client = OfflineSyncManager.GetOfflineSyncManager(Constants.AzureUrl);
        private SQLiteAsyncConnection _connection;
        public ICommand DeleteItemCommand { get; set; }
        public ICommand SubmitCartCommand { get; set; }
        public IAzureManager Client { get; set; }
        string OrderID = Guid.NewGuid().ToString();

        private IMobileServiceSyncTable<User> UsersTable;
        private IMobileServiceSyncTable<Item> ItemsTable;
        private IMobileServiceSyncTable<Order> OrdersTable;
        private IMobileServiceSyncTable<OrderItems> OrdersItemsTable;

        private ObservableCollection<Item> AllItems;
        private ObservableCollection<User> Users;
        private ObservableCollection<Order> Orders;
        private ObservableCollection<OrderItems> OrdersItems;

        private ObservableCollection<Item> _CartItems;
        public ObservableCollection<Item> CartItems
        {
            get { return _CartItems; }
            set
            {
                SetValue<ObservableCollection<Item>>(ref _CartItems, value);
            }
        }

        private ObservableCollection<ShoppingCartItem> _SqlLiteItems;
        public ObservableCollection<ShoppingCartItem> SQLITEItems
        {
            get { return _SqlLiteItems; }
            set
            {
                SetValue<ObservableCollection<ShoppingCartItem>>(ref _SqlLiteItems, value);
            }
        }

        private bool _IsCartNotEmpty;
        public bool IsCartNotEmpty
        {
            get { return _IsCartNotEmpty; }
            set
            {
                SetValue<bool>(ref _IsCartNotEmpty, value);
                IsCartEmpty = !value;
            }
        }

        private bool _IsCartEmpty;
        public bool IsCartEmpty
        {
            get { return _IsCartEmpty; }
            set
            {
                SetValue<bool>(ref _IsCartEmpty, value);
            }
        }

        private bool _NotConnected;
        public bool NotConnected
        {
            get { return _NotConnected; }
            set
            {
                SetValue<bool>(ref _NotConnected, value);
            }
        }
        public ShoppingCartViewModel()
        {
            _connection = DependencyService.Get<ISQLiteDB>().GetConnection();
            Client = AzureManager.GetAzureManager(Constants.AzureUrl);
            DeleteItemCommand = new Command(DeleteItemFromSQLLite);
            SubmitCartCommand = new Command(SubmitCartToAzureDB);

            UsersTable = client.GetOfflineSyncTableReference<User>();
            ItemsTable = client.GetOfflineSyncTableReference<Item>();
            OrdersTable = client.GetOfflineSyncTableReference<Order>();
            OrdersItemsTable = client.GetOfflineSyncTableReference<OrderItems>();
        }

        public async void DeleteItemFromSQLLite(object Index)
        {
            try
            {
                await _connection.DeleteAsync(SQLITEItems[(int)Index]);
                if (CartItems.Contains(CartItems[(int)Index]))
                {
                    CartItems.Remove(CartItems[(int)Index]);
                    if (CartItems.Count == 0)
                        IsCartNotEmpty = false;
                }
                await Application.Current.MainPage.Navigation.PopAsync();
                await NavigationHelper.NavigateToPageAsync(new ShoppingCartPage());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
            }
        }

        public async Task<ObservableCollection<ShoppingCartItem>> GetCartItemsFromSQLLite()
        {
            try
            {
                IsLoading = true;
                var x = await _connection.Table<ShoppingCartItem>().ToListAsync();
                var res = new ObservableCollection<ShoppingCartItem>(x);
                SQLITEItems = new ObservableCollection<ShoppingCartItem>(res.Where(i => i.UserName == SharedUser.Name));
                if (SQLITEItems.Count == 0)
                    IsCartNotEmpty = false;
                else
                    IsCartNotEmpty = true;
                IsLoading = false;
                return SQLITEItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
                return null;
            }
        }

        public async void SubmitCartToAzureDB()
        {
            try
            {
                if (InternetConnected)
                {
                    List<OrderItems> Items = new List<OrderItems>();
                    for (int i = 0; i < SQLITEItems.Count; i++)
                    {
                        Items.Add(new OrderItems() { ItemCount = SQLITEItems[i].Count, ItemID = SQLITEItems[i].ItemID });
                    }

                    Order order = new Order()
                    {
                        User = new Models.User() { Name = SharedUser.Name , ID = SharedUser.ID },
                        OrderItems = Items,
                    };

                    var table = Client.GetTableReference<Order>();
                    await Client.InsertObjectAsync(order, table);
                }
                else
                {
                    await PopUpsHelper.DisplayMessage("Connection Error", "Please Connect your phone to the internet to Submit Order items", "OK");
                }
            }
            catch (Exception ex)
            {
                await PopUpsHelper.DisplayMessage("Connection Error", "Please Check Your Internet", "OK");
            }
        }

        public async void SubmitCartToAzureLocalDB()
        {
            try
            {
                List<OrderItems> Items = new List<OrderItems>();
                for (int i = 0; i < CartItems.Count; i++)
                {
                    Items.Add(new OrderItems() { ItemCount = CartItems[i].Count, ItemID = CartItems[i].ID, OrderID = OrderID, Id = Guid.NewGuid().ToString() });
                }

                Order order = new Order()
                {
                    SubmittedOffline = true,
                    ID = OrderID,
                    UserID = SharedUser.ID,
                };

                await client.InsertOfflineSyncObjectAsync<Order>(order,OrdersTable);
                for (int i = 0; i < Items.Count; i++)
                {
                    await client.InsertOfflineSyncObjectAsync<OrderItems>(Items[i], OrdersItemsTable);
                }
            }
            catch (Exception)
            {
                await PopUpsHelper.DisplayMessage("Connection Error", "Error While Submitting To Local DB", "OK");
            }
        }

        public async Task<ObservableCollection<Item>> GetCartItemsFromAzureLocalDB()
        {
            try
            {
                IsLoading = true;

                AllItems = await client.GetAllOfflineSyncItemsAsync<Item>(ItemsTable);
                Users = await client.GetAllOfflineSyncItemsAsync<User>(UsersTable);
                Orders = await client.GetAllOfflineSyncItemsAsync<Order>(OrdersTable);
                OrdersItems = await client.GetAllOfflineSyncItemsAsync<OrderItems>(OrdersItemsTable);


                User User = SharedUser;
                Order Order = Orders.FirstOrDefault(o => (o.UserID == User.ID && (o.SubmittedOffline == false || o.SubmittedOnline == false)));
                List<string> ItemsIDS = OrdersItems.Where(i2 => i2.OrderID == Order.ID).Select(i => i.ItemID).ToList();

                CartItems = new ObservableCollection<Item>(AllItems.Where(o =>
                {
                    var orderitem = OrdersItems.FirstOrDefault(oi => oi.ItemID == o.ID);
                    if (orderitem != null)
                        o.Count = orderitem.ItemCount;

                    return ItemsIDS.Contains(o.ID);

                }));
                if (CartItems.Count == 0)
                    IsCartNotEmpty = false;
                else
                    IsCartNotEmpty = true;
                IsLoading = false;
                return CartItems;
            }
            catch (Exception)
            {
                IsLoading = false;
                await PopUpsHelper.DisplayMessage("Connection Error", "Error While getting Data From local DB", "OK");
                return null;
            }
        }

        public async void DeleteItem(object Index)
        {
            try
            {
                OrdersItems = await client.GetAllOfflineSyncItemsAsync<OrderItems>(OrdersItemsTable);
                Item item = CartItems[(int)Index];
                Order Order = Orders.FirstOrDefault(o => (o.UserID == SharedUser.ID && (o.SubmittedOffline == false || o.SubmittedOnline == false)));
                var orderi = OrdersItems.FirstOrDefault(oi => oi.ItemID == item.ID && oi.OrderID == Order.ID);
                OrdersItems.Remove(orderi);

                await client.DeleteOfflineSyncObjectAsync(orderi, OrdersItemsTable);
                if (CartItems.Contains(CartItems[(int)Index]))
                {
                    CartItems.Remove(CartItems[(int)Index]);
                    if (CartItems.Count == 0)
                        IsCartNotEmpty = false;
                }
                await Application.Current.MainPage.Navigation.PopAsync();
                await NavigationHelper.NavigateToPageAsync(new ShoppingCartPage());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
            }
        }
    }
}

