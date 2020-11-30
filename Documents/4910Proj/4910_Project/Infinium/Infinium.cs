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

/*
 * Phil is just saving this for later
 */


namespace Infinium
{
    public partial class Infinium : Form
    {
        public DBServerInstance _dbServerInstance;
        private const int INFINIUM_WIDTH = 1280;
        private const int INFINIUM_HIEGHT = 720;
        private string INFINIUM_FONT = "Arial";

        private LoginScreen     _loginScreen;
        private SignUpScreen    _signUpScreen;
        private AccountScreen   _accountScreen;
        private CartScreen      _cartScreen;
        private EmailVerifyScreen _emailVerifyScreen;
        private PasswordResetScreen _passwordResetScreen;
        private CouponScreen _couponScreen;
        private AdminPage _adminPage;
        private CatalogScreen _catalogScreen;
        private EditCatalogScreen _editCatalogScreen;
        private ProductScreen _productScreen;
        private UsersScreen _usersScreen;
        private ViewUserScreen _viewUserScreen;
        private Help _help;

        public User _authenticatedUserAcct;

        public Theme _defaultTheme;

        public List<string> createdUsers;
        public string getFontName()
        {
            return INFINIUM_FONT;
        }

        public Infinium()
        {
            this.FormClosing += On_FormClosing;
            createdUsers = new List<string>();
            _dbServerInstance = new DBServerInstance();
            this.Size = new Size(INFINIUM_WIDTH, INFINIUM_HIEGHT);
            _defaultTheme = new Theme(false);
            InitializeComponent();
            ShowLoginScreen();
        }

        public void ShowLoginScreen()
        {
            _loginScreen = new LoginScreen(this);
            _loginScreen.DisplayLogin();
        }
        public void ShowHelp()
        {
            this.Controls.Clear();
            _help = new Help(this, _authenticatedUserAcct);
            _help.DisplayHelp();
        }

        public void ShowViewUserScreen(UserInfo user)
        {
            if (!_authenticatedUserAcct.isSponsorAccount())
                return;

            this.Controls.Clear();
            _viewUserScreen = new ViewUserScreen(this, user);
            _viewUserScreen.DisplayViewUser();
        }

        public void ShowAccountScreen()
        {
            this.Controls.Clear();
            _accountScreen = new AccountScreen(this, _authenticatedUserAcct);
            _accountScreen.DisplayAccount();
        }

        public void ShowSignUpScreen()
        {
            _signUpScreen = new SignUpScreen(this);
            _signUpScreen.DisplaySignUp();
        }

        public void ShowCatalogScreen()
        {
            if (_authenticatedUserAcct.getSelectedSponsor() == null)
                return;

            Catalog catalog = new Catalog(_authenticatedUserAcct, _authenticatedUserAcct.getSelectedSponsor());

            _catalogScreen = new CatalogScreen(this, catalog, _authenticatedUserAcct);
            _catalogScreen.DisplayCatalog();
        }

        public void ShowEditCatalogScreen()
        {
            if (!_authenticatedUserAcct.isSponsorAccount())
                return;
            _editCatalogScreen = new EditCatalogScreen(this, _authenticatedUserAcct);
            _editCatalogScreen.DisplayEditCatalog();
        }

        public void ShowCouponScreen()
        {
            if (!_authenticatedUserAcct.isSponsorAccount())
                return;

            this.Controls.Clear();
            _couponScreen = new CouponScreen(this, _authenticatedUserAcct);
            _couponScreen.DisplayCoupon();
        }

        public void ShowUsersScreen()
        {
            if (!_authenticatedUserAcct.isSponsorAccount())
                return;

            this.Controls.Clear();
            _usersScreen = new UsersScreen(this, _authenticatedUserAcct.getSponsorInstance());
            _usersScreen.DisplayUsers();
        }

        public void OnClick_SignUpButton(object sender, System.EventArgs e)
        {

            this.Controls.Clear();
            ShowSignUpScreen();
        }

        public void ShowAdminPage()
        {
            this.Controls.Clear();
            _adminPage = new AdminPage(this);
            _adminPage.DisplayAdminPage();
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

        public void ShowCartScreen(object sender, EventArgs e)
        {
            this.Controls.Clear();
            _cartScreen = new CartScreen(this, _authenticatedUserAcct);
            _cartScreen.DisplayCart();
        }

        public void ShowProductScreen()
        {
            this.Controls.Clear();
            _productScreen = new ProductScreen(this, _authenticatedUserAcct);
            _productScreen.DisplayProduct();
        }

        public void ShowEmailVerifyScreen(string email, string passwordResetID)
        {
            this.Controls.Clear();
            _emailVerifyScreen = new EmailVerifyScreen(this, email, passwordResetID);
            _emailVerifyScreen.DisplayEmailVerify();
        }

        public void ShowPasswordResetScreen(string email)
        {
            this.Controls.Clear();
            _passwordResetScreen = new PasswordResetScreen(this, email);
            _passwordResetScreen.DisplayPasswordReset();
        }

        private void On_FormClosing(Object sender, FormClosingEventArgs e)
        {
            var dbcon = DBServerInstance.Instance();
            string query = "DELETE FROM Users WHERE Email=@email";
            List<string> targets = new List<string>();
            targets.Add("@email");
            List<string> parms;
            foreach (string email in createdUsers)
            {
                parms = new List<string>();
                parms.Add(email);
                dbcon.ExecuteParameterizedQuery(query, targets, parms, false);
            }
        }
    }
}
