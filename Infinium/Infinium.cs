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
    public partial class Infinium : Form
    {
        Form _attached;

        private const int INFINIUM_WIDTH = 1280;
        private const int INFINIUM_HIEGHT = 720;

        private LoginScreen _loginScreen;
        private SignUpScreen _signUpScreen;
        private AccountScreen _accountScreen;


        public User _authenticatedUserAcct;

        public Infinium()
        {
            InitializeComponent();
            _attached = this;
            ShowLoginScreen();
        }

        public void ShowLoginScreen()
        {
            if(_loginScreen is null)
                _loginScreen = new LoginScreen(this);
            _loginScreen.DisplayLogin();
        }

        public void ShowAccountScreen()
        {
            if(_accountScreen is null)
                _accountScreen = new AccountScreen(_authenticatedUserAcct);
            _accountScreen.DisplayAccount();
        }

        public void ShowSignUpScreen()
        {
            //if (_signUpScreen is null)
                _signUpScreen = new SignUpScreen(this);
            _signUpScreen.DisplaySignUp();
        }

        public void OnClick_SignUpButton(object sender, System.EventArgs e)
        {
            this.Controls.Clear();
            ShowSignUpScreen();
        }

        public void OnClick_SignUpBackButton(object sender, System.EventArgs e)
        {
            this.Controls.Clear();
            ShowLoginScreen();
        }

        private void LoginScreen_Load(object sender, EventArgs e)
        { }
    }
}
