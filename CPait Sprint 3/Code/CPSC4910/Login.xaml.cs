using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace CPSC4910
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(password.Password))
            {
                MessageBox.Show("Please fill out the entire form!");
                return;
            }

            LoginButton(email.Text, password.Password);
        }

        private void LoginButton(string email, string password)
        {
            var dbCon = DBConnection.Instance();

            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM Users WHERE Email = '" + email + "' and password = '" + password + "'";
                MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    //TODO: Store user info and redirect
                    MessageBox.Show("Logged in!");
                } else
                {
                    MessageBox.Show("Invalid email or password!");
                }

                rdr.Close();
                dbCon.Close();
            }
        }
    }
}
