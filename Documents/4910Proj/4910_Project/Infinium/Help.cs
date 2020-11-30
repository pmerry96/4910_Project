using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infinium.Model;

namespace Infinium
{
    class Help : Form
    {
        Infinium _infinium;
        User _user;

        Label _title;
        Button _goToProduct;
        Button _accountButton;
        Button _myCartButton;
        Label _userpointsLabel;
        Label _adminTitle;
        Label _adminNum;
        Label _adminEmail;
        Label _sponsorTitle;
        Label _sponsorNum;
        public Help(Form form, User user)
        {
            _infinium = (Infinium)form;
            _user = user;

            _title = new Label();
            _title.Text = "Help Page";
            _title.Location = new Point(_infinium.Width / 2 - _title.Width / 2, _infinium.Height / 6);
            _infinium.Controls.Add(_title);

            _myCartButton = new Button();
            _myCartButton.Text = "My Cart";
            _myCartButton.Location = new Point(4 * _infinium.Width / 5 + 60, 35);
            _infinium.Controls.Add(_myCartButton);
            _myCartButton.Click += OnClick__myCartButton;

            _goToProduct = new Button();
            _goToProduct.Text = "Product";
            _goToProduct.Location = new Point(_myCartButton.Left, _myCartButton.Bottom + 5);
            _infinium.Controls.Add(_goToProduct);
            _goToProduct.Click += onClick_productPageButton;

            _accountButton = new Button();
            _accountButton.Text = "Account";
            _accountButton.Location = new Point(_goToProduct.Left, _goToProduct.Bottom + 5);
            _infinium.Controls.Add(_accountButton);
            _accountButton.Click += onClick_accountButton;

            _userpointsLabel = new Label();
            _userpointsLabel.Text = "Points: " + user.getPoints().ToString();
            _userpointsLabel.Location = new Point(_goToProduct.Left, _accountButton.Bottom + 5);
            _infinium.Controls.Add(_userpointsLabel);

            _adminTitle = new Label();
            _adminTitle.Text = "Admin Contact Info";
            _adminTitle.Location = new Point(_infinium.Width / 3 - 60, _infinium.Height / 3);
            _infinium.Controls.Add(_adminTitle);

            _adminNum = new Label();
            _adminNum.AutoSize = true;
            _adminNum.Text = "Number: (864) 867-5309";
            _adminNum.Location = new Point(_adminTitle.Left, _adminTitle.Bottom + 5);
            _infinium.Controls.Add(_adminNum);

            _adminEmail = new Label();
            _adminEmail.AutoSize = true;
            _adminEmail.Text = "Email: Infinium@gmail.com";
            _adminEmail.Location = new Point(_adminNum.Left, _adminNum.Bottom + 5);
            _infinium.Controls.Add(_adminEmail);

            _sponsorTitle = new Label();
            _sponsorTitle.Text = "Sponsor Help Line";
            _sponsorTitle.Location = new Point(2 * _infinium.Width / 3 - 60, _infinium.Height / 3);
            _infinium.Controls.Add(_sponsorTitle);

            _sponsorNum = new Label();
            _sponsorNum.AutoSize = true;
            _sponsorNum.Text = "Number: (864) 123-0987";
            _sponsorNum.Location = new Point(_sponsorTitle.Left, _sponsorTitle.Bottom + 5);
            _infinium.Controls.Add(_sponsorNum);
        }
        public void DisplayHelp()
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
            _title.Show();
            _goToProduct.Show();
            _accountButton.Show();
            _myCartButton.Show();
            _userpointsLabel.Show();
            _adminTitle.Show();
            _adminNum.Show();
            _adminEmail.Show();
            _sponsorTitle.Show();
            _sponsorNum.Show();
        }
        public void onClick_productPageButton(object sender, System.EventArgs e)
        {
            //_infinium.ShowProductScreen();
        }

        public void onClick_accountButton(object sender, System.EventArgs e)
        {
            _infinium.ShowAccountScreen();
        }
        public void OnClick__myCartButton(object sender, System.EventArgs e)
        {
            _infinium.ShowCartScreen(sender, e);
        }
    }
}
