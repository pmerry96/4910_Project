using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CPSC4910App
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        private void GoHomeButtonClick(object sender, RoutedEventArgs e)
        {
            // Home page hasn't been created/implemented yet
            Home goingHome = new Home();
            goingHome.Show();
            this.Close();
        }

        private void GoCatalogButtonClick(object sender, RoutedEventArgs e)
        {
            Catalog toCatalog = new Catalog();
            toCatalog.Show();
            this.Close();
        }

        private void GoShoppingButtonClick(object sender, RoutedEventArgs e)
        {
            ShoppingCart toCart = new ShoppingCart();
            toCart.Show();
            this.Close();
        }
    }
}
