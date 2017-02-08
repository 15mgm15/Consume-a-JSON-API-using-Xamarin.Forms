using SayUSDollar.ViewModel;
using Xamarin.Forms;

namespace SayUSDollar
{
	public partial class SayUSDollarPage : ContentPage
	{
		public SayUSDollarPage()
		{
			InitializeComponent();
			BindingContext = new CurrencyListViewModel();

			CurrencyList.ItemTapped += (sender, e) =>
			{
				CurrencyList.SelectedItem = null;
			};
		}
	}
}
