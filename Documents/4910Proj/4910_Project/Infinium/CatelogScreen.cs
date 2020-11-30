using Infinium.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Infinium
{
    public class CatalogScreen : Form
    {
        User _user;
        Infinium infinium;
        Catalog catalog;

        Label catalogTitle;
        ListBox catalogListBox;
        Button returnToAccountButton;
        Button returnToCartButton;

        Label pointsLabel;
        Label usdToPointsLabel;

        public CatalogScreen(Form form, Catalog catalog, User user)
        {
            _user = user;
            infinium = (Infinium)form;
            infinium.Controls.Clear();
            infinium.Text = "Infinium";

            this.catalog = catalog;

            catalogTitle = new Label();
            catalogTitle.Text = catalog.GetSponsor().GetName() + "'s Catalog";
            catalogTitle.Location = new Point(5, 5);
            infinium.Controls.Add(catalogTitle);

            returnToAccountButton = new Button();
            returnToAccountButton.Text = "Cart";
            returnToAccountButton.Location = new Point((infinium.Width * 3 / 4) + 100, catalogTitle.Top);
            infinium.Controls.Add(returnToAccountButton);
            returnToAccountButton.Click += OnClick_CartButton;

            returnToCartButton = new Button();
            returnToCartButton.Text = "Account";
            returnToCartButton.Location = new Point((infinium.Width * 3 / 4) + 20, catalogTitle.Top);
            infinium.Controls.Add(returnToCartButton);
            returnToCartButton.Click += OnClick_AccountButton;

            catalogListBox = new ListBox();
            catalogListBox.Location = new Point(catalogTitle.Left, catalogTitle.Bottom + 5);
            catalogListBox.Size = new Size(infinium.Width * 3 / 4, infinium.Height * 3 / 5);
            infinium.Controls.Add(catalogListBox);
            catalogListBox.MouseDoubleClick += OnMouseDoubleClick_AddToCart;

            pointsLabel = new Label();
            pointsLabel.Text = "Points: " + user.getPoints();
            pointsLabel.AutoSize = true;
            pointsLabel.Location = new Point(420, 9);
            infinium.Controls.Add(pointsLabel);

            usdToPointsLabel = new Label();
            usdToPointsLabel.Text = "USD to Points: $" + catalog.GetSponsor().GetUsdToPoints();
            usdToPointsLabel.AutoSize = true;
            usdToPointsLabel.Location = new Point(520, 9);
            infinium.Controls.Add(usdToPointsLabel);

            PopulateCatalog();
        }

        public void DisplayCatalog()
        {
            foreach (Control _indexControl in infinium.Controls)
            {
                if (infinium._defaultTheme._isDarkMode)
                {
                    infinium.BackColor = System.Drawing.Color.Black;
                    _indexControl.BackColor = System.Drawing.Color.Black;
                    _indexControl.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    infinium.BackColor = System.Drawing.Color.White;
                    _indexControl.BackColor = System.Drawing.Color.White;
                    _indexControl.ForeColor = System.Drawing.Color.Black;
                }
            }

            catalogTitle.Show();
            catalogListBox.Show();
            returnToAccountButton.Show();
            returnToCartButton.Show();
            catalogListBox.Show();
        }

        public void OnClick_AccountButton(object sender, System.EventArgs e)
        {
            infinium.ShowAccountScreen();
        }

        public void OnClick_CartButton(object sender, System.EventArgs e)
        {
            infinium.ShowCartScreen(sender, e);
        }

        public void OnMouseDoubleClick_AddToCart(object sender, MouseEventArgs e)
        {
            int index = catalogListBox.IndexFromPoint(e.Location);

            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Product product = catalog.GetProducts()[index];
                UpdateDBWithSelectedItem(product);
                infinium._authenticatedUserAcct.getCart().addToCart(product);
            }
        }

        private int FindCartID()
        {
            int _cart_id = -1;
            DBServerInstance dbCon = DBServerInstance.Instance();
            int _user_id = _user.getId();
            string query = "SELECT Cart_ID FROM Fills_Cart WHERE User_ID = @_user_id;";
            List<string> targets = new List<string>();
            targets.Add("@_user_id");
            List<string> parms = new List<string>();
            parms.Add(_user_id.ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            /*
            if (rdr == null)
            {
                return -1;
            }
            */
            if (rdr.Read())
            {
                _cart_id = rdr.GetInt32("Cart_ID");
            }
            rdr.Close();
            return _cart_id;
        }
        private void UpdateDBWithSelectedItem(Product product)
        {
            int _cart_id = FindCartID();
            int _product_id = product.GetId();
            // ***
            bool existingprod = CheckForItem(_cart_id, _product_id);
            if (existingprod)
            {
                return;
            }
            // ***
            DBServerInstance dbCon = DBServerInstance.Instance();
            int _user_id = _user.getId();
            string query = "INSERT INTO Cart_Contains(Cart_ID, Product_ID, Quantity) VALUES(@_cart_id, @_product_id, 1);";
            List<string> targets = new List<string>();
            targets.Add("@_cart_id");
            targets.Add("@_product_id");
            List<string> parms = new List<string>();
            parms.Add(_cart_id.ToString());
            parms.Add(_product_id.ToString());
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);
        }

        private void PopulateCatalog()
        {
            catalog.Load();

            foreach (Product product in catalog.GetProducts())
            {
                int points = (int) (product.GetPrice() / catalog.GetSponsor().GetUsdToPoints()) + 1;
                string output = product.GetName().Substring(0, 50) + " - Price: $" + product.GetPrice() + " - Points: " + points;
                catalogListBox.Items.Add(output);
            }
        }

        private bool CheckForItem(int cartID, int productID)
        {
            // check cart_contains....
            // have cart_ID
            //if product_ID matches/is valid, increase the quantity by 1
            // else do what we were doing before

            int checkedquantity = -1;
            DBServerInstance dbCon = DBServerInstance.Instance();
            //string query = "SELECT Product_ID, Quantity FROM Cart_Contains WHERE Cart_ID = @cartID;";
            string query = "SELECT COUNT(*) AS nItems FROM Cart_Contains WHERE Cart_ID = @cartID AND Product_ID = @productID;";
            List<string> targets = new List<string>();
            targets.Add("@cartID");
            targets.Add("@productID");
            List<string> parms = new List<string>();
            parms.Add(cartID.ToString());
            parms.Add(productID.ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr.Read())
            {
                checkedquantity = rdr.GetInt32("nItems");
            }
            rdr.Close();

            if (checkedquantity > 0)
            {
                FindQuantity(cartID, productID);
                return true;
            }
            else return false;
        }

        private void FindQuantity(int cartID, int productID)
        {
            int quantity = -1;
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "SELECT Quantity FROM Cart_Contains WHERE Cart_ID = @cartID AND Product_ID = @productID;";
            List<string> targets = new List<string>();
            targets.Add("@cartID");
            targets.Add("@productID");
            List<string> parms = new List<string>();
            parms.Add(cartID.ToString());
            parms.Add(productID.ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr.Read())
            {
                quantity = rdr.GetInt32("Quantity");
            }
            rdr.Close();
            SetNewQuantity(cartID, productID, quantity);
        }

        private void SetNewQuantity(int cartID, int productID, int newquantity)
        {
            newquantity += 1;
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "UPDATE Cart_Contains SET Quantity = @newquantity WHERE Cart_ID = @cartID AND Product_ID = @productID;";
            List<string> targets = new List<string>();
            targets.Add("@newquantity");
            targets.Add("@cartID");
            targets.Add("@productID");
            List<string> parms = new List<string>();
            parms.Add(newquantity.ToString());
            parms.Add(cartID.ToString());
            parms.Add(productID.ToString());
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);

            foreach (CartItem cartItem in _user.getCart().getCartItems())
            {
                Product product = cartItem.GetProduct();
                if (product.GetId() == productID)
                {
                    product.SetQuantity(newquantity);
                }
            }
        }

    }
}