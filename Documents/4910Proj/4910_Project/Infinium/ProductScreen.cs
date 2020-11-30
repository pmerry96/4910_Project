using Infinium.Model;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
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

using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Data;

namespace Infinium
{
    public class ProductScreen : Form
    {
        Infinium _infinium;
        User _user;

        Label _prodTitle;
        Label _prodName;
        Label _prodPrice;
        Label _prodDescription;
        Label _emptyCart;

        PictureBox _prodImage;

        Button _returnToCartButton;
        Button _firstButton;
        Button _secondButton;
        Button _thirdButton;
        Button _fourthButton;

        string imagepath;

        int _n_items;
        int _i_item;

        List<Model.Product> prods;

        // User: User ID - 8
        // Cart: Cart ID - 8 
        // Product: Product_ID, Ebay_ID, Name, Price, Description, Display_IMG_Path
        // Fills_Cart: User_ID, Cart_ID
        // Cart_Contains: Cart_ID, Product_ID, Quantity (always 1)

        public ProductScreen(Form form, User user)
        {
            _infinium = (Infinium)form;
            _user = user;

            //title
            _prodTitle = new Label();
            _prodTitle.Text = "Product Information";
            _prodTitle.Location = new Point(5, 5);
            _infinium.Controls.Add(_prodTitle);

            _returnToCartButton = new Button();
            _returnToCartButton.Text = "Cart";
            _returnToCartButton.Location = new Point((_infinium.Width * 3 / 4) + 55, _prodTitle.Top);
            _infinium.Controls.Add(_returnToCartButton);
            _returnToCartButton.Click += OnClick_CartButton;

            //name
            _prodName = new Label();
            _prodName.Text = "[Product Name]";
            _prodName.Width = _infinium.Width;
            _prodName.Location = new Point(_prodTitle.Left, _prodTitle.Bottom + 5);
            _infinium.Controls.Add(_prodName);

            //price
            _prodPrice = new Label();
            _prodPrice.Text = "[Product Price ($)]";
            _prodPrice.Location = new Point(_prodName.Left, _prodName.Bottom + 5);
            _infinium.Controls.Add(_prodPrice);

            //description
            _prodDescription = new Label();
            _prodDescription.Text = "[Product Description]";
            _prodDescription.Location = new Point(_prodPrice.Left, _prodPrice.Bottom + 5);
            _prodDescription.Width = _infinium.Width;
            _infinium.Controls.Add(_prodDescription);

            //image
            _prodImage = new PictureBox();
            _prodImage.Location = new Point(_prodDescription.Left, _prodDescription.Bottom + 5);
            _prodImage.Width = 131;
            _prodImage.Height = 140;
            _infinium.Controls.Add(_prodImage);

            // |< button --> first
            _firstButton = new Button();
            _firstButton.Text = "|<";
            _firstButton.Location = new Point(25, _prodImage.Bottom + _prodImage.Height + 25);
            _firstButton.MouseClick += OnMouseClick_FirstItem;
            _infinium.Controls.Add(_firstButton);

            // < button --> previous
            _secondButton = new Button();
            _secondButton.Text = "<";
            _secondButton.Location = new Point(_firstButton.Right + 10, _prodImage.Bottom + _prodImage.Height + 25);
            _secondButton.MouseClick += OnMouseClick_PreviousItem;
            _infinium.Controls.Add(_secondButton);

            // > button --> next
            _thirdButton = new Button();
            _thirdButton.Text = ">";
            _thirdButton.Location = new Point(_secondButton.Right + 10, _prodImage.Bottom + _prodImage.Height + 25);
            _thirdButton.MouseClick += OnMouseClick_NextItem;
            _infinium.Controls.Add(_thirdButton);

            // >| button --> last
            _fourthButton = new Button();
            _fourthButton.Text = ">|";
            _fourthButton.Location = new Point(_thirdButton.Right + 10, _prodImage.Bottom + _prodImage.Height + 25);
            _fourthButton.MouseClick += OnMouseClick_LastItem;
            _infinium.Controls.Add(_fourthButton);

            // displays if no items in cart
            _emptyCart = new Label();
            _emptyCart.Text = "No products to see here! *Empty Cart*";
            _emptyCart.Width = _infinium.Width;
            _emptyCart.Location = new Point(5, 5);
            _infinium.Controls.Add(_emptyCart);
        }

        public void DisplayProduct()
        {
            foreach (Control _indexControl in _infinium.Controls)
            {
                if (_infinium._defaultTheme._isDarkMode)
                {
                    _infinium.BackColor = System.Drawing.Color.Black;
                    _indexControl.BackColor = System.Drawing.Color.Black;
                    _indexControl.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    _infinium.BackColor = System.Drawing.Color.White;
                    _indexControl.BackColor = System.Drawing.Color.White;
                    _indexControl.ForeColor = System.Drawing.Color.Black;
                }
            }
            _returnToCartButton.Show();

            _prodTitle.Hide();
            _prodName.Hide();
            _prodPrice.Hide();
            _prodDescription.Hide();
            _prodImage.Hide();
            _firstButton.Hide();
            _secondButton.Hide();
            _thirdButton.Hide();
            _fourthButton.Hide();
            _emptyCart.Show();

            GetProductsFromCart();
        }

        public void GetProductsFromCart()
        {
            DBServerInstance dbCon = DBServerInstance.Instance();
            int _cart_id = _user.getId();
            string query = "SELECT COUNT(*) AS nItems FROM Cart_Contains cc JOIN Product p ON p.Product_ID = cc.Product_ID WHERE cc.Cart_ID = @_cart_id ; " +
                "SELECT p.Product_ID, p.Name, p.Price, p.Description, p.Display_IMG_Path FROM Cart_Contains cc JOIN Product p ON p.Product_ID = cc.Product_ID WHERE cc.Cart_ID = @_cart_id;";
            List<string> targets = new List<string>();
            targets.Add("@_cart_id");
            List<string> parms = new List<string>();
            parms.Add(_cart_id.ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (rdr.Read())
            {
                _n_items = rdr.GetInt32("nItems");
                rdr.NextResult();
            }
            if (_n_items != 0)
            {
                _prodTitle.Show();
                _prodName.Show();
                _prodPrice.Show();
                _prodDescription.Show();
                _prodImage.Show();
                _firstButton.Show();
                _secondButton.Show();
                _thirdButton.Show();
                _fourthButton.Show();
                _emptyCart.Hide();
            }
            _i_item = -1;

            prods = new List<Model.Product>();
            while (rdr.Read())
            {
                int id = rdr.GetInt32("Product_ID");
                string name = rdr.GetString("Name");
                double price = rdr.GetDouble("Price");
                string desc = rdr.GetString("Description");
                string img = rdr.GetString("Display_IMG_Path");
                Model.Product _product = new Model.Product(id, "0", name, price, desc, img);
                prods.Add(_product);
            }
            rdr.Close();

            if (prods.Count > 0)
            {
                _i_item = 0;
                ReadCartItem(_i_item);
                _firstButton.Enabled = false;
                _secondButton.Enabled = false;
                if (_n_items == 0)
                {
                    _thirdButton.Enabled = false;
                    _fourthButton.Enabled = false;
                }
            }

            if (prods.Count == 1)
            {
                _firstButton.Enabled = false;
                _secondButton.Enabled = false;
                _thirdButton.Enabled = false;
                _fourthButton.Enabled = false;
            }


        }

        public void LoadPicturefromImagePath()
        {
            _prodImage.Load(imagepath);

        }

        private void ReadCartItem(int index)
        {
            _prodName.Text = prods[index].GetName();
            _prodPrice.Text = "$" + prods[index].GetPrice();
            _prodDescription.Text = prods[index].GetDescription();
            imagepath = prods[index].GetImgPath();
            LoadPicturefromImagePath();
            _firstButton.Enabled = true;
            _secondButton.Enabled = true;
            _thirdButton.Enabled = true;
            _fourthButton.Enabled = true;

        }

        public void OnMouseClick_FirstItem(object sender, MouseEventArgs e)
        {

            _i_item = 0;
            ReadCartItem(_i_item);
            _firstButton.Enabled = false;
            _secondButton.Enabled = false;
        }

        public void OnMouseClick_PreviousItem(object sender, MouseEventArgs e)
        {
            _i_item -= 1;
            ReadCartItem(_i_item);
            if (_i_item == 0)
            {
                _firstButton.Enabled = false;
                _secondButton.Enabled = false;
            }
        }

        public void OnMouseClick_NextItem(object sender, MouseEventArgs e)
        {
            _i_item += 1;
            ReadCartItem(_i_item);
            if (_i_item == _n_items - 1)
            {
                _thirdButton.Enabled = false;
                _fourthButton.Enabled = false;
            }

        }

        public void OnMouseClick_LastItem(object sender, MouseEventArgs e)
        {
            _i_item = _n_items - 1;
            ReadCartItem(_i_item);
            _thirdButton.Enabled = false;
            _fourthButton.Enabled = false;
        }

        public void OnClick_AccountButton(object sender, System.EventArgs e)
        {
            _infinium.ShowAccountScreen();
        }
        public void OnClick_CartButton(object sender, System.EventArgs e)
        {
            _infinium.ShowCartScreen(sender, e);
        }
    }
}
