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
	public partial class FiltersPage : ContentPage
	{
        public FilerPageViewModel VM { get; set; }
        public FiltersPage ()
		{
			InitializeComponent ();
            VM = new FilerPageViewModel();
            BindingContext = VM;
		}
	}
}