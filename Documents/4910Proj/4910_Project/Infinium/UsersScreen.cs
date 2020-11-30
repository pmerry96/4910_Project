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
    public class UsersScreen : Form
    {

        Infinium infinium;
        Sponsor sponsor;

        Label usersTitle;
        ListBox usersListBox;
        Button returnToAccountButton;

        List<UserInfo> userInfo = new List<UserInfo>();

        public UsersScreen(Form form, Sponsor sponsor)
        {
            infinium = (Infinium)form;
            infinium.Controls.Clear();
            infinium.Text = "Infinium";

            this.sponsor = sponsor;

            usersTitle = new Label();
            usersTitle.Text = sponsor.GetName() + "'s Users";
            usersTitle.Location = new Point(5, 5);
            infinium.Controls.Add(usersTitle);

            returnToAccountButton = new Button();
            returnToAccountButton.Text = "Account";
            returnToAccountButton.Location = new Point((infinium.Width * 3 / 4) + 100, usersTitle.Top);
            infinium.Controls.Add(returnToAccountButton);
            returnToAccountButton.Click += OnClick_AccountButton;

            usersListBox = new ListBox();
            usersListBox.Location = new Point(usersTitle.Left, usersTitle.Bottom + 5);
            usersListBox.Size = new Size(infinium.Width * 3 / 4, infinium.Height * 3 / 5);
            infinium.Controls.Add(usersListBox);
            usersListBox.MouseDoubleClick += OnMouseDoubleClick_VisitUser;

            PopulateUsers();
        }

        public void DisplayUsers()
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

            usersTitle.Show();
            usersListBox.Show();
            returnToAccountButton.Show();
        }

        public void OnClick_AccountButton(object sender, System.EventArgs e)
        {
            infinium.ShowAccountScreen();
        }

        public void OnMouseDoubleClick_VisitUser(object sender, MouseEventArgs e)
        {
            int index = usersListBox.IndexFromPoint(e.Location);

            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                UserInfo info = userInfo[index];
                infinium.ShowViewUserScreen(info);
            }
        }

        private void PopulateUsers()
        {
            DBServerInstance dbCon = DBServerInstance.Instance();
            //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT p.Product_ID, p.Ebay_ID, p.Name, p.Price, p.Description, p.Display_IMG_Path FROM Product p JOIN Catalog_Contains cc ON p.Product_ID = cc.Product_ID JOIN Catalog c ON c.Catalog_ID = cc.Catalog_ID JOIN Fills_Catalog fc ON fc.Sponsor_ID = " + sponsor.GetId() + " AND fc.Catalog_ID = c.Catalog_ID WHERE fc.Sponsor_ID = " + sponsor.GetId(), true);

            string query = "SELECT u.UserID, u.Name, u.Phone_Num, u.Email, u.Notif_Preference FROM Users u JOIN Sponsored_By sb ON u.UserID = sb.UserID JOIN Sponsor s ON sb.SponsorID = s.Sponsor_ID WHERE s.Sponsor_ID = @sponsorID";
            List<string> targets = new List<string>();
            targets.Add("@sponsorID");
            List<string> parms = new List<string>();
            parms.Add(sponsor.GetId().ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if(rdr == null)
            {
                return;
            }
            while (rdr.Read())
            {
                UserInfo info = new UserInfo(rdr.GetInt32("UserID"), rdr.GetString("Email"), rdr.GetString("Name"), rdr.IsDBNull(2) ? null : rdr.GetString("Phone_Num"), null, null, null, null, null);
                userInfo.Add(info);
            }

            rdr.Close();

            foreach (UserInfo info in userInfo)
            {
                usersListBox.Items.Add(info.GetName());
            }
        }

    }
}