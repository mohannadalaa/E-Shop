using IntegrantFirstTask.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IntegrantFirstTask.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginPageViewModel VM = new LoginPageViewModel();
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = VM;
        }
    }
}