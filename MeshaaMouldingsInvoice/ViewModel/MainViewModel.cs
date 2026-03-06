using Factory.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoicePrint.Model;
using System.Windows;
using System.Collections.ObjectModel;
using Factory.Common;
using WpfApp1.Model;
using System.IO;
using System.Windows.Input;
using System.Data.Common;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace InvoicePrint
{
    public class MainViewModel : ViewModelBase
    {
        #region Buyer Details
        private BuyerDetails _buyerDetails;
		public BuyerDetails BuyerDetails
		{
			get { return _buyerDetails; }
			set { _buyerDetails = value; }
		}

        public ObservableCollection<BuyerDetails> BuyerDetailsCollection = new ObservableCollection<BuyerDetails>();

        public RelayCommand BuyerNameLostFocus { get; set; }
        private void ExecuteTextLostFocusCommand()
        {
            BuyerDetails tempBuyer = BuyerDetailsCollection.FirstOrDefault(o => o.BuyerName.Equals(BuyerDetails.BuyerName, StringComparison.InvariantCultureIgnoreCase));
            if (tempBuyer != null)
            {
                BuyerDetails.BuyerAddress1 = tempBuyer.BuyerAddress1;
                BuyerDetails.BuyerAddress2 = tempBuyer.BuyerAddress2;
                BuyerDetails.BuyerGSTIN = tempBuyer.BuyerGSTIN;
            }
            else
            {
                BuyerDetails.BuyerAddress1 = string.Empty;
                BuyerDetails.BuyerAddress2 = string.Empty;
                BuyerDetails.BuyerGSTIN = string.Empty;
            }
        }
        #endregion

        #region Invoice Details
        private InvoiceDetails _invoiceDetails;
        public InvoiceDetails InvoiceDetails
        {
            get { return _invoiceDetails; }
            set { _invoiceDetails = value; }
        }

        public RelayCommand InvoiceNumLostFocus { get; set; }

        private void ExecuteInvoiceNumLostFocusCommand()
        {
            if (InvoiceDetails.InvoiceNumber != null)
            {
                long nextInvoiceNum = Int64.Parse(InvoiceDetails.InvoiceNumber);
                nextInvoiceNum += 1;
                File.WriteAllText("InvoiceNumber.txt", nextInvoiceNum.ToString());
            }            
        }
        #endregion

        #region Goods Data
        private ObservableCollection<GoodsDetails> _goodsList;
        public ObservableCollection<GoodsDetails> GoodsList
        {
            get { return _goodsList; }
            set { SetProperty(ref _goodsList, value); }
        }

        #endregion

        #region Calculation
        private decimal _subTotal;
        public decimal SubTotal
        {
            get { return _subTotal; }
            set { 
                SetProperty(ref _subTotal, value);

                CGSTCalcVal = UpdateTaxPercentage(CGSTVal);
                SGSTCalcVal = UpdateTaxPercentage(SGSTVal);
                IGSTCalcVal = UpdateTaxPercentage(IGSTVal);
                InvokePropertyChanged(nameof(CGSTCalcVal));
                InvokePropertyChanged(nameof(SGSTCalcVal));
                InvokePropertyChanged(nameof(IGSTCalcVal));
                UpdateGrandTotal();
            }
        }

        private string _cGSTVal;
        public string CGSTVal
        {
            get { return _cGSTVal; }
            set { 
                SetProperty(ref _cGSTVal, value);
                CGSTCalcVal = UpdateTaxPercentage(CGSTVal);
                InvokePropertyChanged(nameof(CGSTCalcVal));
                UpdateGrandTotal();
            }
        }
        private string _cGSTCalcVal;
        public string CGSTCalcVal
        {
            get { return _cGSTCalcVal; }
            set { SetProperty(ref _cGSTCalcVal, value); }
        }


        private string _sGSTVal;
        public string SGSTVal
        {
            get { return _sGSTVal; }
            set { 
                SetProperty(ref _sGSTVal, value);
                SGSTCalcVal = UpdateTaxPercentage(SGSTVal);
                InvokePropertyChanged(nameof(SGSTCalcVal));
                UpdateGrandTotal();
            }
        }
        private string _sGSTCalcVal;
        public string SGSTCalcVal
        {
            get { return _sGSTCalcVal; }
            set { SetProperty(ref _sGSTCalcVal, value); }
        }

        private string _iGSTVal;
        public string IGSTVal
        {
            get { return _iGSTVal; }
            set { 
                SetProperty(ref _iGSTVal, value);
                IGSTCalcVal = UpdateTaxPercentage(IGSTVal);
                InvokePropertyChanged(nameof(IGSTCalcVal));
                UpdateGrandTotal();
            }
        }
        private string _iGSTCalcVal;
        public string IGSTCalcVal
        {
            get { return _iGSTCalcVal; }
            set { SetProperty(ref _iGSTCalcVal, value); }
        }

        private string _grandTotal;
        public string GrandTotal
        {
            get { return _grandTotal; }
            set { SetProperty(ref _grandTotal, value); }
        }

        private string UpdateTaxPercentage(string percent)
        {
            decimal dvalue = 0;
            if (!string.IsNullOrEmpty(percent))
            {
                dvalue = (SubTotal * decimal.Parse(percent)) / 100;
            }
            return dvalue.ToString();            
        }
        private void UpdateGrandTotal()
        {
            decimal dvalue = SubTotal;
            if (!string.IsNullOrEmpty(CGSTVal))
            {
                dvalue += decimal.Parse(CGSTCalcVal);
            }
            if (!string.IsNullOrEmpty(SGSTVal))
            {
                dvalue += decimal.Parse(SGSTCalcVal);
            }
            if (!string.IsNullOrEmpty(IGSTVal))
            {
                dvalue += decimal.Parse(IGSTCalcVal);
            }
            GrandTotal = dvalue.ToString();
            InvokePropertyChanged(nameof(GrandTotal));

            if (!string.IsNullOrEmpty(GrandTotal) && GrandTotal != "0")
            {
                StrGTotal = "Rupees in words \r\n" + NumberToText.ConvertToWords((long)decimal.Parse(GrandTotal)) + " only";
            }
            else
            {
                StrGTotal = "Rupees in words \r\n";
            }
        }

        private string _strGTotal;
        public string StrGTotal
        {
            get { return _strGTotal; }
            set { SetProperty(ref _strGTotal, value); }
        }

        public ICommand SubTotalGotFocus { get; set; }  
        private void ExecuteSubTotalGotFocusCommand()
        {
            SubTotal = 0;
            foreach(var item in GoodsList) 
            {              
                if (item.Amount != null)
                    SubTotal += decimal.Parse(item.Amount);
            }
        }
        #endregion

        #region Transporter Details
        private string _dispatchedBy;
        public string DispatchedBy
        {
            get { return _dispatchedBy; }
            set { SetProperty(ref _dispatchedBy, value); }
        }

        private string _ewayNo;
        public string EWayNo
        {
            get { return _ewayNo; }
            set { SetProperty(ref _ewayNo, value); }
        }

        private string _purchaseOrd;
        public string PurchaseOrd
        {
            get { return _purchaseOrd; }
            set { SetProperty(ref _purchaseOrd, value); }
        }

        private string _dcNo;
        public string DCNo
        {
            get { return _dcNo; }
            set { SetProperty(ref _dcNo, value); }
        }

        #endregion

        #region Print Button       

        public ICommand PrintButton { get; set; }

        private void ExecutePrintCommand(object obj)
        {
            if (obj is FrameworkElement element)
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    FixedDocument fixedDoc = new FixedDocument();
                    fixedDoc.DocumentPaginator.PageSize = new Size(794, 1123); // A4 @ 96dpi

                    string[] headings = { "ORIGINAL FOR RECIPIENT", "DUPLICATE FOR TRANSPORTER", "TRIPLICATE FOR SUPPLIER" };

                    foreach (var heading in headings)
                    {
                        FixedPage fixedPage = new FixedPage
                        {
                            Width = fixedDoc.DocumentPaginator.PageSize.Width,
                            Height = fixedDoc.DocumentPaginator.PageSize.Height
                        };

                        // 1) Heading
                        TextBlock headingBlock = new TextBlock
                        {
                            Text = heading,
                            FontSize = 20,
                            FontWeight = FontWeights.Bold,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 20, 0, 20)
                        };
                        fixedPage.Children.Add(headingBlock);

                        // 2) Visual copy of the actual invoice
                        Rectangle visualHost = new Rectangle
                        {
                            Width = element.ActualWidth,
                            Height = element.ActualHeight,
                            Fill = new VisualBrush(element)
                        };

                        FixedPage.SetTop(visualHost, 60); // leave space for heading
                        fixedPage.Children.Add(visualHost);

                        // Add to document
                        PageContent pc = new PageContent();
                        ((IAddChild)pc).AddChild(fixedPage);
                        fixedDoc.Pages.Add(pc);
                    }

                    printDialog.PrintDocument(fixedDoc.DocumentPaginator, "Invoice Print");
                }
            }
        }
        #endregion

        public MainViewModel(Window window)
        {
            BuyerDetails = new BuyerDetails();
            BuyerDetailsCollection.Add(new BuyerDetails() { BuyerName = "ARPITHA EXPORTS", BuyerAddress1 = "96/1, 1st MAIN, KOTTIGEPALAYA", BuyerAddress2 = "BANGALORE - 560 091", BuyerGSTIN = "29AAPFA9327B1ZB" });
            BuyerNameLostFocus = new RelayCommand(ExecuteTextLostFocusCommand);

            InvoiceDetails = new InvoiceDetails();
            InvoiceNumLostFocus = new RelayCommand(ExecuteInvoiceNumLostFocusCommand);

            GoodsList = new ObservableCollection<GoodsDetails>
            {
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
                new GoodsDetails(),
            };
            SubTotal = 0;
            SubTotalGotFocus = new RelayCommand(ExecuteSubTotalGotFocusCommand);

            PrintButton = new RelayCommand<object>(ExecutePrintCommand);
        }

        
    }
}
