using IntegrantFirstTask.Helpers;
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
	public partial class ShoppingCartPage : ContentPage
	{
        ShoppingCartViewModel VM;
		public ShoppingCartPage ()
		{
			InitializeComponent ();
            VM = new ShoppingCartViewModel();
            BindingContext = VM;
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var items =  await VM.GetCartItemsFromSQLLite();
            
            for (int i = 0; i < items.Count; i++)
            {
                ItemsGrid.RowDefinitions.Add(new RowDefinition() { Height = 80 }) ;
                StackLayout FirstPart = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal

                };
                Image IMG = new Image()
                {
                    Aspect = Aspect.AspectFit,
                    WidthRequest = 100,
                    HeightRequest = 100,
                    Source = (UriImageSource)ImageSource.FromUri(new Uri(items[i].ImgURL))
                };
                StackLayout Secondpart = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical
                };
                Label Name = new Label()
                {
                    Text = items[i].Name,
                    FontSize = 10,
                };
                Label InStock = new Label()
                {
                    FontSize = 8,
                    Text = "InStock \n gggggg",

                };
                Label button = new Label()
                {
                    FontSize = 8,
                    Text = "Delete",
                    TextColor = Color.Blue,
                    TextDecorations = TextDecorations.Underline

                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Command = VM.DeleteItemCommand;
                tapGestureRecognizer.CommandParameter = i;
                //tapGestureRecognizer.Tapped += (s, e) => {

                //    items.Remove(items[i]);
                //};
                button.GestureRecognizers.Add(tapGestureRecognizer);

                Secondpart.Children.Add(Name);
                Secondpart.Children.Add(InStock);
                Secondpart.Children.Add(button);
                FirstPart.Children.Add(IMG);
                FirstPart.Children.Add(Secondpart);

                Label Price = new Label()
                {
                    FontSize = 12,
                    FontAttributes = FontAttributes.Bold,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Text = items[i].Price.ToString()
                };

                Label Quantity = new Label()
                {
                    FontSize = 12,
                     VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Text = items[i].Count.ToString()
                };

                ItemsGrid.Children.Add(FirstPart, 0, i+1);
                ItemsGrid.Children.Add(Price, 1, i+1);
                ItemsGrid.Children.Add(Quantity, 2, i+1);
            }
        }
    }
}