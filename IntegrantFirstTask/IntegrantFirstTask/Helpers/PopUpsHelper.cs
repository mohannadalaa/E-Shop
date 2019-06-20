using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IntegrantFirstTask.Helpers
{
    public static class PopUpsHelper
    {
        public static Task<bool> DisplayMessage(string title, string message, string accept, string cancel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
        public static Task DisplayMessage(string title, string message, string accept)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept);
        }
    }
}
