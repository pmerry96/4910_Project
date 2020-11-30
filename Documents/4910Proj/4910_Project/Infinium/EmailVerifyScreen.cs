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
using MySql.Data.MySqlClient;

namespace Infinium
{
    public class EmailVerifyScreen : Form
    {
        Infinium _infinium;
        Label _emailPrompt;
        Label _emailEntry;
        Label _resetPrompt;
        TextBox _resetEntry;
        Button _continueButton;
        Button _backButton;
        Label _title;

        string _email;
        string _passwordResetID;
        public EmailVerifyScreen(Form form, string email, string passwordResetID)
        {
            _infinium = (Infinium)form;
            _email = email;
            _passwordResetID = passwordResetID;

            _title = new Label();
            _title.Text = "Email Verification";
            _title.Location = new Point(_infinium.Width / 2 - _title.Width / 2, _infinium.Height / 3);


            _emailEntry = new Label();
            _emailEntry.Text = email;
            _emailEntry.Location = new Point(_infinium.Width / 2, _title.Bottom + 20);


            _emailPrompt = new Label();
            _emailPrompt.Text = "Email";
            _emailPrompt.Location = new Point(_emailEntry.Left - _emailPrompt.Width - 20, _emailEntry.Top);


            _resetEntry = new TextBox();
            _resetEntry.Location = new Point(_emailEntry.Left, _emailEntry.Bottom + 5);


            _resetPrompt = new Label();
            _resetPrompt.Text = "Reset ID";
            _resetPrompt.Location = new Point(_resetEntry.Left - _resetPrompt.Width - 20, _resetEntry.Top);

            _continueButton = new Button();
            _continueButton.Text = "Continue";
            _continueButton.Location = new Point(_resetEntry.Left + ((_resetEntry.Width - _continueButton.Width) / 2), _resetEntry.Bottom + 5);
            _continueButton.Click += OnClick_continueButton;

            _backButton = new Button();
            _backButton.Text = "Reset";
            _backButton.Location = new Point(_resetPrompt.Left, _resetPrompt.Bottom + 5);
            _backButton.Click += OnClick_backButton;
        }
        public void DisplayEmailVerify()
        {
            _infinium.Controls.Add(_title);
            _title.Show();

            _infinium.Controls.Add(_emailEntry);
            _emailEntry.Show();

            _emailPrompt.Show();
            _infinium.Controls.Add(_emailPrompt);

            _infinium.Controls.Add(_resetEntry);
            _resetEntry.Show();

            _infinium.Controls.Add(_resetPrompt);
            _resetPrompt.Show();

            _infinium.Controls.Add(_continueButton);
            _continueButton.Show();
        }

        public void OnClick_continueButton(object sender, System.EventArgs e)
        {
            if (_passwordResetID != _resetEntry.Text)
            {
                MessageBox.Show("The verification code is incorrect.");
                return;
            }
            _infinium.ShowPasswordResetScreen(_email);
        }

        public void OnClick_backButton(object sender, System.EventArgs e)
        {
            _infinium.ShowLoginScreen();
        }
    }
}