using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using SayUSDollar.Model;

namespace SayUSDollar
{
	public class WebApi
	{
		//Static instace so we can acces this class everywhere
		public static WebApi Instance { get; } = new WebApi();

		//Our base API url
		static string _baseUrl { get { return "http://api.fixer.io/"; } }

		HttpClient CreateClient()
		{
			var httpCLient = new HttpClient(new NativeMessageHandler())
			{
				BaseAddress = new Uri(_baseUrl)
			};

			return httpCLient;
		}

		public async Task<List<Currency>> GetCurrenciesAsync()
		{
			var currenciesList = new List<Currency>();
			try
			{
				//TODO: Check network connection
				if (await CheckNetworkConnection())
				{
					var url = _baseUrl + "latest?base=USD";
					using (var httpClient = CreateClient())
					{
						var result = await httpClient.GetAsync(url);
						var responseText = await result.Content.ReadAsStringAsync();
						//Serialize the json object to our c# classes
						var rootObject = JsonConvert.DeserializeObject<RootObject>(responseText);

						foreach (var currency in rootObject.rates)
						{
							currenciesList.Add(new Currency
							{
								Name = currency.Key,
								Rate = currency.Value
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				//In case we have a problem...
				Console.WriteLine("Whooops! " + ex.Message);
			}
			return currenciesList;
		}

		public static async Task<bool> CheckNetworkConnection()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				return false;
			}

			if (!await CrossConnectivity.Current.IsRemoteReachable("https://www.google.com"))
			{
				return false;
			}

			return true;
		}
	}
}
