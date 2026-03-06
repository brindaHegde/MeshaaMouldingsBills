using Factory.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicePrint.Model
{
    public class BuyerDetails : ViewModelBase
    {
		private string _buyerName;
		public string BuyerName
		{
			get { return _buyerName; }
			set { SetProperty(ref _buyerName, value); }
		}

		private string _buyerAddress1;
		public string BuyerAddress1
		{
			get { return _buyerAddress1; }
			set { SetProperty(ref _buyerAddress1, value); }
		}

        private string _buyerAddress2;
        public string BuyerAddress2
        {
            get { return _buyerAddress2; }
            set { SetProperty(ref _buyerAddress2, value); }
        }

        private string _buyerGSTIN;
		public string BuyerGSTIN
		{
			get { return _buyerGSTIN; }
			set { SetProperty(ref _buyerGSTIN, value); }
		}

        public BuyerDetails()
        {
            BuyerName = string.Empty;
            BuyerAddress1 = string.Empty;
            BuyerAddress2 = string.Empty;
            BuyerGSTIN = string.Empty;
        }
    }
}
