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
using MySql.Data.MySqlClient;

namespace Infinium
{
    public class PasswordResetScreen : Form
    {
        Infinium _infinium;
        Label _pass1Prompt;
        TextBox _pass1Entry;
        Label _pass2Prompt;
        TextBox _pass2Entry;
        Button _saveButton;
        Button _backButton;
        Label _title;

        string _email;
        public PasswordResetScreen(Form form, string email)
        {
            _infinium = (Infinium)form;
            _email = email;

            _title = new Label();
            _title.Text = "Password Reset";
            _title.Location = new Point(_infinium.Width / 2 - _title.Width / 2, _infinium.Height / 3);


            _pass1Entry = new TextBox();
            _pass1Entry.Text = email;
            _pass1Entry.Location = new Point(_infinium.Width / 2, _title.Bottom + 20);


            _pass1Prompt = new Label();
            _pass1Prompt.Text = "Email";
            _pass1Prompt.Location = new Point(_pass1Entry.Left - _pass1Prompt.Width - 20, _pass1Entry.Top);


            _pass2Entry = new TextBox();
            _pass2Entry.Location = new Point(_pass1Entry.Left, _pass1Entry.Bottom + 5);


            _pass2Prompt = new Label();
            _pass2Prompt.Text = "Reset ID";
            _pass2Prompt.Location = new Point(_pass2Entry.Left - _pass2Prompt.Width - 20, _pass2Entry.Top);

            _saveButton = new Button();
            _saveButton.Text = "Save";
            _saveButton.Location = new Point(_pass2Entry.Left + ((_pass2Entry.Width - _saveButton.Width) / 2), _pass2Entry.Bottom + 5);
            _saveButton.Click += OnClick_saveButton;

            _backButton = new Button();
            _backButton.Text = "Reset";
            _backButton.Location = new Point(_pass2Prompt.Left, _pass2Prompt.Bottom + 5);
            _backButton.Click += OnClick_backButton;
        }
        public void DisplayPasswordReset()
        {
            _infinium.Controls.Add(_title);
            _title.Show();

            _infinium.Controls.Add(_pass1Entry);
            _pass1Entry.Show();

            _pass1Prompt.Show();
            _infinium.Controls.Add(_pass1Prompt);

            _infinium.Controls.Add(_pass2Entry);
            _pass2Entry.Show();

            _infinium.Controls.Add(_pass2Prompt);
            _pass2Prompt.Show();

            _infinium.Controls.Add(_saveButton);
            _saveButton.Show();
        }
        public void OnClick_saveButton(object sender, System.EventArgs e)
        {
            if (_pass1Entry.Text != _pass2Entry.Text)
            {
                MessageBox.Show("The passwords do not match.");
                return;
            }
            var dbCon = DBServerInstance.Instance();
            if (dbCon.IsConnect())
            {
                string myQuery = "UPDATE Users SET Password = \"" + _pass1Entry.Text + "\" WHERE Email = " + _email;

                var cmd = new MySqlCommand(myQuery, DBServerInstance.Instance().Connection);
                cmd.ExecuteNonQuery();
            }
            _infinium.ShowLoginScreen();
        }

        public void OnClick_backButton(object sender, System.EventArgs e)
        {
            _infinium.ShowLoginScreen();
        }
    }
}