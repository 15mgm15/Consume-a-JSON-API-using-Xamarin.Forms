using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SayUSDollar.Model;
using SQLite.Net.Interop;

namespace SayUSDollar
{
	public class DataLayer
	{
		public static DataLayer Instance { get; } = new DataLayer();

		Database Database;

		public void SetDataBasePlatform(ISQLitePlatform platform)
		{
			if (Database == null)
			{
				Database = new Database(platform);
				Database.Open();
			}
		}

		public async Task<List<Currency>> GetRemoteOrLocalCurrenciesAsync()
		{
			var localCurrenciesList = new List<Currency>();

			try
			{

				var remoteCurrenciesList = await WebApi.Instance.GetCurrenciesAsync();
				if (remoteCurrenciesList == null || remoteCurrenciesList?.Count == 0)
				{
					//There are no currencies on the server side, or there is no internet connection
					//Try to show the database ones if there are some...
					localCurrenciesList = Database.Fetch<Currency>().ToList();
				}
				else
				{
					localCurrenciesList = remoteCurrenciesList;
					Database.TruncateTable<Currency>();
					foreach (var currency in localCurrenciesList)
					{
						Database.Insert(currency);
					}
				}

			}
			catch(Exception ex)
			{
				//In case we have a problem...
				Console.WriteLine("Whooops! " + ex.Message);
			}

			return localCurrenciesList;
		}
	}
}
