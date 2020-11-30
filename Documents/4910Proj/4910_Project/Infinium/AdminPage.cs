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
    public class AdminPage: Form
    {
        Infinium _infinium;
        Button _backButton;
        Button _driverPurchaseReportButton;
        ListBox _dataReturnListbox;
        public AdminPage(Form form)
        {
            _infinium = (Infinium) form;
            _backButton = new Button();
            _backButton.Text = "Back";
            _backButton.Location = new Point(5,5);
            _backButton.Click += onClick_BackButton;
            _infinium.Controls.Add(_backButton);

            _driverPurchaseReportButton = new Button();
            _driverPurchaseReportButton.Text = "Generate Purchase Report";
            _driverPurchaseReportButton.Location = new Point(5, _backButton.Bottom + 10);
            _driverPurchaseReportButton.Click += onClick_driverPurchaseReportButton;
            _infinium.Controls.Add(_driverPurchaseReportButton);

            _dataReturnListbox = new ListBox();
            _dataReturnListbox.Location = new Point(_backButton.Right + 10, 5);
            _dataReturnListbox.Size = new Size(_infinium.Size.Width/2, _infinium.Size.Height/2);
            _infinium.Controls.Add(_dataReturnListbox);
        }

        public void DisplayAdminPage()
        {
            _backButton.Show();
            _driverPurchaseReportButton.Show();
            _dataReturnListbox.Show();
        }

        private void onClick_BackButton(object sender, EventArgs e)
        {
            _infinium.ShowAccountScreen();
        }

        private void onClick_driverPurchaseReportButton(object sender, EventArgs e)
        {
            List<string> purchaseslist = new List<string>(); 
            var dbcon = DBServerInstance.Instance();
            if(dbcon.IsConnect())
            {
                string myQuery = "SELECT * FROM Placed_For";
                MySqlDataReader rdr = dbcon.ExecuteQuery(myQuery, true);
                while(rdr.Read())
                {
                    purchaseslist.Add(rdr.GetInt32("Order_Number").ToString() + " " + rdr.GetInt32("Cart_ID").ToString() + " " + rdr.GetDouble("Total_Cost").ToString());
                }
                _dataReturnListbox.DataSource = purchaseslist;
                //this.Controls.Add(_dataReturnListbox);
                _dataReturnListbox.Refresh();
            }
        }
    }
}
