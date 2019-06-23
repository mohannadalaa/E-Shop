using IntegrantFirstTask.Clients;
using IntegrantFirstTask.Interfaces;
using IntegrantFirstTask.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntegrantFirstTask.Services
{
    class UserService
    {
        IAPISManager APIManager;
        public UserService()
        {
            APIManager = new APISManager();
        }

        public async Task<User> GetUserByUserNameAsync (string UserName)
        {
            var uriBuilder = new UriBuilder(Constants.AzureUrl)
            {
                Path = $"api/users",
                Query = $"username={UserName}"
            };

            var user = await APIManager.GetAsync<User>(uriBuilder.Uri.AbsoluteUri);

            return user;
        }

        public async Task<User> PostUserAsync(User User)
        {
            try
            {
                var uriBuilder = new UriBuilder(Constants.AzureUrl)
                {
                    Path = $"api/users",
                };

                var user = await APIManager.PostAsync<User, User>(uriBuilder.Uri.AbsoluteUri, User);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
    }
}
