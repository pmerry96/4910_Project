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
    class LoginScreen : Form
    {
        Infinium _infinium;

        Label _loginTitle;
        Label _usernamePrompt;
        TextBox _usernameEntry;
        Label _passwordPrompt;
        TextBox _passwordEntry;
        Button _loginButton;
        Button _signupButton;

        Label _invalidLoginPrompt;

        User _userAcct;

        public LoginScreen(Form form)
        {
            _infinium = (Infinium) form;
            _infinium.Controls.Clear();
            _infinium.Text = "Infinium";

            _invalidLoginPrompt = new Label();

            _loginTitle = new Label();
            _loginTitle.Text = "Log In";
            _loginTitle.Location = new Point(_infinium.Width / 2 - _loginTitle.Width / 2, _infinium.Height / 3);
            

            _usernameEntry = new TextBox();
            _usernameEntry.Location = new Point(_infinium.Width / 2, _loginTitle.Bottom + 20);


            _usernamePrompt = new Label();
            _usernamePrompt.Text = "Username";
            _usernamePrompt.Location = new Point(_usernameEntry.Left - _usernamePrompt.Width - 20, _usernameEntry.Top);


            _passwordEntry = new TextBox();
            _passwordEntry.Location = new Point(_usernameEntry.Left, _usernameEntry.Bottom + 5);


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
        }

        public void DisplayLogin()
        {
            _infinium.Controls.Add(_loginTitle);
            _loginTitle.Show();

            _infinium.Controls.Add(_usernameEntry);
            _usernameEntry.Show();

            _usernamePrompt.Show();
            _infinium.Controls.Add(_usernamePrompt);

            _infinium.Controls.Add(_passwordEntry);
            _passwordEntry.Show();

            _infinium.Controls.Add(_passwordPrompt);
            _passwordPrompt.Show();

            _infinium.Controls.Add(_loginButton);
            _loginButton.Show();

            _infinium.Controls.Add(_signupButton);
            _signupButton.Show();
        }

        public void OnClick_LoginButton(object sender, System.EventArgs e)
        {
            _userAcct = new User(_usernameEntry.Text, _passwordEntry.Text);
            if (_userAcct.IsAuth())
            {
                _infinium._authenticatedUserAcct = _userAcct;
                _infinium.ShowAccountScreen();
            }
            else
            {
                _invalidLoginPrompt.Text = "Your Username or Password is invalid - please re enter your credentials";
                _invalidLoginPrompt.Size = new Size(_infinium.Width / 2, _invalidLoginPrompt.Height);
                _invalidLoginPrompt.Location = new Point(_infinium.Width / 2 - _invalidLoginPrompt.Width / 4, _signupButton.Bottom + 5);
                _infinium.Controls.Add(_invalidLoginPrompt);
                _invalidLoginPrompt.Show();
            }
        }



    }
}
