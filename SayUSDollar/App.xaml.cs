using SQLite.Net.Interop;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SayUSDollar
{
	public partial class App : Application
	{
		public App(ISQLitePlatform platform)
		{
			DataLayer.Instance.SetDataBasePlatform(platform);
			InitializeComponent();

			MainPage = new NavigationPage(new SayUSDollarPage());
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
