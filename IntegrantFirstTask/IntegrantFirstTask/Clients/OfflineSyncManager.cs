using IntegrantFirstTask.Interfaces;
using IntegrantFirstTask.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace IntegrantFirstTask.Clients
{
    class OfflineSyncManager : IOfflineSyncManager
    {
        static OfflineSyncManager DefaultManager;
        public MobileServiceClient Client { get; set; }

        private OfflineSyncManager(string URL)
        {
            Client = GetOfflineSyncManagerClient(URL);
        }

        public static OfflineSyncManager GetOfflineSyncManager(string URL)
        {
            if (DefaultManager == null)
                DefaultManager = new OfflineSyncManager(URL);

            return DefaultManager;
        }

        private MobileServiceClient GetOfflineSyncManagerClient(string URL)
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

        public MobileServiceSQLiteStore InitiateSqlLiteStore(string StoreName)
        {
            try
            {
                var store = new MobileServiceSQLiteStore(StoreName);
                return store;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
                return null;
            }
        }

        public bool CreateSqlLiteTable<T>(MobileServiceSQLiteStore Store)
        {
            try
            {
                Store.DefineTable<T>();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message : {ex.Message} \n StackTrace : {ex.StackTrace}");
                return false;
            }
        }

        public IMobileServiceSyncTable<T> GetOfflineSyncTableReference<T>()
        {
            try
            {
                IMobileServiceSyncTable<T> ReturnedTable;
                if (Client != null)
                {
                    ReturnedTable = Client.GetSyncTable<T>();
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

        #region CRUD Operations

        public async Task<ObservableCollection<T>> GetAllOfflineSyncItemsAsync<T>(IMobileServiceSyncTable<T> Table)
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

        public async Task<T> GetItemByID<T>(string id, IMobileServiceSyncTable<T> Table)
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

        public async Task<bool> InsertOfflineSyncObjectAsync<T>(T Object, IMobileServiceSyncTable<T> Table)
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

        public async Task<bool> UpdateOfflineSyncObjectAsync<T>(T Object, IMobileServiceSyncTable<T> Table)
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

        public async Task<bool> DeleteOfflineSyncObjectAsync<T>(T Object, IMobileServiceSyncTable<T> Table)
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

        public async Task SyncAsync<T>(IMobileServiceSyncTable<T> Table)
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.Client.SyncContext.PushAsync();

                // The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                // Use a different query name for each unique query in your program.
                await Table.PullAsync(nameof(Table), Table.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        // Update failed, revert to server's copy
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
    }
}
