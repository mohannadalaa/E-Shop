using IntegrantFirstTask.Clients;
using IntegrantFirstTask.Interfaces;
using IntegrantFirstTask.Models;
using IntegrantFirstTask.SQLiteModels;
using Microsoft.WindowsAzure.MobileServices.Sync;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace IntegrantFirstTask.ViewModels
{
    class DetailsPageViewModel : BaseViewModel
    {
        private Item _item;
        readonly OfflineSyncManager Client = OfflineSyncManager.GetOfflineSyncManager(Constants.AzureUrl);
        private SQLiteAsyncConnection _connection;
        public Item Item
        {
            get { return _item; }
            set
            {
                SetValue<Item>(ref _item, value);
            }
        }

        private double _StepperValue = 1;
        public double StepperValue
        {
            get { return _StepperValue; }
            set
            {
                SetValue<double>(ref _StepperValue, value);
            }
        }
        public DetailsPageViewModel(Item _Item)
        {
            Item = _Item;
            _connection = DependencyService.Get<ISQLiteDB>().GetConnection();
        }

        public async void AddToLocalStore(Item _item)
        {
            try
            {
                var x = await _connection.Table<ShoppingCartItem>().ToListAsync();
                var res = new ObservableCollection<ShoppingCartItem>(x);

                if (res.Any(i => i.ItemID == _item.ID && i.UserName == SharedUser.Name))
                {
                    var item = res.FirstOrDefault(i => i.ItemID == _item.ID && i.UserName == SharedUser.Name);
                    item.Count = StepperValue;
                    await _connection.UpdateAsync(item);
                }
                else
                {
                    ShoppingCartItem sci = new ShoppingCartItem()
                    {
                        ItemID = _item.ID,
                        Details = _item.Details,
                        ImgURL = _item.ImgURL,
                        Name = _item.Name,
                        Price = _item.Price,
                        SmallDetails = _item.SmallDetails,
                        UserName = SharedUser.Name,
                        Count = StepperValue
                    };
                    await _connection.InsertAsync(sci);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
            }


            //IMobileServiceSyncTable<Item> Table = Client.GetOfflineSyncTableReference<Item>();
            //await Client.InsertOfflineSyncObjectAsync(_item, Table);
        }
    }
}
