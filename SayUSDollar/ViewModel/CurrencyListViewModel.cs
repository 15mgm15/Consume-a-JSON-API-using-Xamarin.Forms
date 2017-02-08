﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using SayUSDollar.Model;
using Xamarin.Forms;

namespace SayUSDollar.ViewModel
{
	public class CurrencyListViewModel : INotifyPropertyChanged
	{
		
		public CurrencyListViewModel()
		{
			_currencyList = new List<Currency>();
			//Since we are going to do UI changes we should do it on the UI Thread!
			Device.BeginInvokeOnMainThread(async () =>
			{
				await PopulateList();	
			});
		}

		#region Properties

		//To let the user know that we are working on something
		bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				_isBusy = value;
				OnPropertyChanged(nameof(IsBusy));
			}
		}

		//Our list of objects!
		List<Currency> _currencyList;
		public List<Currency> CurrencyList
		{
			get { return _currencyList; }
			set
			{
				_currencyList = value;
				OnPropertyChanged(nameof(CurrencyList));
			}
		}

		#endregion

		#region Methods

		async Task PopulateList()
		{
			IsBusy = true;
			var rootObject = await WebApi.Instance.GetCurrenciesAsync();

			if (rootObject == null)
			{
				IsBusy = false;
				return;
			}

			foreach (var currency in rootObject.rates)
			{
				_currencyList.Add(new Currency
				{
					Name = currency.Key,
					Rate = currency.Value
				});
			}

			CurrencyList = _currencyList;
			IsBusy = false;
		}

		#endregion

		#region INotifyPropertyChanged implementation

		//To let the UI know that something changed on the View Model
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion
	}
}