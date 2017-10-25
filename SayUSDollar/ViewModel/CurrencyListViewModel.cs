using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using SayUSDollar.Model;
using Xamarin.Forms;

namespace SayUSDollar.ViewModel
{
	public class CurrencyListViewModel : INotifyPropertyChanged
	{
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

        bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
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

        //Refresh command
        Command _refreshCommand;
        public Command RefreshCommand
        {
            get
            {
                return _refreshCommand;
            }
        }

        #endregion
		
		public CurrencyListViewModel()
		{
			_currencyList = new List<Currency>();
			_refreshCommand = new Command(async () => await RefreshList());

			Task.Run(async () =>
			{
                IsBusy = true;
                CurrencyList = await PopulateList();
                IsBusy = false;
			});
		}

		#region Methods

		async Task<List<Currency>> PopulateList()
		{
            _currencyList = await DataLayer.Instance.GetRemoteOrLocalCurrenciesAsync().ConfigureAwait(false);
			return _currencyList;
		}

        async Task RefreshList()
        {
            IsRefreshing = true;
            CurrencyList = await PopulateList();
            IsRefreshing = false;
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
