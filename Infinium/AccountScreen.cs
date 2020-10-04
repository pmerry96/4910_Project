using Infinium.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Infinium
{
    public class AccountScreen : Form
    {
        Infinium _infinium;
        User _user;
        Label _welcomeTitle;
        Label _accountScreenTitle;
        Label _namePrompt;
        Label _addressPrompt;
        Label _emailPrompt;
        Label _phonePrompt;
        Label _sponsorPrompt;
        Button _saveAccountChangesButton;

        Label _recentPurchasesTitle;
        ListBox _recentPurchasesListBox;
        List<Product> _purchases;

        Label _specialOfferTitle;
        ListBox _specialOfferListBox;
        List<Product> _specialOffers;

        Button _cartButton;
        Label _userpointsLabel;

        public AccountScreen(User user)
        {
            _user = user;

            _welcomeTitle = new Label();
        }

        public void DisplayAccount()
        {
        }
    }
}
