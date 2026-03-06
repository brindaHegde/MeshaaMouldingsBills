using Factory.Common.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class InvoiceDetails : ViewModelBase
    {
		private string _invoiceNumber;
		public string InvoiceNumber
		{
			get { return _invoiceNumber; }
			set { SetProperty(ref _invoiceNumber, value); }
		}

		private string _invoiceDate;
		public string InvoiceDate
		{
			get { return _invoiceDate; }
			set { SetProperty(ref _invoiceDate, value); }
		}

		private string _invoiceTime;
		public string InvoiceTime
		{
			get { return _invoiceTime; }
			set { SetProperty(ref _invoiceTime, value); }
		}

		private string _bags;
		public string Bags
		{
			get { return _bags; }
			set { SetProperty(ref _bags, value); }
		}

        public InvoiceDetails()
        {
            string content = File.ReadAllText("InvoiceNumber.txt");
			long nextInvoiceNum = 0;
			if (content != null)
                nextInvoiceNum = Int64.Parse(content);

            InvoiceNumber = nextInvoiceNum.ToString();
            DateTime now = DateTime.Now;
            InvoiceDate = now.ToString("dd/MM/yyyy");
			InvoiceTime = now.ToString("hh:mm tt");

			Bags = string.Empty;
        }
    }
}
