using Infinium.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Infinium
{
    public class ViewUserScreen : Form
    {
        Infinium _infinium;
        UserInfo _user;
        Label _accountScreenTitle;
        Label _namePrompt;
        TextBox _nameEntry;
        Label _addressPrompt;
        TextBox _addressEntry;
        Label _emailPrompt;
        TextBox _emailEntry;
        Label _phonePrompt;
        TextBox _phoneEntry;
        Button _backToUsersButton;
        Button _removeUserButton;

        Label _recentPurchasesTitle;
        ListBox _recentPurchasesListBox;

        Label _specialOfferTitle;
        ListBox _specialOfferListBox;

        public ViewUserScreen(Form form, UserInfo user)
        {
            _infinium = (Infinium) form;
            _user = user;

            _accountScreenTitle = new Label();
            _accountScreenTitle.Text = user.GetName() + " Account Details";
            _accountScreenTitle.Location = new Point(_infinium.Width / 4 - _accountScreenTitle.Width / 2, 10);
            _infinium.Controls.Add(_accountScreenTitle);

            _namePrompt = new Label();
            _namePrompt.Text = "Name:";
            _namePrompt.Location = new Point(_accountScreenTitle.Left + (_accountScreenTitle.Width / 2 - _namePrompt.Width/2), _accountScreenTitle.Bottom + 10);
            _infinium.Controls.Add(_namePrompt);

            _nameEntry = new TextBox();
            _nameEntry.Text = _user.GetName();
            _nameEntry.Location = new Point(_namePrompt.Right + 2, _namePrompt.Top);
            _infinium.Controls.Add(_nameEntry);

            _addressPrompt = new Label();
            _addressPrompt.Text = "Address: ";
            _addressPrompt.Location = new Point(_accountScreenTitle.Left + (_accountScreenTitle.Width / 2 - _addressPrompt.Width / 2), _namePrompt.Bottom + 5);
            _infinium.Controls.Add(_addressPrompt);

            _addressEntry = new TextBox();
            _addressEntry.Text = "";
            _addressEntry.Location = new Point(_addressPrompt.Right + 2, _addressPrompt.Top);
            _infinium.Controls.Add(_addressEntry);

            _emailPrompt = new Label();
            _emailPrompt.Text = "Email:";
            _emailPrompt.Location = new Point(_accountScreenTitle.Left + (_accountScreenTitle.Width / 2 - _addressPrompt.Width / 2), _addressPrompt.Bottom + 5);
            _infinium.Controls.Add(_emailPrompt);

            _emailEntry = new TextBox();
            _emailEntry.Text = _user.GetEmail();
            _emailEntry.Location = new Point(_emailPrompt.Right + 2, _emailPrompt.Top);
            _infinium.Controls.Add(_emailEntry);
            
            _phonePrompt = new Label();
            _phonePrompt.Text = "Phone #:";
            _phonePrompt.Location = new Point(_accountScreenTitle.Left + (_accountScreenTitle.Width / 2 - _addressPrompt.Width / 2), _emailPrompt.Bottom + 5);
            _infinium.Controls.Add(_phonePrompt);

            _phoneEntry = new TextBox();
            _phoneEntry.Text = _user.GetPhone();
            _phoneEntry.Location = new Point(_phonePrompt.Right + 2, _phonePrompt.Top);
            _infinium.Controls.Add(_phoneEntry);

            _removeUserButton = new Button();
            _removeUserButton.Text = "Remove User";
            _removeUserButton.Location = new Point(_phoneEntry.Left, _phoneEntry.Bottom + 10);
            _infinium.Controls.Add(_removeUserButton);
            _removeUserButton.Click += OnClick_removeUserButton;

            _backToUsersButton = new Button();
            _backToUsersButton.Text = "View Users";
            _backToUsersButton.Location = new Point(_removeUserButton.Right + 15, _phoneEntry.Bottom + 10);
            _infinium.Controls.Add(_backToUsersButton);
            _backToUsersButton.Click += OnClick_viewUsersButton;

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
        }


        public void DisplayViewUser()
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
            _accountScreenTitle.Show();
            _namePrompt.Show();
            _nameEntry.Show();
            
            _addressPrompt.Show();
            _addressEntry.Show();
            
            _emailPrompt.Show();
            _emailEntry.Show();
            
            _phonePrompt.Show();
            _phoneEntry.Show();

            _recentPurchasesTitle.Show();
            _recentPurchasesListBox.Show();
            _specialOfferTitle.Show();
            _specialOfferListBox.Show();
            _backToUsersButton.Show();
        }

        public void OnClick_viewUsersButton(object sender, System.EventArgs e)
        {
            _infinium.ShowUsersScreen();
        }

        public void OnClick_removeUserButton(object sender, System.EventArgs e)
        {
            DBServerInstance conn = DBServerInstance.Instance();
            string query = "DELETE FROM Sponsored_By WHERE UserID = @userID AND SponsorID = @sponsorID";
            List<string> targets = new List<string>();
            targets.Add("@userID");
            targets.Add("@sponsorID");
            List<string> parms = new List<string>();
            parms.Add(_user.GetId().ToString());
            parms.Add(_infinium._authenticatedUserAcct.getSponsorInstance().GetId().ToString());

            conn.ExecuteParameterizedQuery(query, targets, parms, false);
            _infinium.ShowUsersScreen();
        }
    }
}
