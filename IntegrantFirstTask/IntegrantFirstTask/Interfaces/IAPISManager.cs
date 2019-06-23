using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntegrantFirstTask.Interfaces
{
    public interface IAPISManager
    {
        Task<T> GetAsync<T>(string uri);
        Task<TResult> PostAsync<T , TResult>(string uri , T Data);
    }
}
