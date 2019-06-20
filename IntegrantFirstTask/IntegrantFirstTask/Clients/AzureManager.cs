using IntegrantFirstTask.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace IntegrantFirstTask.Clients
{
    public class AzureManager : IAzureManager
    {
        static AzureManager DefaultManager;
        private MobileServiceClient Client;

        public static AzureManager GetAzureManager(string URL)
        {
            if (DefaultManager == null)
                DefaultManager = new AzureManager(URL);

            return DefaultManager;
        }

        private AzureManager(String URL)
        {
            Client = GetAzureClient(URL);
        }

        private MobileServiceClient GetAzureClient(string URL)
        {
            try
            {
                var Result = new MobileServiceClient(URL);
                return Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
                if (Client != null)
                    Client.Dispose();

                return null;
            }
        }

        #region Online DB
        public IMobileServiceTable<T> GetTableReference<T>()
        {
            try
            {
                IMobileServiceTable<T> ReturnedTable;
                if (Client != null)
                {
                    ReturnedTable = Client.GetTable<T>();
                    return ReturnedTable;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
                return null;
            }
        }

        public async Task<ObservableCollection<T>> GetAllItemsAsync<T>(IMobileServiceTable<T> Table)
        {
            try
            {
                if (Table != null)
                {
                    IEnumerable<T> items = await Table.ToEnumerableAsync();
                    var returned = new ObservableCollection<T>(items);
                    return returned;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
                return null;
            }
        }

        public async Task<T> GetItemByID<T,T2>(T2 id, IMobileServiceTable<T> Table)
        {
            try
            {
                if (Table != null)
                {
                    var item = await Table.LookupAsync(id);
                    return item;
                }
                return default(T);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
                return default(T);
            }
        }

        public async Task<bool> InsertObjectAsync<T>(T Object, IMobileServiceTable<T> Table)
        {
            try
            {
                await Table.InsertAsync(Object);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> UpdateObjectAsync<T>(T Object, IMobileServiceTable<T> Table)
        {
            try
            {
                await Table.UpdateAsync(Object);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> DeleteObjectAsync<T>(T Object, IMobileServiceTable<T> Table)
        {
            try
            {
                await Table.DeleteAsync(Object);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
                return false;
            }
        }

        #endregion
        
    }
}
