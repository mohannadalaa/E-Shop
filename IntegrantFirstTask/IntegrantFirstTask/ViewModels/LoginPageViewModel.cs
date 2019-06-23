using IntegrantFirstTask.Clients;
using IntegrantFirstTask.Helpers;
using IntegrantFirstTask.Interfaces;
using IntegrantFirstTask.Models;
using IntegrantFirstTask.Services;
using IntegrantFirstTask.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace IntegrantFirstTask.ViewModels
{
    class LoginPageViewModel : BaseViewModel
    {
        public IAzureManager Client { get; set; }
        public IOfflineSyncManager OfflineManager { get; set; }
        public UserService UserService { get; set; }
        private bool _IsErrorShown = false;

        public bool IsErrorShown
        {
            get { return _IsErrorShown; }
            set
            {
                SetValue<bool>(ref _IsErrorShown, value);
            }
        }

        private string _ErrorMessage = "";
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                SetValue<string>(ref _ErrorMessage , value);
            }
        }


        private string _UserName = "";
        public string UserName
        {
            get { return _UserName; }
            set
            {
                SetValue<string>(ref _UserName, value);
                if (String.IsNullOrEmpty(value))
                    LoginButtonEnabled = false;
                else
                    LoginButtonEnabled = true;


            }
        }

        private bool _LoginButtonEnabled = false;
        public bool LoginButtonEnabled
        {
            get { return _LoginButtonEnabled; }
            set
            {
                SetValue<bool>(ref _LoginButtonEnabled, value);
            }
        }



        public ICommand LoginCommand { get; set; }

        public LoginPageViewModel()
        {
            LoginCommand = new Command(LoginButtonClicked);
            Client = AzureManager.GetAzureManager(Constants.AzureUrl);
            UserService = new UserService();
            OfflineManager = OfflineSyncManager.GetOfflineSyncManager(Constants.AzureUrl);
        }

        private async void LoginButtonClicked()
        {
            if(Regex.IsMatch(UserName, "[^A-Za-z0-9]+"))
            {
                IsErrorShown = true;
                ErrorMessage = "Invalid User Name Only Chars and numbers are allowed";
            }
            else
            {
                IsLoading = true;
                IsErrorShown = false;
                ErrorMessage = "";
                var UserTable = Client.GetTableReference<User>();
                var user = await UserService.PostUserAsync(new User(){Name = UserName });

               var  UsersTable = OfflineManager.GetOfflineSyncTableReference<User>();
               var Users = await OfflineManager.GetAllOfflineSyncItemsAsync<User>(UsersTable);
                if (!Users.Any(i => i.ID == SharedUser?.ID ))
                    await OfflineManager.InsertOfflineSyncObjectAsync<User>(new User() { Name = SharedUser?.Name, ID = SharedUser?.ID }, UsersTable);
                

                Application.Current.Properties["UserName"] = UserName;
                Application.Current.Properties["User"] = user != null ? user : new User() { Name = UserName};
                await NavigationHelper.NavigateToPageAsync(new HomePage());
                UserName = null;
                IsLoading = false;
            }
                
        }

    }
}
