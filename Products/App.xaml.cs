using System.Collections.ObjectModel;

namespace Products
{
    public partial class App : Application
    {
        public static ObservableCollection<OrderItem> Orders { get; set; } = new ObservableCollection<OrderItem>();

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage (new LottieSplashPage());
        }
    }
}
