using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Infinium.Model
{
    public class AccountModel
    {
        public string Password { get; set; }
        public string Username { get; set; }

        public AccountModel(string user, string pass) => (Username, Password) = (user, pass);
    }
}
