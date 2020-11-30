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
using System.Security.Cryptography;

namespace Infinium
{
    public class SignUpScreen : Form
    {
        Infinium _infinium;

        Label _signUpTitle;
        Label _emailPrompt;
        TextBox _emailEntry;
        Label _namePrompt;
        TextBox _nameEntry;
        Label _sponsorIDPrompt;
        TextBox _sponsorIDEntry;
        Label _passwordPrompt;
        TextBox _passwordEntry;
        Label _confirmPasswordPrompt;
        TextBox _confirmPasswordEntry;

        Button _signUpButton;
        Button _backButton;

        public SignUpScreen(Form form)
        {
            _infinium = (Infinium) form;

            _signUpTitle = new Label();
            _signUpTitle.Text = "Sign Up";
            _signUpTitle.Location = new Point(_infinium.Width / 2 - _signUpTitle.Width/2, _infinium.Height / 4);
            _infinium.Controls.Add(_signUpTitle);

            _emailPrompt = new Label();
            _emailPrompt.Text = "Email Address";
            _emailPrompt.Location = new Point(_infinium.Width/2 - _emailPrompt.Width - 10, _signUpTitle.Bottom + 5);
            _infinium.Controls.Add(_emailPrompt);

            _emailEntry = new TextBox();
            _emailEntry.Location = new Point(_infinium.Width/2 + 10, _emailPrompt.Top);
            _infinium.Controls.Add(_emailEntry);

            _namePrompt = new Label();
            _namePrompt.Text = "Name";
            _namePrompt.Location = new Point(_infinium.Width/2 - _namePrompt.Width - 10, _emailPrompt.Bottom + 5);
            _infinium.Controls.Add(_namePrompt);

            _nameEntry = new TextBox();
            _nameEntry.Location = new Point(_infinium.Width/2 + 10, _namePrompt.Top);
            _infinium.Controls.Add(_nameEntry);

            _sponsorIDPrompt = new Label();
            _sponsorIDPrompt.Text = "Sponsor ID Number";
            _sponsorIDPrompt.Location = new Point(_infinium.Width/2 - _sponsorIDPrompt.Width - 10, _namePrompt.Bottom + 5);
            _infinium.Controls.Add(_sponsorIDPrompt);

            _sponsorIDEntry = new TextBox();
            _sponsorIDEntry.Location = new Point(_infinium.Width/2 + 10, _sponsorIDPrompt.Top);
            _infinium.Controls.Add(_sponsorIDEntry);

            _passwordPrompt = new Label();
            _passwordPrompt.Text = "Password";
            _passwordPrompt.Location = new Point(_infinium.Width / 2 - _passwordPrompt.Width - 10, _sponsorIDPrompt.Bottom + 5);
            _infinium.Controls.Add(_passwordPrompt);

            _passwordEntry = new TextBox();
            _passwordEntry.Location = new Point(_infinium.Width / 2 + 10, _passwordPrompt.Top);
            _infinium.Controls.Add(_passwordEntry);

            _confirmPasswordPrompt = new Label();
            _confirmPasswordPrompt.Text = "Confirm Password";
            _confirmPasswordPrompt.Location = new Point(_infinium.Width / 2 - _confirmPasswordPrompt.Width - 10, _passwordPrompt.Bottom + 5);
            _infinium.Controls.Add(_confirmPasswordPrompt);

            _confirmPasswordEntry = new TextBox();
            _confirmPasswordEntry.Location = new Point(_infinium.Width / 2 + 10, _confirmPasswordPrompt.Top);
            _infinium.Controls.Add(_confirmPasswordEntry);

            _signUpButton = new Button();
            _signUpButton.Text = "Signup";
            _signUpButton.Location = new Point(_infinium.Width / 2 - _signUpButton.Width / 2, _confirmPasswordPrompt.Bottom + 10);
            _signUpButton.Click += OnClick_SignUpButton;
            _infinium.Controls.Add(_signUpButton);

            _backButton = new Button();
            _backButton.Text = "Back";
            _backButton.Location = new Point(_infinium.Width/2 - _backButton.Width/2, _signUpButton.Bottom + 10);
            _backButton.Click += _infinium.OnClick_SignUpBackButton;
            _infinium.Controls.Add(_backButton);
        }

        public void DisplaySignUp()
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
            _signUpTitle.Show();
            _emailPrompt.Show();
            _emailEntry.Show();
            _namePrompt.Show();
            _nameEntry.Show();
            _sponsorIDPrompt.Show();
            _sponsorIDEntry.Show();
            _passwordPrompt.Show();
            _passwordEntry.Show();
            _confirmPasswordPrompt.Show();
            _confirmPasswordEntry.Show();
            _backButton.Show();
        }
        private void OnClick_SignUpButton(object sender, System.EventArgs e)
        { 
            if(string.IsNullOrWhiteSpace(_passwordEntry.Text) || string.IsNullOrWhiteSpace(_confirmPasswordEntry.Text))
            {
                MessageBox.Show("Please confirm your password by typing it in both the password and password confirm boxes");
                return;
            }

            string mySalt = _emailEntry.Text;
            var sha = SHA256.Create();
            byte[] result = sha.ComputeHash(Encoding.UTF8.GetBytes(_passwordEntry.Text + mySalt));
            string hashstr = HashHandler.ToHexString(result);
            _passwordEntry.Text = "";

            byte[] resultconfirm = sha.ComputeHash(Encoding.UTF8.GetBytes(_confirmPasswordEntry.Text + mySalt));
            _confirmPasswordEntry.Text = "";
            string hashstrconf = HashHandler.ToHexString(resultconfirm);

            if(!hashstr.Equals(hashstrconf))
            {
                MessageBox.Show("Your password and password condfirmation fields do not match, please re-enter them to confirm they are the same");
                return;
            }

            if (string.IsNullOrEmpty(_nameEntry.Text) || string.IsNullOrEmpty(_emailEntry.Text) || string.IsNullOrEmpty(_sponsorIDEntry.Text))
            {
                MessageBox.Show("Please fill out the entire form!");
                return;
            }

            if (EmailExists(_emailEntry.Text))
            {
                MessageBox.Show("That email already exists!");
                return;
            }

            if (!SponsorExists(_sponsorIDEntry.Text))
            {
                MessageBox.Show("A sponsor with that code doesn't exist!");
                return;
            }

            if (!PasswordsMatch(hashstr, hashstrconf))
            {
                MessageBox.Show("Your passwords don't match!");
                return;
            }

            SignupButton(_nameEntry.Text, _emailEntry.Text, _sponsorIDEntry.Text, hashstr);
        }

        public void SignupButton(string name, string email, string sponsorCode, string password)
        {
            var dbCon = DBServerInstance.Instance();
            //dbCon.ExecuteQuery("INSERT INTO Users (Name, Email, Password) VALUES ('" + name + "','" + email + "','" + password + "')", false);
            string query = "INSERT INTO Users (Name, Email, Password) VALUES (@name,@email,@password)";
            List<string> targets = new List<string>();
            targets.Add("@name");
            targets.Add("@email");
            targets.Add("@password");
            List<string> parms = new List<string>();
            parms.Add(name);
            parms.Add(email);
            parms.Add(password);
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);
            _infinium.ShowLoginScreen();
        }

        private bool EmailExists(string email)
        {
            var dbCon = DBServerInstance.Instance();
            //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT COUNT(*) FROM Users WHERE Email = '" + email + "'", true);
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @email";
            List<string> targets = new List<string>();
            targets.Add("@email");
            List<string> parms = new List<string>();
            parms.Add(email);
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return false; //SQL injection detected == email does not exist
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

        private bool SponsorExists(string sponsorCode)
        {
            var dbCon = DBServerInstance.Instance();
            //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT COUNT(*) FROM Sponsor WHERE Sponsor_ID = " + Int32.Parse(sponsorCode), true);
            string query = "SELECT COUNT(*) FROM Sponsor WHERE Sponsor_ID = @sponsorcode";
            List<string> targets = new List<string>();
            targets.Add("@sponsorcode");
            List<string> parms = new List<string>();
            parms.Add(Int32.Parse(sponsorCode).ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return false; //SQL injection detected == sponsor doesnt exist
            }
            if (rdr.Read())
            {
                if (Int32.Parse(rdr[0].ToString()) != 0)
                {
                    rdr.Close();

                    return true;
                }
            }
            rdr.Close();

            return false;
        }

        private bool PasswordsMatch(string pass, string confirmed)
        {
            Console.WriteLine(pass); //do we need this - could be a sec concern to print out the hash
            return pass.Equals(confirmed);
        }

    }
}
