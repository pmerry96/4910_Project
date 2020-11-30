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
    public class CouponScreen : Form
    {

        Infinium infinium;
        Catalog catalog;
        Sponsor sponsor;

        Label catalogTitle;
        Button returnToAccountButton;

        Label dollarsOffLabel;
        TextBox dollarsOffText;

        Label minAmountLabel;
        TextBox minAmountText;

        Label couponLabel;
        TextBox couponText;

        Button createButton;

        public CouponScreen(Form form, User user)
        {
            infinium = (Infinium)form;
            infinium.Controls.Clear();
            infinium.Text = "Infinium";

            sponsor = user.getSponsorInstance();
            catalog = new Catalog(user, sponsor);

            catalogTitle = new Label();
            catalogTitle.Text = "Create Coupon";
            catalogTitle.Location = new Point(5, 5);
            infinium.Controls.Add(catalogTitle);

            returnToAccountButton = new Button();
            returnToAccountButton.Text = "Account";
            returnToAccountButton.Location = new Point((infinium.Width * 3 / 4) + 100, catalogTitle.Top);
            infinium.Controls.Add(returnToAccountButton);
            returnToAccountButton.Click += OnClick_AccountButton;

            dollarsOffLabel = new Label();
            dollarsOffLabel.Text = "Dollars Off:";
            dollarsOffLabel.Location = new Point((infinium.Width / 2) - dollarsOffLabel.Width, (infinium.Height / 2) - 180);
            infinium.Controls.Add(dollarsOffLabel);

            dollarsOffText = new TextBox();
            dollarsOffText.Location = new Point(dollarsOffLabel.Right + 2, dollarsOffLabel.Top);
            infinium.Controls.Add(dollarsOffText);

            minAmountLabel = new Label();
            minAmountLabel.Text = "Minimum Amount:";
            minAmountLabel.Location = new Point((infinium.Width / 2) - minAmountLabel.Width, dollarsOffLabel.Bottom + 25);
            infinium.Controls.Add(minAmountLabel);

            minAmountText = new TextBox();
            minAmountText.Location = new Point(minAmountLabel.Right + 2, minAmountLabel.Top);
            infinium.Controls.Add(minAmountText);

            couponLabel = new Label();
            couponLabel.Text = "Coupon:";
            couponLabel.Location = new Point((infinium.Width / 2) - couponLabel.Width, minAmountLabel.Bottom + 25);
            infinium.Controls.Add(couponLabel);

            couponText = new TextBox();
            couponText.Location = new Point(couponLabel.Right + 2, couponLabel.Top);
            infinium.Controls.Add(couponText);

            createButton = new Button();
            createButton.Text = "Create";
            createButton.Location = new Point((infinium.Width / 2) - (createButton.Width / 2), (infinium.Height / 2) - 25);
            infinium.Controls.Add(createButton);
            createButton.Click += OnClick__CreateCoupon;
        }

        public void DisplayCoupon()
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
            returnToAccountButton.Show();
            dollarsOffLabel.Show();
            dollarsOffText.Show();
            minAmountLabel.Show();
            minAmountText.Show();
            couponLabel.Show();
            couponText.Show();
            createButton.Show();
        }

        public void OnClick_AccountButton(object sender, System.EventArgs e)
        {
            infinium.ShowAccountScreen();
        }

        public void OnClick__CreateCoupon(object sender, System.EventArgs e)
        {
            string dollarsOff = dollarsOffText.Text;
            string minAmount = minAmountText.Text;
            string coupon = couponText.Text;

            double dollarsOffDouble;
            double minAmountDouble;

            if (!double.TryParse(dollarsOff, out dollarsOffDouble))
            {
                MessageBox.Show("Incorrect dollars off amount!");
                return;
            }

            if (!double.TryParse(minAmount, out minAmountDouble))
            {
                MessageBox.Show("Incorrect minimum amount!");
                return;
            }

            if (CouponExists(coupon))
            {
                MessageBox.Show("That coupon already exists!");
                return;
            }

            createSaleRecord(coupon, minAmountDouble);
            int id = getSaleId(coupon);

            if (id == -1)
                return;

            createDirectSaleRecord(id, dollarsOffDouble);
            
            infinium.ShowAccountScreen();
            Notification.Instance().SponsorNotification(sponsor, "Your sponsor " + sponsor.GetName() + " created a $" + dollarsOff + " coupon for their catalog! Code: " + coupon);
            MessageBox.Show("Coupon code '" + coupon + "' created!");
        }

        private bool CouponExists(string coupon)
        {
            var dbCon = DBServerInstance.Instance();

            string query = "SELECT COUNT(*) FROM Sale WHERE Code = @coupon";
            List<string> targets = new List<string>();
            targets.Add("@coupon");
            List<string> parms = new List<string>();
            parms.Add(coupon);
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return false; //coupon doesnt exist if injection
            }
            if (rdr.Read())
            {
                if (Int32.Parse(rdr[0].ToString()) == 0)
                {
                    rdr.Close();
                    return false;
                }
            }

            rdr.Close();

            return true;
        }

        private void createSaleRecord(string coupon, double minAmount)
        {
            var dbCon = DBServerInstance.Instance();
            string query = "INSERT INTO Sale (Code, Min_AMT) VALUES (@code,@minAmount)";
            List<string> targets = new List<string>();
            targets.Add("@code");
            targets.Add("@minAmount");
            List<string> parms = new List<string>();
            parms.Add(coupon);
            parms.Add(minAmount.ToString());
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);
        }

        private int getSaleId(string coupon)
        {
            var dbCon = DBServerInstance.Instance();
            string query = "SELECT * FROM Sale WHERE Code = @code";
            List<string> targets = new List<string>();
            targets.Add("@code");
            List<string> parms = new List<string>();
            parms.Add(coupon);
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return -1;
            }
            if (rdr.Read())
            {
                int id = rdr.GetInt32("Sale_ID");
                rdr.Close();
                return id;
            }

            rdr.Close();
            return -1;
        }

        private void createDirectSaleRecord(int saleId, double dollarsOff)
        {
            var dbCon = DBServerInstance.Instance();
            string query = "INSERT INTO Direct_Sale (Dollars_off, Sale_ID) VALUES (@dollarsOff,@saleId)";
            List<string> targets = new List<string>();
            targets.Add("@dollarsOff");
            targets.Add("@saleId");
            List<string> parms = new List<string>();
            parms.Add(dollarsOff.ToString());
            parms.Add(saleId.ToString());
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);
        }

    }
}