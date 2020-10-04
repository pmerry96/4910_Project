using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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
            _loginScreen = new LoginScreen(this);
            _loginScreen.DisplayLogin();
        }

        public void ShowAccountScreen()
        {
            _accountScreen = new AccountScreen(_authenticatedUserAcct);
            _accountScreen.DisplayAccount();
        }

        public void ShowSignUpScreen()
        {
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
        { 
            //dont delete this - its not used but the program needs it becuase some back back backend stuff references it
            // and it only needs a pointer to the function - not any real functionality
        }
    }
}
