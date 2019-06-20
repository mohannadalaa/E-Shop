using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IntegrantFirstTask.Helpers
{
    public static class NavigationHelper
    {
        public static Task NavigateToModelAsync(Page page)
        {
            return Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public static Task NavigateToPageAsync(Page page)
        {
            return Application.Current.MainPage.Navigation.PushAsync(page);
        }

        public static void InsertPageBeforePage(Page CurrentPage, Page NewPage)
        {
            Application.Current.MainPage.Navigation.InsertPageBefore(NewPage, CurrentPage);
        }

        public static Task NavigateToRootAsync()
        {
           return Application.Current.MainPage.Navigation.PopToRootAsync(true);
        }

        public static Task PopPageAsync()
        {
            return Application.Current.MainPage.Navigation.PopAsync(true);
        }

        public static Task PopModelAsync()
        {
            return Application.Current.MainPage.Navigation.PopModalAsync(true);
        }
    }
}
