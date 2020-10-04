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
    public class SignUpScreen : Form
    {
        Infinium _infinium;

        Label _signUpTitle;
        Label _emailPrompt;
        TextBox _emailEntry;
        Label _firstNamePrompt;
        TextBox _firstNameEntry;
        Label _lastNamePrompt;
        TextBox _lastNameEntry;
        Label _sponsorIDPrompt;
        TextBox _sponsorIDEntry;

        Button _backButton;

        public SignUpScreen(Form form)
        {
            _infinium = (Infinium) form;

            _signUpTitle = new Label();
            _signUpTitle.Text = "Sign Up";
            _signUpTitle.Location = new Point(_infinium.Width / 2 - _signUpTitle.Width/2, _infinium.Height / 4);
            _infinium.Controls.Add(_signUpTitle);

            _emailPrompt = new Label();
            _emailPrompt.Text = "Email Address";
            _emailPrompt.Location = new Point(_infinium.Width/2 - _emailPrompt.Width - 10, _signUpTitle.Bottom + 5);
            _infinium.Controls.Add(_emailPrompt);

            _emailEntry = new TextBox();
            _emailEntry.Location = new Point(_infinium.Width/2 + 10, _emailPrompt.Top);
            _infinium.Controls.Add(_emailEntry);

            _firstNamePrompt = new Label();
            _firstNamePrompt.Text = "First Name";
            _firstNamePrompt.Location = new Point(_infinium.Width/2 - _firstNamePrompt.Width - 10, _emailPrompt.Bottom + 5);
            _infinium.Controls.Add(_firstNamePrompt);

            _firstNameEntry = new TextBox();
            _firstNameEntry.Location = new Point(_infinium.Width/2 + 10, _firstNamePrompt.Top);
            _infinium.Controls.Add(_firstNameEntry);

            _lastNamePrompt = new Label();
            _lastNamePrompt.Text = "Last Name";
            _lastNamePrompt.Location = new Point(_infinium.Width/2 - _lastNamePrompt.Width - 10, _firstNamePrompt.Bottom + 5);
            _infinium.Controls.Add(_lastNamePrompt);

            _lastNameEntry = new TextBox();
            _lastNameEntry.Location = new Point(_infinium.Width/2 + 10, _lastNamePrompt.Top);
            _infinium.Controls.Add(_lastNameEntry);

            _sponsorIDPrompt = new Label();
            _sponsorIDPrompt.Text = "Sponsor ID Number";
            _sponsorIDPrompt.Location = new Point(_infinium.Width/2 - _sponsorIDPrompt.Width - 10, _lastNamePrompt.Bottom + 5);
            _infinium.Controls.Add(_sponsorIDPrompt);

            _sponsorIDEntry = new TextBox();
            _sponsorIDEntry.Location = new Point(_infinium.Width/2 + 10, _sponsorIDPrompt.Top);
            _infinium.Controls.Add(_sponsorIDEntry);

            _backButton = new Button();
            _backButton.Text = "Back";
            _backButton.Location = new Point(_infinium.Width/2 - _backButton.Width/2, _sponsorIDEntry.Bottom + 10);
            _backButton.Click += _infinium.OnClick_SignUpBackButton;
            _infinium.Controls.Add(_backButton);
        }

        public void DisplaySignUp()
        {
            _signUpTitle.Show();
            _emailPrompt.Show();
            _emailEntry.Show();
            _firstNamePrompt.Show();
            _firstNameEntry.Show();
            _lastNamePrompt.Show();
            _lastNameEntry.Show();
            _sponsorIDPrompt.Show();
            _sponsorIDEntry.Show();
            _backButton.Show();
        }
    }
}
