using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace IntegrantFirstTask.Interfaces
{
     public interface IAzureManager
    {
        IMobileServiceTable<T> GetTableReference<T>();
        Task<ObservableCollection<T>> GetAllItemsAsync<T>(IMobileServiceTable<T> Table);
        Task<T> GetItemByID<T, T2>(T2 id, IMobileServiceTable<T> Table);
        Task<bool> InsertObjectAsync<T>(T Object, IMobileServiceTable<T> Table);
        Task<bool> UpdateObjectAsync<T>(T Object, IMobileServiceTable<T> Table);
        Task<bool> DeleteObjectAsync<T>(T Object, IMobileServiceTable<T> Table);
    }
}
