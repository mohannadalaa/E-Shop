using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace IntegrantFirstTask.Interfaces
{
    public interface IOfflineSyncManager
    {
        MobileServiceClient Client { get; set; }
        MobileServiceSQLiteStore InitiateSqlLiteStore(string StoreName);
        bool CreateSqlLiteTable<T>(MobileServiceSQLiteStore Store);
        IMobileServiceSyncTable<T> GetOfflineSyncTableReference<T>();
        Task<ObservableCollection<T>> GetAllOfflineSyncItemsAsync<T>(IMobileServiceSyncTable<T> Table);
        Task<T> GetItemByID<T>(string id, IMobileServiceSyncTable<T> Table);
        Task<bool> InsertOfflineSyncObjectAsync<T>(T Object, IMobileServiceSyncTable<T> Table);
        Task<bool> UpdateOfflineSyncObjectAsync<T>(T Object, IMobileServiceSyncTable<T> Table);
        Task<bool> DeleteOfflineSyncObjectAsync<T>(T Object, IMobileServiceSyncTable<T> Table);
        Task SyncAsync<T>(IMobileServiceSyncTable<T> Table);
    }
}
