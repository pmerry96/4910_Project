using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CPSC4910App
{
    /// <summary>
    /// Interaction logic for ShoppingCart.xaml
    /// </summary>
    public partial class ShoppingCart : Window
    {
        public ShoppingCart()
        {
            InitializeComponent();
        }

        // Populates page with one product's attributes
        // TODO: get image to appear, description not showing up either
        private void GetCheckoutItemsClick(object sender, RoutedEventArgs e)
        {
            // Connect to the database
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = "SELECT  `Product_ID`,  `Name`,  `Price`,  'Description',  `Display_IMG_Path` FROM `infTest_fel7`.`Product` LIMIT 1000;";
                MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read()) // Goes through every row in "Product" table, will be changed to list of products with the same attributes in the cart
                {
                    // Name - description to go in product description
                    string name = Convert.ToString(rdr["Name"]);
                    string desc = Convert.ToString(rdr["Description"]);
                    string productdescription = name + " - " + desc;
                    ProductDescription.Text = productdescription;
                    
                    // Setting quantity to 1 for now
                    string quantity = "1";
                    ProductQuantity.Text = quantity;
                    
                    // Price
                    decimal price = Convert.ToDecimal(rdr["Price"]);
                    ProductPrice.Text = price.ToString();

                    // Image path -https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.image.source?view=netcore-3.1
                    string img = Convert.ToString(rdr["Display_IMG_Path"]);
                    string imgpath = "\\" + img;
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri(imgpath, UriKind.Relative);
                    bi3.EndInit();
                    ProductImage.Stretch = System.Windows.Media.Stretch.Fill;
                    ProductImage.Source = bi3;
                    
                    // Console.WriteLine(imgpath); <-- this is outputting as '\rtx.png'
                }
            }
            dbCon.Close();
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
            Catalog toCart = new Catalog();
            toCart.Show();
            this.Close();
        }
    }
}
