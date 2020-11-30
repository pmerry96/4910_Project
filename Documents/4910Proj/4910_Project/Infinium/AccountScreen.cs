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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Infinium
{
    public class AccountScreen : Form
    {
        Infinium _infinium;
        User _user;
        Label _welcomeTitle;
        Label _accountScreenTitle;
        Label _namePrompt;
        TextBox _nameEntry;
        Label _addressPrompt;
        TextBox _addressEntry;
        Label _emailPrompt;
        TextBox _emailEntry;
        Label _phonePrompt;
        TextBox _phoneEntry;
        Label _preferencesPrompt;
        ComboBox _preferencesEntry;
        Label _sponsorPrompt;
        ComboBox _sponsorSelect;
        Button _goToCatalog;
        Button _saveAccountChangesButton;
        //Button _goToProduct;
        Button _helpButton;

        Button _calcProfitButton;
        Label _calcProfitLabel;

        CheckBox _toggleDarkMode;

        Label _recentPurchasesTitle;
        ListBox _recentPurchasesListBox;
        List<Model.Product> _purchases;

        Label _specialOfferTitle;
        ListBox _specialOfferListBox;
        List<Model.Product> _specialOffers;

        Button _myCartButton;
        Label _userpointsLabel;

        Button _adminPageButton;
        Button _editCatalogButton;
        Button _createCouponButton;
        Button _viewUsersButton;
        Button _maintenanceButton;
        Button _viewAsDriverButton;

        Label _passwordPrompt;
        TextBox _passwordEntry;
        string[] _addresses;
        List<string> _usersSponsors;
        DBServerInstance _dbServerInstance;
        List<int> ordernums;
        List<int> saleids;

        public AccountScreen(Form form, User user)
        {
            _infinium = (Infinium) form;
            _user = user;
            ordernums = new List<int>();
            saleids = new List<int>();

            _welcomeTitle = new Label();
            _welcomeTitle.Text = "Welcome, " + _user.getName();
            _welcomeTitle.Location = new Point(5, 5);
            _infinium.Controls.Add(_welcomeTitle);

            _accountScreenTitle = new Label();
            _accountScreenTitle.Text = "Account Details";
            _accountScreenTitle.Location = new Point(_infinium.Width / 4 - _accountScreenTitle.Width / 2, _welcomeTitle.Bottom + 5);
            _infinium.Controls.Add(_accountScreenTitle);

            _namePrompt = new Label();
            _namePrompt.Text = "Name:";
            _namePrompt.Location = new Point(_accountScreenTitle.Left + (_accountScreenTitle.Width / 2 - _namePrompt.Width/2), _accountScreenTitle.Bottom + 10);
            _infinium.Controls.Add(_namePrompt);

            _nameEntry = new TextBox();
            _nameEntry.Text = _user.getName();
            _nameEntry.Location = new Point(_namePrompt.Right + 2, _namePrompt.Top);
            _infinium.Controls.Add(_nameEntry);

            _addressPrompt = new Label();
            _addressPrompt.Text = "Address: ";
            _addressPrompt.Location = new Point(_accountScreenTitle.Left + (_accountScreenTitle.Width / 2 - _addressPrompt.Width / 2), _namePrompt.Bottom + 5);
            _infinium.Controls.Add(_addressPrompt);

            _addressEntry = new TextBox();
            _addressEntry.Text = _user.getAddress();
            _addressEntry.Location = new Point(_addressPrompt.Right + 2, _addressPrompt.Top);
            _infinium.Controls.Add(_addressEntry);

            _emailPrompt = new Label();
            _emailPrompt.Text = "Email:";
            _emailPrompt.Location = new Point(_accountScreenTitle.Left + (_accountScreenTitle.Width / 2 - _addressPrompt.Width / 2), _addressPrompt.Bottom + 5);
            _infinium.Controls.Add(_emailPrompt);

            _emailEntry = new TextBox();
            _emailEntry.Text = _user.getEmail();
            _emailEntry.Location = new Point(_emailPrompt.Right + 2, _emailPrompt.Top);
            _infinium.Controls.Add(_emailEntry);
            
            _phonePrompt = new Label();
            _phonePrompt.Text = "Phone #:";
            _phonePrompt.Location = new Point(_accountScreenTitle.Left + (_accountScreenTitle.Width / 2 - _addressPrompt.Width / 2), _emailPrompt.Bottom + 5);
            _infinium.Controls.Add(_phonePrompt);

            _phoneEntry = new TextBox();
            _phoneEntry.Text = _user.getPhone();
            _phoneEntry.Location = new Point(_phonePrompt.Right + 2, _phonePrompt.Top);
            _infinium.Controls.Add(_phoneEntry);

            _preferencesPrompt = new Label();
            _preferencesPrompt.Text = "Notification Preference:";
            _preferencesPrompt.Location = new Point(_accountScreenTitle.Left + (_accountScreenTitle.Width / 2 - _addressPrompt.Width / 2), _phonePrompt.Bottom + 5);
            _infinium.Controls.Add(_preferencesPrompt);

            _preferencesEntry = new ComboBox();
            _preferencesEntry.Text = _user.getNotificationPreference();
            string[] options = new string[] { "Email", "Text" };
            _preferencesEntry.Items.AddRange(options);
            _preferencesEntry.Location = new Point(_preferencesPrompt.Right + 2, _preferencesPrompt.Top);
            _infinium.Controls.Add(_preferencesEntry);

            _sponsorPrompt = new Label();
            _sponsorPrompt.Text = "Sponsor";
            _sponsorPrompt.Location = new Point(_accountScreenTitle.Left + (_accountScreenTitle.Width / 2 - _addressPrompt.Width / 2), _preferencesPrompt.Bottom + 5);
            _infinium.Controls.Add(_sponsorPrompt);

            _sponsorSelect = new ComboBox();
            foreach (Sponsor sponsor in user.getSponsors())
            {
                _sponsorSelect.Items.Add(sponsor.GetName());
            }
            _sponsorSelect.Location = new Point(_sponsorPrompt.Right + 2, _sponsorPrompt.Top);
            _infinium.Controls.Add(_sponsorSelect);

            _goToCatalog = new Button();
            _goToCatalog.Text = "Catalog";
            _goToCatalog.Location = new Point(_sponsorSelect.Right + 10, _preferencesEntry.Bottom + 6);
            _infinium.Controls.Add(_goToCatalog);
            _goToCatalog.Click += OnClick_goToCatalog;

            _passwordPrompt = new Label();
            _passwordPrompt.Text = "Password:";
            _passwordPrompt.Location = new Point(_sponsorPrompt.Left, _sponsorPrompt.Bottom + 5);
            _infinium.Controls.Add(_passwordPrompt);

            _passwordEntry = new TextBox();
            _passwordEntry.Text = "";
            _passwordEntry.Location = new Point(_passwordPrompt.Right + 2, _passwordPrompt.Top);
            _passwordEntry.PasswordChar = '*';
            _infinium.Controls.Add(_passwordEntry);

            _saveAccountChangesButton = new Button();
            _saveAccountChangesButton.Text = "Save Changes";
            _saveAccountChangesButton.Location = new Point(_passwordEntry.Left, _passwordEntry.Bottom + 10);
            _infinium.Controls.Add(_saveAccountChangesButton);
            _saveAccountChangesButton.Click += OnClick_saveAccountChangesButton;

            _toggleDarkMode = new CheckBox();
            _toggleDarkMode.Text = "Dark Mode:";
            _toggleDarkMode.Location = new Point(_saveAccountChangesButton.Left + (_accountScreenTitle.Width / 2 - _addressPrompt.Width / 2), _saveAccountChangesButton.Bottom + 5);
            if (_infinium._defaultTheme._isDarkMode)
            {
                _toggleDarkMode.Checked = true;
            }
            else _toggleDarkMode.Checked = false;
            _infinium.Controls.Add(_toggleDarkMode);
            _toggleDarkMode.Click += OnClick__toggleDarkMode;

            _recentPurchasesTitle = new Label();
            _recentPurchasesTitle.Text = "Recent Purchases";
            //_recentPurchasesTitle.Font = new Font(_infinium.getFontName(), 20, FontStyle.Regular);
            _recentPurchasesTitle.Location = new Point((3 * _infinium.Width / 4) - _recentPurchasesTitle.Width /2, _accountScreenTitle.Top);
            _infinium.Controls.Add(_recentPurchasesTitle);

            _recentPurchasesListBox = new ListBox();
            //add the list it will display. this needs some hookups
            _recentPurchasesListBox.Location = new Point(_recentPurchasesTitle.Left, _recentPurchasesListBox.Bottom + 5);
            _infinium.Controls.Add(_recentPurchasesListBox);

            _specialOfferTitle = new Label();
            _specialOfferTitle.Text = "Special Offers";
            //_specialOfferTitle.Font = new Font(_infinium.getFontName(), 20, FontStyle.Regular);
            _specialOfferTitle.Location = new Point((3 * _infinium.Width / 4) - _recentPurchasesTitle.Width / 2, _recentPurchasesListBox.Bottom + 20);
            _infinium.Controls.Add(_specialOfferTitle);

            _specialOfferListBox = new ListBox();
            _specialOfferListBox.Location = new Point(_specialOfferTitle.Left, _specialOfferTitle.Bottom + 5);
            _infinium.Controls.Add(_specialOfferListBox);

            _myCartButton = new Button();
            _myCartButton.Text = "My Cart";
            _myCartButton.Location = new Point(_recentPurchasesTitle.Right + (_recentPurchasesTitle.Width / 2), _recentPurchasesTitle.Top);
            _infinium.Controls.Add(_myCartButton);
            _myCartButton.Click += OnClick__myCartButton;

            _helpButton = new Button();
            _helpButton.Text = "Help";
            _helpButton.Location = new Point(_myCartButton.Left, _myCartButton.Bottom + 5);
            _infinium.Controls.Add(_helpButton);
            _helpButton.Click += onClick_helpButton;

            _calcProfitButton = new Button();
            _calcProfitButton.Text = "Calc Profit";
            _calcProfitButton.Location = new Point(_myCartButton.Left, _myCartButton.Bottom + 50);
            _infinium.Controls.Add(_calcProfitButton);
            _calcProfitButton.Click += onClick__calcProfitButton;

            _calcProfitLabel = new Label();
            _calcProfitLabel.Text = "";
            //_specialOfferTitle.Font = new Font(_infinium.getFontName(), 20, FontStyle.Regular);
            _calcProfitLabel.Location = new Point(_myCartButton.Left, _myCartButton.Bottom + 75);
            _infinium.Controls.Add(_calcProfitLabel);

            /*
            _userpointsLabel = new Label();
            _userpointsLabel.Text = "Points: " + user.getPoints().ToString();
            _userpointsLabel.Location = new Point(_helpButton.Left, _helpButton.Bottom + 5);
            _infinium.Controls.Add(_userpointsLabel);
            */

            _adminPageButton = new Button();
            _adminPageButton.Text = "Admin Control Panel";
            _adminPageButton.Width = 150;
            _adminPageButton.Location = new Point(5, _specialOfferListBox.Bottom + 5);
            _adminPageButton.Click += onClick_adminPageButton;
            _infinium.Controls.Add(_adminPageButton);

            _editCatalogButton = new Button();
            _editCatalogButton.Text = "Edit Catalog";
            _editCatalogButton.Width = 150;
            _editCatalogButton.Location = new Point(155, _specialOfferListBox.Bottom + 5);
            _editCatalogButton.Click += onClick_editCatalogButton;
            _infinium.Controls.Add(_editCatalogButton);

            _createCouponButton = new Button();
            _createCouponButton.Text = "Coupon";
            _createCouponButton.Width = 150;
            _createCouponButton.Location = new Point(305, _specialOfferListBox.Bottom + 5);
            _createCouponButton.Click += onClick_createCouponButton;
            _infinium.Controls.Add(_createCouponButton);

            _viewUsersButton = new Button();
            _viewUsersButton.Text = "View Users";
            _viewUsersButton.Width = 150;
            _viewUsersButton.Location = new Point(5, _adminPageButton.Bottom + 5);
            _viewUsersButton.Click += onClick_viewUsersButton;
            _infinium.Controls.Add(_viewUsersButton);

            _maintenanceButton = new Button();
            _maintenanceButton.Text = "Maintenance";
            _maintenanceButton.Width = 150;
            _maintenanceButton.Location = new Point(155, _adminPageButton.Bottom + 5);
            _maintenanceButton.Click += onClick_maintenanceButton;
            _infinium.Controls.Add(_maintenanceButton);

            if(_user.isSponsorAccount() || _user.isAdmin())
            {
                _viewAsDriverButton = new Button();
                _viewAsDriverButton.Text = "View As Driver";
                _viewAsDriverButton.Location = new Point(_maintenanceButton.Left , _maintenanceButton.Bottom + 5);
                _viewAsDriverButton.Click += onClick_ViewAsDriverButton;
                _infinium.Controls.Add(_viewAsDriverButton);
            }

        }

        public void onClick_ViewAsDriverButton(object sender, System.EventArgs e)
        {
            int sponsorID = 1;
            if (_sponsorSelect.Text != String.Empty)
                sponsorID = Int32.Parse(_sponsorSelect.Text.ToString());
            var dbcon = DBServerInstance.Instance();
            

            string password = "infiniumpassword";
            var sha = SHA256.Create();
            string pass = _passwordEntry.Text;
            


            string emailpt1, emailpt2;
            var rand = new Random((int) DateTime.Now.Ticks);
            int randi = rand.Next();
            emailpt1 = "drivertest" + randi.ToString();
            emailpt2 = "@test.com";
            string email = emailpt1 + emailpt2;
            string result = HashHandler.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(password + email)));
            string msgboxstring = "Please record the following information for logging in as a Driver\n" +
                "Email =" + email + "\n" +
                "Password = infiniumpassword";

            MessageBox.Show(msgboxstring);
            string query = "INSERT INTO Users (Name, Email, Password) VALUES (@name, @email, @result);";
            List<string> targets = new List<string>();
            targets.Add("@name");
            targets.Add("@email");
            targets.Add("@result");
            List<string> parms = new List<string>();
            parms.Add("Testname");
            parms.Add(email);
            parms.Add(result);
            dbcon.ExecuteParameterizedQuery(query, targets, parms, false);

            string query2 = "SELECT UserID FROM Users WHERE Email = @email;";
            List<string> targets2 = new List<string>();
            targets2.Add("@email");
            List<string> parms2 = new List<string>();
            parms2.Add(email);
            var rdr = dbcon.ExecuteParameterizedQuery(query2, targets2, parms2, true);
            int userID = 0;
            if(rdr == null)
            {
                return;
            }
            while(rdr.Read())
            {
                userID = rdr.GetInt32("UserID");
            }
            rdr.Close();

            string query3 = "SELECT * FROM Driver;";
            rdr = dbcon.ExecuteQuery(query3, true);
            int maxdriverID = 0;
            int tmpID = maxdriverID;
            int i = 0;
            while(rdr.Read())
            {
                tmpID = rdr.GetInt32(1);
                if (tmpID > maxdriverID)
                    maxdriverID = tmpID;
            }
            rdr.Close();
            maxdriverID++;

            string query4 = "INSERT INTO Driver (Driver_ID, UserID, Sponsor_ID) VALUES(@driverID, @userID, @sponsorID);";
            List<string> targets4 = new List<string>();
            targets4.Add("@driverID");
            targets4.Add("@userID");
            targets4.Add("@sponsorID");
            List<string> parms4 = new List<string>();
            parms4.Add(maxdriverID.ToString());
            parms4.Add(userID.ToString());
            parms4.Add(sponsorID.ToString());
            dbcon.ExecuteParameterizedQuery(query4, targets4, parms4, false);

            string query5 = "INSERT INTO Sponsored_By (UserID, SponsorID, Points) VALUES (@UserID, @SponsorID, @Points);";
            List<string> targets5 = new List<string>();
            targets5.Add("@UserID");
            targets5.Add("@sponsorID"); 
            targets5.Add("@Points");
            List<string> parms5 = new List<string>();
            parms5.Add(userID.ToString());
            parms5.Add(sponsorID.ToString());
            parms5.Add("20000");
            dbcon.ExecuteParameterizedQuery(query5, targets5, parms5, false);
            var m = new Infinium();
            m.Show();
            m.createdUsers.Add(email);
        }

      

        public void onClick_adminPageButton(object sender, System.EventArgs e)
        {
            _infinium.ShowAdminPage();
        }
        public void onClick_editCatalogButton(object sender, System.EventArgs e)
        {
            _infinium.ShowEditCatalogScreen();
        }

        public void onClick_createCouponButton(object sender, System.EventArgs e)
        {
            _infinium.ShowCouponScreen();
        }

        public void onClick_viewUsersButton(object sender, System.EventArgs e)
        {
            _infinium.ShowUsersScreen();
        }

        public void onClick__calcProfitButton(object sender, System.EventArgs e)
        {
            string selectedName = _sponsorSelect.Text;
            Sponsor sponsor = _infinium._authenticatedUserAcct.GetSponsor(selectedName);

            if (sponsor == null)
                return;

            _infinium._authenticatedUserAcct.setSelectedSponsor(sponsor.GetName());

            //SELECT Order_Number FROM (Orders AS O RIGHT JOIN Places_Order AS P ON O.Order_Number = P.Order_Num) LEFT JOIN Sponsored_By AS S ON P.UserID = S.UserID WHERE S.SponsorID = 2;
            var dbcon = DBServerInstance.Instance();
            string query = "SELECT Order_Number FROM (Orders AS O RIGHT JOIN Places_Order AS P ON O.Order_Number = P.Order_Num) LEFT JOIN Sponsored_By AS S ON P.UserID = S.UserID WHERE S.SponsorID = @sponsorId";
            List<string> targets = new List<string>();
            targets.Add("@sponsorId");
            List<string> parms = new List<string>();
            parms.Add(_user.getSelectedSponsor().GetId().ToString());
            var rdr = dbcon.ExecuteParameterizedQuery(query, targets, parms, true);
            List<int> orderNums = new List<int>();
            while (rdr.Read())
            {
                orderNums.Add(rdr.GetInt32("Order_Number"));
            }
            rdr.Close();

            double totalCost = 0;
            foreach (int orderNum in orderNums)
            {
                Console.WriteLine(orderNum);
                string query2 = "SELECT Total_Cost FROM Placed_For WHERE Order_Number = @orderNum;";
                List<string> targets2 = new List<string>();
                targets2.Add("@orderNum");
                List<string> parms2 = new List<string>();
                parms2.Add(orderNum.ToString());
                var rdr2 = dbcon.ExecuteParameterizedQuery(query2, targets2, parms2, true);

                while (rdr2.Read())
                {
                    totalCost += rdr2.GetInt32("Total_Cost");
                }

                rdr2.Close();
            }

            double profit = totalCost * 0.01;
            _calcProfitLabel.Text = "Profit: $" + profit;
        }

        public void onClick_helpButton(object sender, System.EventArgs e)
        {
            _infinium.ShowHelp();
        }
        public void onClick_maintenanceButton(object sender, System.EventArgs e)
        {
            Notification.Instance().GlobalNotification("Planned maintenance tonight!");
            MessageBox.Show("Announcement sent!");
        }

        public void DisplayAccount()
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
            _welcomeTitle.Show();
            _accountScreenTitle.Show();
            _namePrompt.Show();
            _nameEntry.Show();
            _toggleDarkMode.Show();
            
            _addressPrompt.Show();
            _addressEntry.Show();
            
            _emailPrompt.Show();
            _emailEntry.Show();
            
            _phonePrompt.Show();
            _phoneEntry.Show();

            _sponsorPrompt.Show();
            _sponsorSelect.Show();
            _preferencesPrompt.Show();
            _preferencesEntry.Show();
            _saveAccountChangesButton.Show();
            _goToCatalog.Show();
            _recentPurchasesTitle.Show();
            _recentPurchasesListBox.Show();
            _specialOfferTitle.Show();
            _specialOfferListBox.Show();
            _myCartButton.Show();
            //_userpointsLabel.Show();
            _helpButton.Show();
            _passwordEntry.Show();
            _passwordPrompt.Show();

            if (!_user.isSponsorAccount() && !_user.isAdmin())
            {
                _editCatalogButton.Hide();
                _createCouponButton.Hide();
                _viewUsersButton.Hide();
                _adminPageButton.Hide();
                _maintenanceButton.Hide();
            }

            if(_user.isSponsorAccount() || _user.isAdmin())
            {
                _viewAsDriverButton.Show();
            }

            DisplayRecentPurchases();
            DisplaySpecialOffers();
        }
        /*
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
        */

        public void OnClick_saveAccountChangesButton(object sender, System.EventArgs e){
            var dbcon = DBServerInstance.Instance();
            bool validpassword = false;
            var sha = SHA256.Create();
            string pass = _passwordEntry.Text;
            string result = HashHandler.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(_passwordEntry.Text + _user.getEmail())));
            string query = "SELECT Password FROM Users WHERE UserID = @userid";
            List<string> targets = new List<string>();
            targets.Add("@userid");
            List<string> parms = new List<string>();
            parms.Add(_user.getId().ToString());
            var rdr = dbcon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                MessageBox.Show("Due to an SQL injection attempt, changes were not retained");
                return;
            }
            while(rdr.Read())
            {
                string hashsalt = rdr.GetString("Password");
                if(result == hashsalt)
                {
                    validpassword = true;
                }
                else
                {
                    validpassword = false;
                }
            }
            rdr.Close();
            if(validpassword)
            {
                int changesMade = 0;
                int addrChange = 0;
                if (_addressEntry.Text != _user.getAddress())
                {
                    if (!addressVerify(_addressEntry.Text))
                    {
                        MessageBox.Show("Please format addresses in this style: Street Address, City, State, ZIP");
                        return;
                    }
                    _user.setAddress(_addressEntry.Text);
                    addrChange++;
                    changesMade++;
                }
                if (_user.getName() != _nameEntry.Text)
                {
                    _user.setName(_nameEntry.Text);
                    changesMade++;
                }
                if (_user.getEmail() != _emailEntry.Text)
                {
                    string newHash = HashHandler.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(_passwordEntry.Text + _emailEntry.Text)));
                    int id = _user.getId();
                    string query2 = "UPDATE Users SET Password = @newHash WHERE UserID = @userid";
                    List<string> targets2 = new List<string>();
                    targets2.Add("@newHash");
                    targets2.Add("@userid");
                    List<string> parms2 = new List<string>();
                    parms2.Add(newHash);
                    parms2.Add(_user.getId().ToString());
                    dbcon.ExecuteParameterizedQuery(query2, targets2, parms2, false);
                    _user.setEmail(_emailEntry.Text);
                    changesMade++;
                }
                if (_user.getPhone() != _phoneEntry.Text)
                {
                    _user.setPhone(_phoneEntry.Text);
                    changesMade++;
                }
                if (_user.getNotificationPreference() != _preferencesEntry.Text)
                {
                    _user.setNotificationPreference(_preferencesEntry.Text);
                    changesMade++;
                }
                /*
                if(!_usersSponsors.Contains(_sponsorSelect.Text)) //TODO - change this to work through user class
                {
                    //add the users new sponsor
                    string query = "INSERT INTO Sponsored_By VALUES (" + _user.getId() + "," + _sponsorSelect.Text + ")";
                    _dbServerInstance.ExecuteQuery(query, false);
                }
                */
                if (changesMade != 0)
                {
                    var dbCon = DBServerInstance.Instance();
                    if (dbCon.IsConnect())
                    {
                        string myQuery = "UPDATE Users SET Name = \"" + _user.getName() + "\", Email = \"" + _user.getEmail() + "\", Phone_Num = \"" + _user.getPhone() + "\", Notif_Preference = \"" + _user.getNotificationPreference() + "\" WHERE UserID = " + _user.getId();

                        var cmd = new MySqlCommand(myQuery, DBServerInstance.Instance().Connection);
                        cmd.ExecuteNonQuery();
                        if (addrChange != 0)
                        {
                            //This needs to be a parameterized statement.
                            string myQuery2 = "UPDATE Users SET Street_Num = \"" + _user.getStreetNum() + "\", Street = \"" + _user.getStreet() +
                                                    "\", City = \"" + _user.getCity() + "\", State = \"" + _user.getState() + "\", Zip_Code = \"" + _user.getZip_Code() +
                                                    "\" WHERE UserID = " + _user.getId(); 
                            var cmd2 = new MySqlCommand(myQuery, DBServerInstance.Instance().Connection);

                            cmd2.ExecuteNonQuery();
                        }

                    }
                    _infinium.ShowAccountScreen();
                }
                _infinium._dbServerInstance.Close();
            }
            else
            {
                var box = MessageBox.Show("You did not enter a valid password to edit the information");
            }
            dbcon.Close();
        }

        public void OnClick_goToCatalog(object sender, System.EventArgs e)
        {
            string selectedName = _sponsorSelect.Text;
            Sponsor sponsor = _infinium._authenticatedUserAcct.GetSponsor(selectedName);

            if (sponsor == null)
                return;

            _infinium._authenticatedUserAcct.setSelectedSponsor(sponsor.GetName());
            _infinium.ShowCatalogScreen();
        }

        public void OnClick__myCartButton(object sender, System.EventArgs e)
        {
            _infinium.ShowCartScreen(sender, e);
        }
        private bool addressVerify(string address)
        {
            string[] parsed = address.Split(',');
            if (parsed.Length != 4)
            {
                return false;
            }
            string[] strAddressParse = parsed[0].Split(' ');
            if (strAddressParse.Length < 2)
            {
                return false;
            }
            int i = 0;
            if (!Int32.TryParse(strAddressParse[0], out i))
            {
                return false;
            }
            if (!Int32.TryParse(strAddressParse[3], out i))
            {
                return false;
            }
            return true;
        }

        public void OnClick__toggleDarkMode(object sender, System.EventArgs e)
        {
            _infinium._defaultTheme.switchTheme();
            _infinium.ShowAccountScreen();
        }

        private void DisplayRecentPurchases()
        {
            int cart_ID = _user.getCart().getID();
            int order_number;
            decimal total_cost;

            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "SELECT * FROM Placed_For WHERE Cart_ID = @cart_ID;";
            List<string> targets = new List<string>();
            targets.Add("@cart_ID");
            List<string> parms = new List<string>();
            parms.Add(cart_ID.ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            while (rdr.Read())
            {
                order_number = rdr.GetInt32("Order_Number");
                total_cost = rdr.GetInt32("Total_Cost");
                string order = "Order: " + order_number + " = $" + total_cost;
                ordernums.Add(order_number);
                _recentPurchasesListBox.Items.Add(order);
            }
            rdr.Close();
        }

        private void DisplaySpecialOffers()
        {
            foreach (int ordernum in ordernums)
            {
                int sale_ID;
                DBServerInstance dbCon = DBServerInstance.Instance();
                string query = "SELECT * FROM Orders WHERE Order_Number = @ordernum;";
                List<string> targets = new List<string>();
                targets.Add("@ordernum");
                List<string> parms = new List<string>();
                parms.Add(ordernum.ToString());
                MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
                if (rdr == null)
                {
                    return;
                }
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(1))
                    {
                        sale_ID = rdr.GetInt32("SaleID_Applied");
                    }
                    else
                    {
                        rdr.Close();
                        return;
                    }
                    if (!saleids.Contains(sale_ID))
                    {
                        saleids.Add(sale_ID);
                    }
                }
                rdr.Close();
            }

            foreach (int IDs in saleids)
            {
                AddToSpecialOffers(IDs);
            }
        }

        private void AddToSpecialOffers(int saleID)
        {
            if (saleID == 1)
            {
                return;
            }
            string code;
            DBServerInstance dbCon = DBServerInstance.Instance();
            string query = "SELECT * FROM Sale WHERE Sale_ID = @saleID;";
            List<string> targets = new List<string>();
            targets.Add("@saleID");
            List<string> parms = new List<string>();
            parms.Add(saleID.ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (rdr.Read())
            {
                code = rdr.GetString("Code");
                _specialOfferListBox.Items.Add(code);
            }
            rdr.Close();
        }
    }
}
