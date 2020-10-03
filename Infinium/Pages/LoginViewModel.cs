using Stylet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;


namespace Infinium.Pages
{
    public class LoginViewModel : Screen
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public void OnClick_LoginButton()
        {
            // event handler goes here
            MessageBox.Show(Username);
        }
    }
}
