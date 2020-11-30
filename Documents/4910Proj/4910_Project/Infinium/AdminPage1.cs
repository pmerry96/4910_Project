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
    public class AdminPage : Form
    {
        Infinium _infinium;
        Button _backButton;
        Label _driverPurchaseReport;
        TextBox _sponsorEntry;
        Button _bySponsor;
        TextBox _driverEntry;
        Button _byDriver;
        TextBox _dateEntry;
        Button _byDate;
        ListBox _dataReturnListbox;
        Label _suspendLabel;
        TextBox _emailEntry;
        Button _isUserSuspended;
        Button _suspendUser;
        Button _unsuspendUser;
        Label _updateBox;
        Button _driverbySponsorButton;
        Label _driverbySponsorLabel;
        ComboBox _selectsponsorBox;
        Button _makeSponsorButton;
        Button _makeAdminButton;

        Button _addSponsorButton;
        ComboBox _sponsorToAdd;
        TextBox _startingPoints;

        public AdminPage(Form form)
        {
            _infinium = (Infinium)form;

            _backButton = new Button();
            _backButton.Text = "Back";
            _backButton.Location = new Point(5, 5);
            _backButton.Click += onClick_BackButton;
            _infinium.Controls.Add(_backButton);

            _driverPurchaseReport = new Label();
            _driverPurchaseReport.Text = "Sales Reports";
            _driverPurchaseReport.Location = new Point(5, _backButton.Bottom + 10);
            _infinium.Controls.Add(_driverPurchaseReport);

            _dateEntry = new TextBox();
            _dateEntry.Text = "Month MM/YYYY";
            _dateEntry.Location = new Point(_driverPurchaseReport.Left, _driverPurchaseReport.Bottom + 5);
            _infinium.Controls.Add(_dateEntry);

            _byDate = new Button();
            _byDate.Text = "By Date";
            _byDate.Location = new Point(_driverPurchaseReport.Left, _dateEntry.Bottom + 5);
            _byDate.Click += onClick_byDate;
            _infinium.Controls.Add(_byDate);

            _sponsorEntry = new TextBox();
            _sponsorEntry.Text = "SponsorID Here";
            _sponsorEntry.Location = new Point(_driverPurchaseReport.Left, _byDate.Bottom + 5);
            _infinium.Controls.Add(_sponsorEntry);

            _bySponsor = new Button();
            _bySponsor.Text = "By Sponsor";
            _bySponsor.Location = new Point(_driverPurchaseReport.Left, _sponsorEntry.Bottom + 5);
            _bySponsor.Click += onClick_bySponsor;
            _infinium.Controls.Add(_bySponsor);

            _driverEntry = new TextBox();
            _driverEntry.Text = "Driver Email Here";
            _driverEntry.Location = new Point(_driverPurchaseReport.Left, _bySponsor.Bottom + 5);
            _infinium.Controls.Add(_driverEntry);

            _byDriver = new Button();
            _byDriver.Text = "By Driver";
            _byDriver.Location = new Point(_driverPurchaseReport.Left, _driverEntry.Bottom + 5);
            _byDriver.Click += onClick_byDriver;
            _infinium.Controls.Add(_byDriver);

            _dataReturnListbox = new ListBox();
            _dataReturnListbox.Location = new Point(_backButton.Right + 40, 5);
            _dataReturnListbox.Size = new Size(_infinium.Size.Width / 2, _infinium.Size.Height / 2);
            _infinium.Controls.Add(_dataReturnListbox);

            _suspendLabel = new Label();
            _suspendLabel.Text = "Email:";
            _suspendLabel.Location = new Point(_dataReturnListbox.Right + 2, 25);
            _infinium.Controls.Add(_suspendLabel);

            _emailEntry = new TextBox();
            _emailEntry.Location = new Point(_suspendLabel.Right + 2, _suspendLabel.Top);
            _infinium.Controls.Add(_emailEntry);

            _updateBox = new Label();
            _updateBox.Text = "";
            _updateBox.Location = new Point(_suspendLabel.Left, _suspendLabel.Bottom + 2);
            _infinium.Controls.Add(_updateBox);

            _isUserSuspended = new Button();
            _isUserSuspended.Text = "Check";
            _isUserSuspended.Location = new Point(_suspendLabel.Left, _updateBox.Bottom + 2);
            _isUserSuspended.Click += onClick_isUserSuspended;
            _infinium.Controls.Add(_isUserSuspended);

            _suspendUser = new Button();
            _suspendUser.Text = "Suspend";
            _suspendUser.Location = new Point(_isUserSuspended.Right + 2, _updateBox.Bottom + 2);
            _suspendUser.Click += onClick_suspendUser;
            _infinium.Controls.Add(_suspendUser);

            _unsuspendUser = new Button();
            _unsuspendUser.Text = "Unsuspend";
            _unsuspendUser.Location = new Point(_suspendUser.Right + 2, _updateBox.Bottom + 2);
            _unsuspendUser.Click += onClick_unsuspendUser;
            _infinium.Controls.Add(_unsuspendUser);

            _driverbySponsorButton = new Button();
            _driverbySponsorButton.Text = "Drivers";
            _driverbySponsorButton.Location = new Point(_dataReturnListbox.Left, _dataReturnListbox.Bottom + 5);
            _driverbySponsorButton.Click += onClick_DriverBySponsor;
            _infinium.Controls.Add(_driverbySponsorButton);

            _driverbySponsorLabel = new Label();
            _driverbySponsorLabel.Text = "Sponsored by";
            _driverbySponsorLabel.Location = new Point(_driverbySponsorButton.Right + 5, _driverbySponsorButton.Top);
            _infinium.Controls.Add(_driverbySponsorLabel);

            List<string> sponsors = getAllSponsors();

            _selectsponsorBox = new ComboBox();
            _selectsponsorBox.DataSource = sponsors;
            _selectsponsorBox.Location = new Point(_driverbySponsorLabel.Right + 5, _driverbySponsorLabel.Top);
            _infinium.Controls.Add(_selectsponsorBox);

            _addSponsorButton = new Button();
            _addSponsorButton.Text = "Add Sponsor";
            _addSponsorButton.Location = new Point(_isUserSuspended.Left, _isUserSuspended.Bottom + 5);
            _addSponsorButton.Click += onClick_AddSponsorButton;
            _infinium.Controls.Add(_addSponsorButton);

            _sponsorToAdd = new ComboBox();
            _sponsorToAdd.DataSource = sponsors;
            _sponsorToAdd.Text = sponsors.First();
            _sponsorToAdd.Location = new Point(_addSponsorButton.Right + 5, _addSponsorButton.Top);
            _infinium.Controls.Add(_sponsorToAdd);

            _startingPoints = new TextBox();
            _startingPoints.Text = "Points To Start With";
            _startingPoints.Location = new Point(_addSponsorButton.Left, _addSponsorButton.Bottom + 5);
            _infinium.Controls.Add(_startingPoints);

            _makeSponsorButton = new Button();
            _makeSponsorButton.Text = "Add Sponsor";
            _makeSponsorButton.Location = new Point(_startingPoints.Left, _startingPoints.Bottom + 5);
            _makeSponsorButton.Click += onClick_MakeSponsorButton;
            _infinium.Controls.Add(_makeSponsorButton);

            _makeAdminButton = new Button();
            _makeAdminButton.Text = "Add Sponsor";
            _makeAdminButton.Location = new Point(_makeSponsorButton.Right + 5, _makeSponsorButton.Top);
            _makeAdminButton.Click += onClick_MakeAdminButton;
            _infinium.Controls.Add(_makeAdminButton);
        }

        private List<string> getAllSponsors()
        {
            List<string> sponsors = new List<string>();

            string query = "SELECT * FROM Sponsor";
            var rdr = DBServerInstance.Instance().ExecuteParameterizedQuery(query, null, null, true);
            if (rdr == null)
            {
                return sponsors;
            }
            else
            {
                while (rdr.Read())
                {
                    sponsors.Add(rdr.GetInt32("Sponsor_ID").ToString());
                }
            }
            rdr.Close();
            return sponsors;

        }

        public void DisplayAdminPage()
        {
            _backButton.Show();
            _driverPurchaseReport.Show();
            _sponsorEntry.Show();
            _bySponsor.Show();
            _driverEntry.Show();
            _byDriver.Show();
            _dateEntry.Show();
            _byDate.Show();
            _dataReturnListbox.Show();
            _suspendLabel.Show();
            _emailEntry.Show();
            _isUserSuspended.Show();
            _suspendUser.Show();
            _unsuspendUser.Show();
            _updateBox.Show();
            _driverbySponsorButton.Show();
            _driverbySponsorLabel.Show();
            _selectsponsorBox.Show();
            _addSponsorButton.Show();
            _sponsorToAdd.Show();
            _startingPoints.Show();
            _makeSponsorButton.Show();
            _makeAdminButton.Show();
        }

        private void onClick_AddSponsorButton(object sender, EventArgs e)
        {
            var dbcon = DBServerInstance.Instance();
            string query = "SELECT * FROM Users WHERE Email=@email";
            List<string> targets = new List<string>();
            targets.Add("@email");
            List<string> parms = new List<string>();
            parms.Add(_emailEntry.Text);
            var rdr = dbcon.ExecuteParameterizedQuery(query, targets, parms, true);
            int id = 0;
            if (rdr == null)
            {
                return;
            }
            else
            {
                while (rdr.Read())
                {
                    id = rdr.GetInt32("UserID");
                }
            }
            rdr.Close();
            string query2 = "INSERT INTO Sponsored_By VALUES(@UserID, @SponsorID, @Points);";
            List<string> targets2 = new List<string>();
            targets2.Add("@UserID");
            targets2.Add("@SponsorID");
            targets2.Add("@Points");
            List<string> parms2 = new List<string>();
            parms2.Add(id.ToString());
            parms2.Add(_selectsponsorBox.Text);
            int points = 0;
            if (_startingPoints.Text != "Points To Start With" || _startingPoints.Text != "")
            {
                points = int.Parse(_startingPoints.Text);
            }
            parms2.Add(points.ToString());
            dbcon.ExecuteParameterizedQuery(query2, targets2, parms2, false);
        }

        private void onClick_BackButton(object sender, EventArgs e)
        {
            _infinium.ShowAccountScreen();
        }

        private void onClick_DriverBySponsor(object sender, EventArgs e)
        {
            List<string> driversforSponsor = new List<string>();
            var dbcon = DBServerInstance.Instance();
            string query = "SELECT * FROM Users AS U RIGHT JOIN Sponsored_By AS S ON U.UserID = S.UserID WHERE S.SponsorID = @sponsorID;";
            List<string> targets = new List<string>();
            targets.Add("@sponsorID");
            List<string> parms = new List<string>();
            parms.Add(_selectsponsorBox.Text);
            var rdr = dbcon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            while (rdr.Read())
            {
                driversforSponsor.Add(rdr.GetString("Name") + " " + rdr.GetInt32("UserID") + " " + rdr.GetString("Email"));
            }
            _dataReturnListbox.DataSource = driversforSponsor;
            _infinium.Controls.Add(_dataReturnListbox);
            _dataReturnListbox.Refresh();
        }

        private void onClick_bySponsor(object sender, EventArgs e)
        {

            var dbcon = DBServerInstance.Instance();
            if (_dateEntry.Text == "Month MM/YYYY" || _dateEntry.Text == "")
            {
                MessageBox.Show("Please enter a date", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Int32.TryParse(_sponsorEntry.Text, out _))
            {
                MessageBox.Show("Please enter a proper sponsor ID", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] monthyear = _dateEntry.Text.Split('/');
            if (!Int32.TryParse(monthyear[0], out _) || monthyear[0].Length != 2)
            {
                MessageBox.Show("Use MM/YYYY Date Format Please", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Int32.TryParse(monthyear[1], out _) || monthyear[1].Length != 4)
            {
                MessageBox.Show("Use MM/YYYY Date Format Please", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dbcon.IsConnect())
            {
                string myQuery = "SELECT UserID FROM Users INNER JOIN Users ON Sponsor.UserID = Users.UserID WHERE Sponsor_ID = @sponsorID";
                List<string> parms = new List<string>();
                parms.Add(_sponsorEntry.Text);
                List<string> targets = new List<string>();
                targets.Add("@sponsorID");
                MySqlDataReader rdr1 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                if (rdr1 == null)
                {
                    return;
                }
                List<string> userlist = new List<string>();
                while (rdr1.Read())
                {
                    userlist.Add(rdr1.GetInt32("UserID").ToString());
                }
                rdr1.Close();

                var usersConv = new StringBuilder();
                string last = userlist.Last();
                foreach (string s in userlist)
                {
                    if (s.Equals(last))
                    {
                        usersConv.Append("'" + s + "'");
                    }
                    else
                    {
                        usersConv.Append("'" + s + "'" + ",");
                    }
                }
                myQuery = "SELECT Order_Num FROM Places_Order WHERE UserID IN (@usersList)";
                parms = new List<string>();
                parms.Add(usersConv.ToString());
                targets = new List<string>();
                targets.Add("@usersList");
                MySqlDataReader rdr2 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> orderidlist = new List<string>();
                if (rdr2 == null)
                {
                    return;
                }
                while (rdr2.Read())
                {
                    orderidlist.Add(rdr2.GetInt32("Order_Number").ToString());
                }
                rdr2.Close();

                var orderidConv = new StringBuilder();
                last = orderidlist.Last();
                foreach (string s in orderidlist)
                {
                    if (s.Equals(last))
                    {
                        orderidConv.Append("'" + s + "'");
                    }
                    else
                    {
                        orderidConv.Append("'" + s + "'" + ",");
                    }
                }
                myQuery = "SELECT Cart_ID FROM Placed_For WHERE Purchase_Date BETWEEN '@year1-@month1-01' AND '@year2-@month2-31' AND Order_Number IN (@orderidList)";
                parms = new List<string>();
                parms.Add(monthyear[1]);
                parms.Add(monthyear[0]);
                parms.Add(monthyear[1]);
                parms.Add(monthyear[0]);
                parms.Add(orderidConv.ToString());
                targets = new List<string>();
                targets.Add("@year1");
                targets.Add("@month1");
                targets.Add("@year2");
                targets.Add("@month2");
                targets.Add("@orderidList");
                MySqlDataReader rdr3 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> cartidlist = new List<string>();
                if (rdr3 == null)
                {
                    return;
                }
                while (rdr3.Read())
                {
                    cartidlist.Add(rdr3.GetInt32("Cart_ID").ToString());
                }
                rdr3.Close();

                var cartidConv = new StringBuilder();
                last = cartidlist.Last();
                foreach (string s in cartidlist)
                {
                    if (s.Equals(last))
                    {
                        cartidConv.Append("'" + s + "'");
                    }
                    else
                    {
                        cartidConv.Append("'" + s + "'" + ",");
                    }
                }
                myQuery = "SELECT Product_ID, Quantity FROM Cart_Contains WHERE Cart_ID IN (@cartidList)";
                parms = new List<string>();
                parms.Add(cartidConv.ToString());
                targets = new List<string>();
                targets.Add("@cartidList");
                MySqlDataReader rdr4 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> prodidlist = new List<string>();
                List<string> quantitylist = new List<string>();
                if (rdr4 == null)
                {
                    return;
                }
                while (rdr4.Read())
                {
                    prodidlist.Add(rdr4.GetInt32("Product_ID").ToString());
                    quantitylist.Add(rdr4.GetInt32("Quantity").ToString());
                }
                rdr4.Close();

                var prodidConv = new StringBuilder();
                last = prodidlist.Last();
                foreach (string s in prodidlist)
                {
                    if (s.Equals(last))
                    {
                        prodidConv.Append("'" + s + "'");
                    }
                    else
                    {
                        prodidConv.Append("'" + s + "'" + ",");
                    }
                }
                myQuery = "SELECT Name, Price FROM Product WHERE Product_ID IN (@prodidList)";
                parms = new List<string>();
                parms.Add(prodidConv.ToString());
                targets = new List<string>();
                targets.Add("@prodidList");
                MySqlDataReader rdr5 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> namelist = new List<string>();
                List<string> pricelist = new List<string>();
                if (rdr5 == null)
                {
                    return;
                }
                while (rdr5.Read())
                {
                    namelist.Add(rdr5.GetInt32("Name").ToString());
                    pricelist.Add(rdr5.GetInt32("Price").ToString());
                }
                rdr5.Close();

                List<string> finaloutput = new List<string>();
                for (int i = 0; i < namelist.Count; i++)
                {
                    finaloutput.Add(namelist[i] + " $" + pricelist[i] + " x" + quantitylist[i] + " = $" + (Int32.Parse(pricelist[i]) * Int32.Parse(quantitylist[i])));
                }

                _dataReturnListbox.DataSource = finaloutput;
                this.Controls.Add(_dataReturnListbox);
                _dataReturnListbox.Refresh();
            }
        }

        private void onClick_byDriver(object sender, EventArgs e)
        {
            var dbcon = DBServerInstance.Instance();
            List<string> finaloutput = new List<string>();

            string query = "SELECT * FROM Users WHERE Email = @email";
            List<string> targets = new List<string>();
            targets.Add("@email");
            List<string> parms = new List<string>();
            parms.Add(_driverEntry.Text);
            MySqlDataReader rdr = dbcon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (!rdr.Read())
            {
                MessageBox.Show("No such user", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Int32.TryParse(_sponsorEntry.Text, out _))
            {
                MessageBox.Show("Please enter a proper sponsor ID", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            rdr.Close();

            if (dbcon.IsConnect())
            {
                List<string> userlist = new List<string>();
                string myQuery = "SELECT UserID FROM Users INNER JOIN Users ON Sponsor.UserID = Users.UserID WHERE Sponsor_ID = @spondor AND UserID = @driver";
                parms = new List<string>();
                parms.Add(_sponsorEntry.Text);
                parms.Add(_driverEntry.Text);
                targets = new List<string>();
                targets.Add("@sponsor");
                targets.Add("@driver");
                MySqlDataReader rdr1 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                if (rdr1 == null)
                {
                    return;
                }
                while (rdr1.Read())
                {
                    userlist.Add(rdr1.GetInt32("UserID").ToString());
                }
                rdr1.Close();

                myQuery = "SELECT Order_Num FROM Places_Order WHERE UserID IN (@driver)";
                parms = new List<string>();
                parms.Add(userlist[0].ToString());
                targets = new List<string>();
                targets.Add("@driver");
                MySqlDataReader rdr2 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> orderidlist = new List<string>();
                if (rdr2 == null)
                {
                    return;
                }
                while (rdr2.Read())
                {
                    orderidlist.Add(rdr2.GetInt32("Order_Number").ToString());
                }
                rdr2.Close();

                var orderidConv = new StringBuilder();
                string last = orderidlist.Last();
                foreach (string s in orderidlist)
                {
                    if (s.Equals(last))
                    {
                        orderidConv.Append("'" + s + "'");
                    }
                    else
                    {
                        orderidConv.Append("'" + s + "'" + ",");
                    }
                }
                myQuery = "SELECT Cart_ID FROM Placed_For WHERE Order_Number IN (@orderidList)";
                parms = new List<string>();
                parms.Add(orderidConv.ToString());
                targets = new List<string>();
                targets.Add("@orderidList");
                MySqlDataReader rdr3 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> cartidlist = new List<string>();
                if (rdr3 == null)
                {
                    return;
                }
                while (rdr3.Read())
                {
                    cartidlist.Add(rdr3.GetInt32("Cart_ID").ToString());
                }
                rdr3.Close();

                var cartidConv = new StringBuilder();
                last = cartidlist.Last();
                foreach (string s in cartidlist)
                {
                    if (s.Equals(last))
                    {
                        cartidConv.Append("'" + s + "'");
                    }
                    else
                    {
                        cartidConv.Append("'" + s + "'" + ",");
                    }
                }
                myQuery = "SELECT Product_ID, Quantity FROM Cart_Contains WHERE Cart_ID IN (@cartidList)";
                parms = new List<string>();
                parms.Add(cartidConv.ToString());
                targets = new List<string>();
                targets.Add("@cartidList");
                MySqlDataReader rdr4 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> prodidlist = new List<string>();
                List<string> quantitylist = new List<string>();
                if (rdr3 == null)
                {
                    return;
                }
                while (rdr4.Read())
                {
                    prodidlist.Add(rdr4.GetInt32("Product_ID").ToString());
                    quantitylist.Add(rdr4.GetInt32("Quantity").ToString());
                }
                rdr4.Close();

                var prodidConv = new StringBuilder();
                last = cartidlist.Last();
                foreach (string s in prodidlist)
                {
                    if (s.Equals(last))
                    {
                        prodidConv.Append("'" + s + "'");
                    }
                    else
                    {
                        prodidConv.Append("'" + s + "'" + ",");
                    }
                }
                myQuery = "SELECT Name, Price FROM Product WHERE Product_ID IN (@prodidList)";
                parms = new List<string>();
                parms.Add(prodidConv.ToString());
                targets = new List<string>();
                targets.Add("@prodidList");
                MySqlDataReader rdr5 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> namelist = new List<string>();
                List<string> pricelist = new List<string>();
                if (rdr5 == null)
                {
                    return;
                }
                while (rdr5.Read())
                {
                    namelist.Add(rdr5.GetInt32("Name").ToString());
                    pricelist.Add(rdr5.GetInt32("Price").ToString());
                }
                rdr5.Close();

                for (int i = 0; i < namelist.Count; i++)
                {
                    finaloutput.Add(namelist[i] + " $" + pricelist[i] + " x" + quantitylist[i] + " = $" + (Int32.Parse(pricelist[i]) * Int32.Parse(quantitylist[i])));
                }

                _dataReturnListbox.DataSource = finaloutput;
                this.Controls.Add(_dataReturnListbox);
                _dataReturnListbox.Refresh();
            }
        }

        private void onClick_byDate(object sender, EventArgs e)
        {
            var dbcon = DBServerInstance.Instance();
            if (_dateEntry.Text == "Month MM/YYYY" || _dateEntry.Text == "")
            {
                MessageBox.Show("Please enter a date", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] monthyear = _dateEntry.Text.Split('/');
            if (!Int32.TryParse(monthyear[0], out _) || monthyear[0].Length != 2)
            {
                MessageBox.Show("Use MM/YYYY Date Format Please", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Int32.TryParse(monthyear[1], out _) || monthyear[1].Length != 4)
            {
                MessageBox.Show("Use MM/YYYY Date Format Please", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dbcon.IsConnect())
            {
                string myQuery = "";
                if (Int32.Parse(monthyear[0]) == 2 && Int32.Parse(monthyear[1]) % 4 != 0)
                {
                    myQuery = "SELECT Cart_ID FROM Placed_For WHERE Purchase_Date BETWEEN '@year1-@month1-01' AND '@year2-@month2-28'";
                }
                else if (Int32.Parse(monthyear[0]) == 2)
                {
                    myQuery = "SELECT Cart_ID FROM Placed_For WHERE Purchase_Date BETWEEN '@year1-@month1-01' AND '@year2-@month2-28'";
                }
                else if (Int32.Parse(monthyear[0]) % 2 == 0)
                {
                    Console.WriteLine("Even Month");
                    myQuery = "SELECT Cart_ID FROM Placed_For WHERE Purchase_Date BETWEEN '@year1-@month1-01' AND '@year2-@month2-31'";
                }
                else
                {
                    Console.WriteLine("Odd Month");
                    myQuery = "SELECT Cart_ID FROM Placed_For WHERE Purchase_Date BETWEEN '@year1-@month1-01' AND '@year2-@month2-30'";
                }
                List<string> parms = new List<string>();
                parms.Add(monthyear[1]);
                parms.Add(monthyear[0]);
                parms.Add(monthyear[1]);
                parms.Add(monthyear[0]);
                List<string> targets = new List<string>();
                targets.Add("@year1");
                targets.Add("@month1");
                targets.Add("@year2");
                targets.Add("@month2");
                MySqlDataReader rdr3 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> cartidlist = new List<string>();
                if (rdr3 == null)
                {
                    return;
                }
                while (rdr3.Read())
                {
                    cartidlist.Add(rdr3.GetInt32("Cart_ID").ToString());
                }
                rdr3.Close();

                if (!cartidlist.Any())
                {
                    MessageBox.Show("No entries in month", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var cartidConv = new StringBuilder();
                string last = cartidlist.Last();
                foreach (string s in cartidlist)
                {
                    if (s.Equals(last))
                    {
                        cartidConv.Append("'" + s + "'");
                    }
                    else
                    {
                        cartidConv.Append("'" + s + "'" + ",");
                    }
                }
                myQuery = "SELECT Product_ID, Quantity FROM Cart_Contains WHERE Cart_ID IN (" + cartidConv.ToString() + ")";
                parms = new List<string>();
                parms.Add(cartidConv.ToString());
                Console.WriteLine(cartidConv.ToString());
                targets = new List<string>();
                targets.Add("@cartidList");
                MySqlDataReader rdr4 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> prodidlist = new List<string>();
                List<string> quantitylist = new List<string>();
                if (rdr4 == null)
                {
                    return;
                }
                while (rdr4.Read())
                {
                    prodidlist.Add(rdr4.GetInt32("Product_ID").ToString());
                    quantitylist.Add(rdr4.GetInt32("Quantity").ToString());
                }
                rdr4.Close();

                var prodidConv = new StringBuilder();
                last = prodidlist.Last();
                foreach (string s in prodidlist)
                {
                    if (s.Equals(last))
                    {
                        prodidConv.Append("'" + s + "'");
                    }
                    else
                    {
                        prodidConv.Append("'" + s + "'" + ",");
                    }
                }
                myQuery = "SELECT Name, Price FROM Product WHERE Product_ID IN (@prodidList)";
                parms = new List<string>();
                parms.Add(prodidConv.ToString());
                targets = new List<string>();
                targets.Add("@prodidList");
                MySqlDataReader rdr5 = dbcon.ExecuteParameterizedQuery(myQuery, targets, parms, true);
                List<string> namelist = new List<string>();
                List<string> pricelist = new List<string>();
                if (rdr5 == null)
                {
                    return;
                }
                while (rdr5.Read())
                {
                    namelist.Add(rdr5.GetInt32("Name").ToString());
                    pricelist.Add(rdr5.GetInt32("Price").ToString());
                }
                rdr5.Close();

                List<string> finaloutput = new List<string>();
                for (int i = 0; i < namelist.Count; i++)
                {
                    finaloutput.Add(namelist[i] + " $" + pricelist[i] + " x" + quantitylist[i] + " = $" + (Int32.Parse(pricelist[i]) * Int32.Parse(quantitylist[i])));
                }

                _dataReturnListbox.DataSource = finaloutput;
                this.Controls.Add(_dataReturnListbox);
                _dataReturnListbox.Refresh();
            }
        }

        private void onClick_isUserSuspended(object sender, EventArgs e)
        {
            var dbCon = DBServerInstance.Instance();
            //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT * FROM Users WHERE Email = '" + _emailEntry.Text + "'", true);
            string query = "SELECT * FROM Users WHERE Email = @email";
            List<string> targets = new List<string>();
            targets.Add("@email");
            List<string> parms = new List<string>();
            parms.Add(_emailEntry.Text);
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (rdr.Read())
            {
                if (rdr.GetByte("Is_Suspended") == 1)
                {
                    _updateBox.Text = "Is suspended.";
                }
                else
                {
                    _updateBox.Text = "Not suspended.";
                }
            }
            else
            {
                _updateBox.Text = "Email not in system.";
            }
            rdr.Close();
        }
        private void onClick_suspendUser(object sender, EventArgs e)
        {
            DBServerInstance.Instance();
            var dbCon = DBServerInstance.Instance();
            //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT * FROM Users WHERE Email = '" + _emailEntry.Text + "'", true);
            string query = "SELECT * FROM Users WHERE Email = @email";
            List<string> targets = new List<string>();
            targets.Add("@email");
            List<string> parms = new List<string>();
            parms.Add(_emailEntry.Text);
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (!rdr.Read())
            {
                _updateBox.Text = "Email not in system.";
            }
            rdr.Close();
            if (dbCon.IsConnect())
            {
                string myQuery = "UPDATE Users SET Is_Suspended = 1 WHERE Email = '@email'";
                parms = new List<string>();
                parms.Add(_emailEntry.Text);
                targets = new List<string>();
                targets.Add("@email");
                MySqlDataReader update = dbCon.ExecuteParameterizedQuery(myQuery, targets, parms, false);
            }
            dbCon.Close();
        }

        private void onClick_unsuspendUser(object sender, EventArgs e)
        {
            DBServerInstance.Instance();
            var dbCon = DBServerInstance.Instance();
            //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT * FROM Users WHERE Email = '" + _emailEntry.Text + "'", true);
            string query = "SELECT * FROM Users WHERE Email = @email";
            List<string> targets = new List<string>();
            targets.Add("@email");
            List<string> parms = new List<string>();
            parms.Add(_emailEntry.Text);
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (!rdr.Read())
            {
                _updateBox.Text = "Email not in system.";
            }
            rdr.Close();
            if (dbCon.IsConnect())
            {
                string myQuery = "UPDATE Users SET Is_Suspended = 0 WHERE Email = '@email'";
                parms = new List<string>();
                parms.Add(_emailEntry.Text);
                targets = new List<string>();
                targets.Add("@email");
                MySqlDataReader update = dbCon.ExecuteParameterizedQuery(myQuery, targets, parms, false);
            }
            dbCon.Close();
        }

        private void onClick_MakeSponsorButton(object sender, EventArgs e)
        {
            var dbCon = DBServerInstance.Instance();
            string myQuery = "UPDATE Users SET Permissions = 'Sponsor' WHERE Email = '@email'";
            List<string> parms = new List<string>();
            parms.Add(_emailEntry.Text);
            List<string> targets = new List<string>();
            targets.Add("@email");
            MySqlDataReader update = dbCon.ExecuteParameterizedQuery(myQuery, targets, parms, false);
            dbCon.Close();
        }

        private void onClick_MakeAdminButton(object sender, EventArgs e)
        {
            var dbCon = DBServerInstance.Instance();
            string myQuery = "UPDATE Users SET Permissions = 'Admin' WHERE Email = '@email'";
            List<string> parms = new List<string>();
            parms.Add(_emailEntry.Text);
            List<string> targets = new List<string>();
            targets.Add("@email");
            MySqlDataReader update = dbCon.ExecuteParameterizedQuery(myQuery, targets, parms, false);
            dbCon.Close();
        }
    }
}