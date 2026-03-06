using Factory.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class GoodsDetails : ViewModelBase
    {
		private string _slNo;
		public string SlNo
		{
			get { return _slNo; }
			set { SetProperty(ref _slNo, value); }
		}

		private string _hsnCode;
		public string HSNCode
		{
			get { return _hsnCode; }
			set { SetProperty(ref _hsnCode, value); }
		}

		private string _description;
		public string Description
		{
			get { return _description; }
			set { SetProperty(ref _description, value); }
		}

		private string _quantity;
		public string Quantity
		{
			get { return _quantity; }
			set { 
				SetProperty(ref _quantity, value);
				UpdateAmount();
            }
		}

		private string _rate;
		public string Rate
		{
			get { return _rate; }
			set {
                if (decimal.TryParse(value, out _))
                    SetProperty(ref _rate, value);
                else
                    SetProperty(ref _rate, ""); ;

				UpdateAmount();
            }
        }

		private string _amount;
		public string Amount
		{
			get { return _amount; }
			set { SetProperty(ref _amount, value); }
		}

		private void UpdateAmount()
		{
            if (!string.IsNullOrEmpty(Rate) && !string.IsNullOrEmpty(Quantity))
            {
                var strAmt = (decimal.Parse(Quantity) * decimal.Parse(Rate));
                Amount = strAmt.ToString();
                InvokePropertyChanged(nameof(Amount));
            }
        }
        public GoodsDetails()
        {
			
        }
    }
}
