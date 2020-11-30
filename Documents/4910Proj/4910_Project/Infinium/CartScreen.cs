using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Infinium.Model;
using MySql.Data.MySqlClient;



namespace Infinium
{
    public class CartScreen : Form
    {
        Infinium _infinium;
        User _user;

        Label _cartScreenLabel;
        ListBox _cartListBox;
        Button _returnToAccountButton;
        Button _getCheckoutItemsButton;
        Label _specialOfferLabel;
        TextBox _specialOfferTextBox;
        Label _subTotal, _subTotalPrice, _calcTotalLabel, _calcTotalLabelPrice, _shippingLabel, _shippingLabelPrice, _taxLabel, _taxLabelPrice, _calcTotalPointsLabel, _calcTotalPointsLabelPrice;
        Button _applyButton;
        Button _goToProduct;
        Button _submitOrder;
        Label _orderMessage;

        int _n_items;

        List<Model.Product> prods;
        Cart _tempcart;

        double finalpointscost;
        int usersponsorpoints;
        int _sale_ID;
        int order_number;
        double final_cart_cost;
        int sponsorID;

        public CartScreen(Form form, User user)
        {
            _user = user;

            _infinium = (Infinium)form;

            _cartScreenLabel = new Label();
            _cartScreenLabel.Text = "My Shopping Cart";
            _cartScreenLabel.Location = new Point(5, 5);
            _infinium.Controls.Add(_cartScreenLabel);

            _cartListBox = new ListBox();
            _cartListBox.Location = new Point(_cartScreenLabel.Left, _cartScreenLabel.Bottom + 5);
            _cartListBox.Size = new Size(_infinium.Width * 3 / 4, _infinium.Height * 3 / 5);
            _cartListBox.AutoSize = true;
            _infinium.Controls.Add(_cartListBox);
            _cartListBox.MouseDoubleClick += OnMouseDoubleClick_RemoveFromCart;

            _returnToAccountButton = new Button();
            _returnToAccountButton.Text = "Account";
            _returnToAccountButton.Location = new Point((_infinium.Width * 3 / 4) - 80, _cartScreenLabel.Top);
            _infinium.Controls.Add(_returnToAccountButton);
            _returnToAccountButton.Click += OnClick_AccountButton;


            _getCheckoutItemsButton = new Button();
            _getCheckoutItemsButton.Text = "Get Items";
            _getCheckoutItemsButton.Width = 100;
            _getCheckoutItemsButton.Location = new Point(_returnToAccountButton.Right + 5, _returnToAccountButton.Top);
            _infinium.Controls.Add(_getCheckoutItemsButton);
            _getCheckoutItemsButton.Click += _getCheckoutItemsButton_Click;


            _specialOfferLabel = new Label();
            _specialOfferLabel.Text = "Special Offers";
            _specialOfferLabel.Location = new Point(_cartScreenLabel.Left, _cartListBox.Bottom + 5);
            _infinium.Controls.Add(_specialOfferLabel);

            _specialOfferTextBox = new TextBox();
            _specialOfferTextBox.Text = "Enter Coupon Code Here";
            _specialOfferTextBox.Location = new Point(_specialOfferLabel.Right + 5, _specialOfferLabel.Top);
            _infinium.Controls.Add(_specialOfferTextBox);

            _subTotal = new Label();
            _subTotal.Text = "Subtotal: ";
            _subTotal.Location = new Point(_cartListBox.Right + 10, _cartListBox.Bottom - 15);
            _infinium.Controls.Add(_subTotal);

            _subTotalPrice = new Label();
            _subTotalPrice.Width = 200;
            _subTotalPrice.Location = new Point(_subTotal.Right + 10, _subTotal.Top);
            _infinium.Controls.Add(_subTotalPrice);

            _taxLabel = new Label();
            _taxLabel.Text = "Tax Added: ";
            _taxLabel.Location = new Point(_subTotal.Left, _subTotal.Bottom + 5);
            _infinium.Controls.Add(_taxLabel);

            _taxLabelPrice = new Label();
            _taxLabelPrice.Width = 200;
            _taxLabelPrice.Location = new Point(_taxLabel.Right + 10, _taxLabel.Top);
            _infinium.Controls.Add(_taxLabelPrice);

            _shippingLabel = new Label();
            _shippingLabel.Text = "Shipping Costs: ";
            _shippingLabel.Location = new Point(_taxLabel.Left, _taxLabel.Bottom + 5);
            _infinium.Controls.Add(_shippingLabel);

            _shippingLabelPrice = new Label();
            _shippingLabelPrice.Width = 200;
            _shippingLabelPrice.Location = new Point(_shippingLabel.Right + 10, _shippingLabel.Top);
            _infinium.Controls.Add(_shippingLabelPrice);

            _calcTotalLabel = new Label();
            _calcTotalLabel.Text = "Calculated Total: ";
            _calcTotalLabel.Location = new Point(_shippingLabel.Left, _shippingLabel.Bottom + 5);
            _infinium.Controls.Add(_calcTotalLabel);

            _calcTotalLabelPrice = new Label();
            _calcTotalLabelPrice.Width = 200;
            _calcTotalLabelPrice.Location = new Point(_calcTotalLabel.Right + 10, _calcTotalLabel.Top);
            _calcTotalLabelPrice.Font = new Font(SystemFonts.DefaultFont.FontFamily, SystemFonts.DefaultFont.Size, FontStyle.Bold);
            _infinium.Controls.Add(_calcTotalLabelPrice);

            _calcTotalPointsLabel = new Label();
            _calcTotalPointsLabel.Text = "Total Points/Dollar: ";
            _calcTotalPointsLabel.Location = new Point(_calcTotalLabel.Left, _calcTotalLabel.Bottom + 5);
            _infinium.Controls.Add(_calcTotalPointsLabel);

            _calcTotalPointsLabelPrice = new Label();
            _calcTotalPointsLabelPrice.Width = 200;
            _calcTotalPointsLabelPrice.Location = new Point(_calcTotalPointsLabel.Right + 10, _calcTotalPointsLabel.Top);
            _calcTotalPointsLabelPrice.Font = new Font(SystemFonts.DefaultFont.FontFamily, SystemFonts.DefaultFont.Size, FontStyle.Bold);
            _infinium.Controls.Add(_calcTotalPointsLabelPrice);

            _taxLabel = new Label();
            _taxLabel.Text = "Tax added: ";
            _taxLabel.Location = new Point(_shippingLabel.Left, _shippingLabel.Bottom + 5);
            _infinium.Controls.Add(_taxLabel);

            _applyButton = new Button();
            _applyButton.Text = "Apply";
            _applyButton.Location = new Point(_specialOfferTextBox.Right + 5, _specialOfferLabel.Top - 1);
            _infinium.Controls.Add(_applyButton);
            _applyButton.Click += onClick_applyCoupon;

            _goToProduct = new Button();
            _goToProduct.Text = "Product";
            _goToProduct.Location = new Point((_infinium.Width * 3 / 4) - 160, _cartScreenLabel.Top);
            _infinium.Controls.Add(_goToProduct);
            _goToProduct.Click += onClick_productPageButton;

            _submitOrder = new Button();
            _submitOrder.Width = 200;
            _submitOrder.Text = "Submit Order";
            _submitOrder.Location = new Point(_applyButton.Right + 15, _specialOfferLabel.Top);
            _infinium.Controls.Add(_submitOrder);
            _submitOrder.Click += onClick_beginOrder;

            _orderMessage = new Label();
            _orderMessage.Location = new Point(_specialOfferLabel.Left, _specialOfferLabel.Bottom + 5);
            _orderMessage.Width = _infinium.Width / 2;
            _infinium.Controls.Add(_orderMessage);
        }

        public void DisplayCart()
        {
            foreach (Control _indexControl in _infinium.Controls)
            {
                _cartListBox.Items.Clear();
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
            _cartScreenLabel.Show();
            _cartListBox.Show();
            _returnToAccountButton.Show();
            _getCheckoutItemsButton.Show();
            _specialOfferLabel.Show();
            _specialOfferTextBox.Show();
            _subTotal.Show();
            _subTotalPrice.Show();
            _calcTotalLabel.Show();
            _calcTotalLabelPrice.Show();
            _shippingLabel.Show();
            _shippingLabelPrice.Show();
            _taxLabel.Show();
            _taxLabelPrice.Show();
            _applyButton.Show();
            _submitOrder.Show();

            CheckIfCartCreated();
        }

        public void onClick_productPageButton(object sender, System.EventArgs e)
        {
            _infinium.ShowProductScreen();
        }

        public void PopulateCart()
        {
            int _quantity;
            int _cart_ID = _user.getCart().getID();
            foreach (CartItem cartItem in _user.getCart().getCartItems())
            {
                Product product = cartItem.GetProduct();
                string output;
                _quantity = FindQuantity(_cart_ID, product.GetId());
                if (_quantity != 1)
                {
                    Cart tempCart = _user.getCart();
                    for (int i = 0; i < _quantity - 1; i++)
                    {
                        tempCart.addToCart(product);
                    }
                }
                //output = product.GetName() + " - " + product.GetDescription() + " Quantity: " + _quantity + " Price: $" + (product.GetPrice() * _quantity) + " " + product.GetImgPath();
                output = product.GetId() + ": " + product.GetName().Substring(0, 25) + " - Total Cost: $" + product.GetPrice() + " x " + _quantity + " = $ " + (product.GetPrice() * _quantity);
                _cartListBox.Items.Add(output);
            }
            Cart NEWtempCart = _user.getCart();
            _subTotalPrice.Text = "$" + _user.getCart().getProdCost();
            _calcTotalLabelPrice.Text = "$" + _user.getCart().getTotalCost();
            _shippingLabelPrice.Text = "$" + _user.getCart().getShipCost();
            _taxLabelPrice.Text = "$" + _user.getCart().getTaxCost();
            if (_user.getSelectedSponsor() == null)
            {
                FindSelectSponsor();
            }
            else usersponsorpoints = _user.getCart().getPointsCost(_user.getSelectedSponsor());
            _calcTotalPointsLabelPrice.Text = _user.getCart().getPointsCost(_user.getSelectedSponsor()).ToString();
            finalpointscost = Convert.ToDouble(_user.getCart().getPointsCost(_user.getSelectedSponsor()).ToString());
        }

        private void _getCheckoutItemsButton_Click(object sender, EventArgs e)
        {
            if (_calcTotalPointsLabelPrice.Text != "")
            {
                _calcTotalPointsLabelPrice.Text = "";
            }
            _user.clearCart();
            _tempcart = _user.getCart();
            _cartListBox.Items.Clear();
            DBServerInstance dbCon = DBServerInstance.Instance();
            int _cart_id = _user.getCart().getID();
            string query = "SELECT COUNT(*) AS nItems FROM Cart_Contains cc JOIN Product p ON p.Product_ID = cc.Product_ID WHERE cc.Cart_ID = @_cart_id ; " +
                "SELECT p.Product_ID, p.Ebay_ID, p.Name, p.Price, p.Description, p.Display_IMG_Path FROM Cart_Contains cc JOIN Product p ON p.Product_ID = cc.Product_ID WHERE cc.Cart_ID = @_cart_id;";
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
            while (rdr.Read())
            {
                int id = rdr.GetInt32("Product_ID");
                string ebayID = rdr.GetString("Ebay_ID");
                string name = rdr.GetString("Name");
                double price = rdr.GetDouble("Price");
                string desc = rdr.GetString("Description");
                string img = rdr.GetString("Display_IMG_Path");
                Product tempProduct = new Product(id, ebayID, name, price, desc, img);
                _tempcart.addToCart(tempProduct);
            }
            rdr.Close();

            PopulateCart();

            if (_cartListBox.Items.Count == 0)
            {
                _shippingLabelPrice.Text = "$0";
                _taxLabelPrice.Text = "$0";
                _subTotalPrice.Text = "$0";
                _calcTotalLabelPrice.Text = "$0";
                _calcTotalPointsLabelPrice.Text = "0";
                _submitOrder.Hide();
            }
        }

        private void GetNewCheckoutItems()
        {
            _user.clearCart();
            _tempcart = _user.getCart();
            _cartListBox.Items.Clear();
            DBServerInstance dbCon = DBServerInstance.Instance();
            int _cart_id = _user.getCart().getID();
            string query = "SELECT COUNT(*) AS nItems FROM Cart_Contains cc JOIN Product p ON p.Product_ID = cc.Product_ID WHERE cc.Cart_ID = @_cart_id ; " +
                "SELECT p.Product_ID, p.Ebay_ID, p.Name, p.Price, p.Description, p.Display_IMG_Path FROM Cart_Contains cc JOIN Product p ON p.Product_ID = cc.Product_ID WHERE cc.Cart_ID = @_cart_id;";
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
            while (rdr.Read())
            {
                int id = rdr.GetInt32("Product_ID");
                string ebayID = rdr.GetString("Ebay_ID");
                string name = rdr.GetString("Name");
                double price = rdr.GetDouble("Price");
                string desc = rdr.GetString("Description");
                string img = rdr.GetString("Display_IMG_Path");
                Product tempProduct = new Product(id, ebayID, name, price, desc, img);
                _tempcart.addToCart(tempProduct);
            }
            rdr.Close();

            PopulateCart();

            if (_cartListBox.Items.Count == 0)
            {
                _shippingLabelPrice.Text = "$0";
                _taxLabelPrice.Text = "$0";
                _subTotalPrice.Text = "$0";
                _calcTotalLabelPrice.Text = "$0";
                _calcTotalPointsLabelPrice.Text = "0";
                _submitOrder.Hide();
            }
        }

        public void OnClick_AccountButton(object sender, System.EventArgs e)
        {
            _infinium.ShowAccountScreen();
        }

        public void OnClick_CatalogButton(object sender, System.EventArgs e)
        {
            _infinium.ShowCatalogScreen();
        }
        public void CheckIfCartCreated()
        {
            var dbcon = DBServerInstance.Instance();
            int _user_id = _user.getId();
            int _hasacart = 0;
            string query = "SELECT COUNT(*) AS nCarts FROM Fills_Cart WHERE User_ID = @_user_id;";
            List<string> targets = new List<string>();
            targets.Add("@_user_id");
            List<string> parms = new List<string>();
            parms.Add(_user_id.ToString());
            MySqlDataReader rdr = dbcon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (rdr.Read())
            {
                _hasacart = rdr.GetInt32("nCarts");
            }
            rdr.Close();
            if (_hasacart != 0)
            {
                return;
            }
            ConnectToUser(_user_id);
        }
        private void FindSelectSponsor()
        {
            int usersID = _user.getId();
            sponsorID = -1;
            usersponsorpoints = -1;
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "SELECT * FROM Sponsored_By WHERE UserID = @usersID;";
            List<string> targets = new List<string>();
            targets.Add("@usersID");
            List<string> parms = new List<string>();
            parms.Add(usersID.ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (rdr.Read())
            {
                sponsorID = rdr.GetInt32("SponsorID");
                usersponsorpoints = rdr.GetInt32("Points");
            }
            rdr.Close();
            if (sponsorID != -1)
            {
                SetSponsorForPoints(sponsorID);
            }
        }

        private void SetSponsorForPoints(int sponsorID)
        {
            string sponsorname = "";
            double USD_To_Points = 0.0;
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "SELECT * FROM Sponsor WHERE Sponsor_ID = @sponsorID;";
            List<string> targets = new List<string>();
            targets.Add("@sponsorID");
            List<string> parms = new List<string>();
            parms.Add(sponsorID.ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (rdr.Read())
            {
                sponsorname = rdr.GetString("Sponsor_Name");
                USD_To_Points = rdr.GetDouble("USD_To_Points");
            }
            rdr.Close();

            Sponsor newsponsor = new Sponsor(sponsorID, sponsorname, USD_To_Points);
            _user.setSelectedSponsor(sponsorname);
        }
        public void ConnectToUser(int user_ID)
        {
            int cart_ID = user_ID;

            var dbcon = DBServerInstance.Instance();
            string query = "INSERT INTO Cart (Cart_ID) VALUES (@cart_ID)";
            List<string> targets = new List<string>();
            targets.Add("@cart_ID");
            List<string> parms = new List<string>();
            parms.Add(cart_ID.ToString());
            dbcon.ExecuteParameterizedQuery(query, targets, parms, false);
            ConnectUserToCart(user_ID);
        }

        public void ConnectUserToCart(int user_ID)
        {
            int cart_ID = user_ID;

            var dbcon = DBServerInstance.Instance();
            string query = "INSERT INTO Fills_Cart (User_ID, Cart_ID) VALUES (@user_id, @cart_id);";
            List<string> targets = new List<string>();
            targets.Add("@user_ID");
            targets.Add("@cart_ID");
            List<string> parms = new List<string>();
            parms.Add(user_ID.ToString());
            parms.Add(cart_ID.ToString());
            dbcon.ExecuteParameterizedQuery(query, targets, parms, false);
        }

        public int FindQuantity(int cartID, int productID)
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
            if (rdr == null)
            {
                return -1;
            }
            if (rdr.Read())
            {
                quantity = rdr.GetInt32("Quantity");
            }
            rdr.Close();

            return quantity;
        }

        public void OnMouseDoubleClick_RemoveFromCart(object sender, MouseEventArgs e)
        {
            int index = _cartListBox.IndexFromPoint(e.Location);

            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                string gettingProdID = _cartListBox.Items[index].ToString();
                int colonLocation = gettingProdID.IndexOf(":", StringComparison.Ordinal);
                string prodIDstr = gettingProdID.Substring(0, colonLocation);
                int _prodID = Convert.ToInt32(prodIDstr);

                int _cart_ID = _user.getCart().getID();
                int _foundQuantity = FindQuantity(_cart_ID, _prodID);
                if (_foundQuantity == 1)
                {
                    DeleteOneItemFromDB(_cart_ID, _prodID);
                }
                else if (_foundQuantity > 1)
                {
                    SetNewQuantity(_cart_ID, _prodID, _foundQuantity - 1);
                }
                CheckIfCartCreated();

                _subTotalPrice.Text = "$";
                _calcTotalLabelPrice.Text = "$";
                _shippingLabelPrice.Text = "$";
                _taxLabelPrice.Text = "$";

                GetNewCheckoutItems();
            }
        }

        public void DeleteOneItemFromDB(int cartID, int productID)
        {
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "DELETE FROM Cart_Contains WHERE Cart_ID = @cartID AND Product_ID = @productID;";
            List<string> targets = new List<string>();
            targets.Add("@cartID");
            targets.Add("@productID");
            List<string> parms = new List<string>();
            parms.Add(cartID.ToString());
            parms.Add(productID.ToString());
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);
        }

        private void SetNewQuantity(int cartID, int productID, int newquantity)
        {
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

        private void onClick_applyCoupon(object sender, System.EventArgs e)
        {
            string potentialcoupon = _specialOfferTextBox.Text;
            _sale_ID = -1;
            decimal minamount = -1;
            if (potentialcoupon != "Enter Coupon Code Here")
            {
                DBServerInstance dbCon = DBServerInstance.Instance();
                string query = "SELECT Sale_ID, Min_AMT FROM Sale WHERE CODE = @potentialcoupon";
                List<string> targets = new List<string>();
                targets.Add("@potentialcoupon");
                List<string> parms = new List<string>();
                parms.Add(potentialcoupon);
                MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
                if (rdr == null)
                {
                    return;
                }
                if (rdr.Read())
                {
                    _sale_ID = rdr.GetInt32("Sale_ID");
                    minamount = rdr.GetDecimal("Min_AMT");
                }
                rdr.Close();
                if (_sale_ID == -1 || minamount == -1)
                {
                    _orderMessage.Text = "Invalid coupon code";
                    return;
                }
                else
                {
                    if (_calcTotalLabelPrice.Text == "")
                    {
                        return;
                    }
                    double total_cost = Convert.ToDouble(_calcTotalLabelPrice.Text.Substring(1));
                    if (total_cost > Convert.ToDouble(minamount))
                    {
                        _orderMessage.Text = "";
                        GetDollarsOffTotal(_sale_ID, total_cost);
                    }
                    else return;
                }
            }
            else return;
            _applyButton.Hide();
        }

        private void GetDollarsOffTotal(int _sale_ID, double total_cart_cost)
        {
            decimal dollars_off = 0;
            final_cart_cost = 0;
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "SELECT Dollars_off FROM Direct_Sale WHERE Sale_ID = @_sale_ID;";
            List<string> targets = new List<string>();
            targets.Add("@_sale_ID");
            List<string> parms = new List<string>();
            parms.Add(_sale_ID.ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (rdr.Read())
            {
                dollars_off = rdr.GetDecimal("Dollars_off");
            }
            rdr.Close();

            final_cart_cost = total_cart_cost - Convert.ToDouble(dollars_off);
            Debug.WriteLine("initial cost = " + total_cart_cost + " final cost = " + final_cart_cost);

            _calcTotalLabelPrice.Text = "$" + Convert.ToString(final_cart_cost);
            double USDTOPOINTS = _user.getSelectedSponsor().GetUsdToPoints();
            finalpointscost = (int)(final_cart_cost / _user.getSelectedSponsor().GetUsdToPoints());
            _calcTotalPointsLabelPrice.Text = Convert.ToString(finalpointscost);
        }

        private void onClick_beginOrder(object sender, System.EventArgs e)
        {
            final_cart_cost = Convert.ToDouble(_calcTotalLabelPrice.Text.Substring(1));
            usersponsorpoints = _user.getPoints();
            if (usersponsorpoints < finalpointscost)
            {
                _orderMessage.Text = "Order failed! Not enough points. You only have " + usersponsorpoints + " points.";
            }
            else
            {
                SubmitOrder();
                _orderMessage.Text = "Order submitted!";
                _submitOrder.Hide();
            }
            _orderMessage.Show();
        }

        private void SubmitOrder()
        {
            UpdateOrderinDB();
            UpdatePlacesOrderinDB();
            UpdatePlacedForinDB();
            UpdateUserSponsorPoints();

        }
        private int GetMaxOrderNums()
        {
            int maxordernum = 0;
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "SELECT MAX(Order_Number) AS maxnum FROM Orders;";
            List<string> targets = new List<string>();
            List<string> parms = new List<string>();
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return 1000;
            }
            if (rdr.Read())
            {
                maxordernum = rdr.GetInt32("maxnum");
            }
            rdr.Close();
            return maxordernum;
        }
        private void UpdateOrderinDB()
        {
            order_number = GetMaxOrderNums() + 1;
            int SaleID_Applied;
            if (_sale_ID == 0 || _sale_ID == -1)
            {
                SaleID_Applied = 1;
            }
            else SaleID_Applied = _sale_ID;
            // Order_Number, SaleID_Applied
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "INSERT INTO Orders (Order_Number, SaleID_Applied) Values (@order_number, @SaleID_Applied);";
            List<string> targets = new List<string>();
            targets.Add("@order_number");
            targets.Add("@SaleID_Applied");
            List<string> parms = new List<string>();
            parms.Add(order_number.ToString());
            parms.Add(SaleID_Applied.ToString());
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);
        }

        private void UpdatePlacesOrderinDB()
        {
            // UserID, Order_Num --> both ints
            int user_ID = _user.getId();
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "INSERT INTO Places_Order Values (@user_ID, @order_number);";
            List<string> targets = new List<string>();
            targets.Add("@user_ID");
            targets.Add("@order_number");
            List<string> parms = new List<string>();
            parms.Add(user_ID.ToString());
            parms.Add(order_number.ToString());
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);
        }

        private void UpdatePlacedForinDB()
        {
            // Order_Number, Cart_ID, Total_Cost --> int, int, decimal
            int cart_ID = _user.getCart().getID();
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "INSERT INTO Placed_For Values (@order_number, @cart_ID, @final_cart_cost, NOW());";
            List<string> targets = new List<string>();
            targets.Add("@order_number");
            targets.Add("@cart_ID");
            targets.Add("@final_cart_cost");
            List<string> parms = new List<string>();
            parms.Add(order_number.ToString());
            parms.Add(cart_ID.ToString());
            parms.Add(final_cart_cost.ToString());
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);
        }

        private void UpdateUserSponsorPoints()
        {
            usersponsorpoints = _user.getPoints();
            int userID = _user.getId();
            int newpoints = usersponsorpoints - (int)finalpointscost;

            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "UPDATE Sponsored_By SET Points = @newpoints WHERE UserID = @userID AND SponsorID = @sponsorID;";
            List<string> targets = new List<string>();
            targets.Add("@newpoints");
            targets.Add("@userID");
            targets.Add("@sponsorID");
            List<string> parms = new List<string>();
            parms.Add(newpoints.ToString());
            parms.Add(userID.ToString());
            parms.Add(sponsorID.ToString());
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);
            usersponsorpoints = newpoints;
            _user.setPoints(newpoints);
        }
    }
}
