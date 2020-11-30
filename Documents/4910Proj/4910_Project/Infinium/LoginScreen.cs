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
using System.Net;
using System.Net.Mail;

namespace Infinium
{
    public class LoginScreen : Form
    {
        Infinium _infinium;

        Label _loginTitle;
        Label _emailPrompt;
        TextBox _emailEntry;
        Label _passwordPrompt;
        TextBox _passwordEntry;
        Button _loginButton;
        Button _signupButton;
        Button _resetButton;

        Label _invalidLoginPrompt;

        User _userAcct;

        public LoginScreen(Form form)
        {
            _infinium = (Infinium)form;
            _infinium.Controls.Clear();
            _infinium.Text = "Infinium";

            _invalidLoginPrompt = new Label();

            _loginTitle = new Label();
            _loginTitle.Text = "Log In";
            _loginTitle.Location = new Point(_infinium.Width / 2 - _loginTitle.Width / 2, _infinium.Height / 3);


            _emailEntry = new TextBox();
            _emailEntry.Location = new Point(_infinium.Width / 2, _loginTitle.Bottom + 20);


            _emailPrompt = new Label();
            _emailPrompt.Text = "Email";
            _emailPrompt.Location = new Point(_emailEntry.Left - _emailPrompt.Width - 20, _emailEntry.Top);


            _passwordEntry = new TextBox();
            _passwordEntry.Location = new Point(_emailEntry.Left, _emailEntry.Bottom + 5);
            _passwordEntry.PasswordChar = '*';

            _passwordPrompt = new Label();
            _passwordPrompt.Text = "Password";
            _passwordPrompt.Location = new Point(_passwordEntry.Left - _passwordPrompt.Width - 20, _passwordEntry.Top);


            _loginButton = new Button();
            _loginButton.Text = "Log In";
            _loginButton.Location = new Point(_passwordEntry.Left + ((_passwordEntry.Width - _loginButton.Width) / 2), _passwordEntry.Bottom + 5);
            _loginButton.Click += OnClick_LoginButton;


            _signupButton = new Button();
            _signupButton.Text = "Sign Up";
            _signupButton.Location = new Point(_loginButton.Left, _loginButton.Bottom + 5);
            _signupButton.Click += _infinium.OnClick_SignUpButton;

            _resetButton = new Button();
            _resetButton.Text = "Reset";
            _resetButton.Location = new Point(_passwordPrompt.Left, _passwordPrompt.Bottom + 5);
            _resetButton.Click += OnClick_ResetButton;

            _infinium.Controls.Add(_loginTitle);
            _infinium.Controls.Add(_emailEntry);
            _infinium.Controls.Add(_emailPrompt);
            _infinium.Controls.Add(_passwordEntry);
            _infinium.Controls.Add(_passwordPrompt);
            _infinium.Controls.Add(_loginButton);
            _infinium.Controls.Add(_signupButton);
            _infinium.Controls.Add(_resetButton);
        }

        public void DisplayLogin()
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
            _loginTitle.Show();
            _emailEntry.Show();
            _emailPrompt.Show();
            _passwordEntry.Show();
            _passwordPrompt.Show();
            _loginButton.Show();
            _signupButton.Show();
            _resetButton.Show();
        }

        public void OnClick_LoginButton(object sender, System.EventArgs e)
        {
            string mySalt = _emailEntry.Text;
            var sha = SHA256.Create();
            string result = HashHandler.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(_passwordEntry.Text + mySalt)));
            _passwordEntry.Text = "";
            if (string.IsNullOrEmpty(_emailEntry.Text) || string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Please fill out the entire form!");
                return;
            }

            LoginButton(_emailEntry.Text, result);
        }


        private void LoginButton(string email, string password)
        {

            //TODO - move the connection to a database instance and execution of a prepared query to a method
            var dbCon = DBServerInstance.Instance();
            //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT * FROM Users WHERE Email = '" + email + "' and password = '" + password + "'", true); //TODO - replace with parameterized sql statement
            string query = "SELECT * FROM Users WHERE Email = @email AND password = @password";
            List<string> targets = new List<string>();
            targets.Add("@email");
            targets.Add("@password");
            List<string> parms = new List<string>();
            parms.Add(email);
            parms.Add(password);
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);

            if (rdr == null)
            {
                return;
            }
            //MessageBox.Show(password);
            if (rdr.Read())
            {
                if (!rdr.GetBoolean("Is_Suspended"))
                {
                    int parm1 = rdr.GetInt32("UserID");
                    string parm2 = rdr.GetString("Email");
                    string parm3 = rdr.GetString("Name");
                    string parm4 = rdr.GetString("Password");
                    string parm5 = rdr.GetString("Notif_Preference");
                    string parm6 = rdr.IsDBNull(3) ? null : rdr.GetString("Phone_Num");
                    string parm7 = rdr.IsDBNull(7) ? null : rdr.GetString("Street_Num");
                    string parm8 = rdr.IsDBNull(8) ? null : rdr.GetString("Street");
                    string parm9 = rdr.IsDBNull(9) ? null : rdr.GetString("City");
                    string parm10 = rdr.IsDBNull(10) ? null : rdr.GetString("State");
                    string parm11 = rdr.IsDBNull(11) ? null : rdr.GetString("Zip_Code");
                    _infinium._authenticatedUserAcct = new User(parm1, parm2, parm3, parm4, parm5, parm6, parm7, parm8, parm9, parm10, parm11);
                    rdr.Close();

                    _infinium._authenticatedUserAcct.UpdateSponsors();
                    _infinium._authenticatedUserAcct.CheckSponsor();
                    _infinium.ShowAccountScreen();
                }
                else
                {
                    MessageBox.Show("User is currently suspended.");
                }
            }
            else
            {
                MessageBox.Show("Invalid email or password!");
            }

            rdr.Close();
        }

        private void OnClick_ResetButton(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(_emailEntry.Text))
            {
                MessageBox.Show("Please input an email.");
                return;
            }
            if (!EmailExists(_emailEntry.Text))
            {
                MessageBox.Show("That email does not exist.");
                return;
            }
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("infiniumclemson@gmail.com", "InfiniumAdmin"),
                EnableSsl = true,
            };

            string recipient = _emailEntry.Text;
            var random = new Random();
            string _randomID = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                _randomID = String.Concat(_randomID, random.Next(10).ToString());
            }

            string body = "Your password reset number is: " + _randomID;

            smtpClient.Send("infiniumclemson@gmail.com", recipient, "Infinium Password Reset", body);

            _infinium.ShowEmailVerifyScreen(recipient, _randomID);
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
                return false;
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
    }
}
