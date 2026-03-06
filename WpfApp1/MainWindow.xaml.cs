using System.Windows;

namespace InvoicePrint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainViewModel(this);
            InitializeComponent();
        }
    }
}
