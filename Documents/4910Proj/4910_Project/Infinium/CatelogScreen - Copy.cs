using Infinium.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Infinium
{
    public class CatalogScreen : Form
    {

        Infinium infinium;
        Catalog catalog;

        Label catalogTitle;
        ListBox catalogListBox;
        Button returnToAccountButton;
        Button returnToCartButton;

        public CatalogScreen(Form form, Catalog catalog)
        {
            infinium = (Infinium)form;
            infinium.Controls.Clear();
            infinium.Text = "Infinium";

            this.catalog = catalog;

            catalogTitle = new Label();
            catalogTitle.Text = catalog.GetSponsor().GetName() + "'s Catalog";
            catalogTitle.Location = new Point(5, 5);
            infinium.Controls.Add(catalogTitle);

            returnToAccountButton = new Button();
            returnToAccountButton.Text = "Cart";
            returnToAccountButton.Location = new Point((infinium.Width * 3 / 4) + 100, catalogTitle.Top);
            infinium.Controls.Add(returnToAccountButton);
            returnToAccountButton.Click += OnClick_CartButton;

            returnToCartButton = new Button();
            returnToCartButton.Text = "Account";
            returnToCartButton.Location = new Point((infinium.Width * 3 / 4) + 20, catalogTitle.Top);
            infinium.Controls.Add(returnToCartButton);
            returnToCartButton.Click += OnClick_AccountButton;

            catalogListBox = new ListBox();
            catalogListBox.Location = new Point(catalogTitle.Left, catalogTitle.Bottom + 5);
            catalogListBox.Size = new Size(infinium.Width * 3 / 4, infinium.Height * 3 / 5);
            infinium.Controls.Add(catalogListBox);
            catalogListBox.MouseDoubleClick += OnMouseDoubleClick_AddToCart;

            PopulateCatalog();
        }

        public void DisplayCatalog()
        {
            foreach (Control _indexControl in infinium.Controls)
            {
                if (infinium._defaultTheme._isDarkMode)
                {
                    infinium.BackColor = System.Drawing.Color.Black;
                    _indexControl.BackColor = System.Drawing.Color.Black;
                    _indexControl.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    infinium.BackColor = System.Drawing.Color.White;
                    _indexControl.BackColor = System.Drawing.Color.White;
                    _indexControl.ForeColor = System.Drawing.Color.Black;
                }
            }

            catalogTitle.Show();
            catalogListBox.Show();
            returnToAccountButton.Show();
            returnToCartButton.Show();
            catalogListBox.Show();
        }

        public void OnClick_AccountButton(object sender, System.EventArgs e)
        {
            infinium.ShowAccountScreen();
        }

        public void OnClick_CartButton(object sender, System.EventArgs e)
        {
            infinium.ShowCartScreen(sender, e);
        }

        public void OnMouseDoubleClick_AddToCart(object sender, MouseEventArgs e)
        {
            int index = catalogListBox.IndexFromPoint(e.Location);

            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Product product = catalog.GetProducts()[index];
                infinium._authenticatedUserAcct.getCart().addToCart(product);
            }
        }

        private void PopulateCatalog()
        {
            catalog.Load();

            foreach (Product product in catalog.GetProducts())
            {
                string output = product.GetName() + " - " + product.GetDescription() + " - Price: $" + product.GetPrice();
                catalogListBox.Items.Add(output);
            }
        }

    }
}