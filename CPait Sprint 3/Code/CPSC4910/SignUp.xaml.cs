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
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {

        public SignupWindow()
        {
            InitializeComponent();
        }

        private void SignupButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(sponsor_code.Text) || string.IsNullOrEmpty(password.Password) || string.IsNullOrEmpty(password_confirm.Password))
            {
                MessageBox.Show("Please fill out the entire form!");
                return;
            }

            if (EmailExists(email.Text))
            {
                MessageBox.Show("That email already exists!");
                return;
            }

            if (!SponsorExists(sponsor_code.Text))
            {
                MessageBox.Show("A sponsor with that code doesn't exist!");
                return;
            }

            if (!PasswordsMatch(password.Password, password_confirm.Password))
            {
                MessageBox.Show("Your passwords don't match!");
                return;
            }

            SignupButton(name.Text, email.Text, sponsor_code.Text, password.Password);
        }

        public void SignupButton(string name, string email, string sponsorCode, string password)
        {
            var dbCon = DBConnection.Instance();

            if (dbCon.IsConnect())
            {
                string query = "INSERT INTO Users (Name, Email, Password) VALUES ('" + name + "','" + email + "','" + password + "')";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.ExecuteNonQuery();
                dbCon.Close();
                MessageBox.Show("Signed up!");
            }
        }

        public bool EmailExists(string email)
        {
            var dbCon = DBConnection.Instance();

            if (dbCon.IsConnect())
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Email = '" + email + "'";
                MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    if (Int32.Parse(rdr[0].ToString()) == 0)
                    {
                        rdr.Close();

                        return false;
                    }
                }

                rdr.Close();
            }

            return true;
        }

        public bool SponsorExists(string sponsorCode)
        {
            var dbCon = DBConnection.Instance();

            if (dbCon.IsConnect())
            {
                string query = "SELECT COUNT(*) FROM Sponsor WHERE Sponsor_ID = " + Int32.Parse(sponsorCode);
                Console.WriteLine(query);
                MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    if (Int32.Parse(rdr[0].ToString()) != 0)
                    {
                        rdr.Close();

                        return true;
                    }
                }

                rdr.Close();
            }

            return false;
        }

        public bool PasswordsMatch(string pass, string confirmed)
        {
            Console.WriteLine(pass);
            return pass.Equals(confirmed);
        }
    }
}
