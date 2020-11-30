using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Infinium.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Infinium
{
    public class EditCatalogScreen : Form
    {
        Infinium infinium;
        User user;
        Sponsor sponsor;
        Catalog catalog;
        List<Product> searchedProducts = new List<Product>();

        private Label titleLabel;
        private Button searchButton;
        private TextBox searchBox;
        private ListBox resultsListBox;
        private ListBox catalogListBox;
        private Label searchResultsLabel;
        private TextBox pointBox;
        private Label pointConversionLabel;
        private Button updatePointButton;
        private Label currentCatalogLabel;

        public EditCatalogScreen(Form form, User user)
        {
            this.user = user;
            infinium = (Infinium) form;
            infinium.Controls.Clear();

            sponsor = user.getSponsorInstance();
            catalog = new Catalog(user, sponsor);

            InitializeComponent();

            infinium.Controls.Add(titleLabel);
            infinium.Controls.Add(searchButton);
            searchButton.MouseClick += OnClick_SearchButton;
            infinium.Controls.Add(searchBox);
            infinium.Controls.Add(resultsListBox);
            infinium.Controls.Add(catalogListBox);
            infinium.Controls.Add(searchResultsLabel);
            infinium.Controls.Add(currentCatalogLabel);
            infinium.Controls.Add(pointConversionLabel);
            infinium.Controls.Add(updatePointButton);
            infinium.Controls.Add(pointBox);

            pointBox.Text = sponsor.GetUsdToPoints().ToString();

            PopulateCatalog();
        }

        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Button();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.resultsListBox = new System.Windows.Forms.ListBox();
            this.catalogListBox = new System.Windows.Forms.ListBox();
            this.searchResultsLabel = new System.Windows.Forms.Label();
            this.currentCatalogLabel = new System.Windows.Forms.Label();
            this.pointBox = new System.Windows.Forms.TextBox();
            this.pointConversionLabel = new System.Windows.Forms.Label();
            this.updatePointButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(12, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(89, 13);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Edit Your Catalog";
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(666, 9);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(98, 23);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Search eBay";
            this.searchButton.UseVisualStyleBackColor = true;
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(374, 11);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(286, 20);
            this.searchBox.TabIndex = 2;
            // 
            // resultsListBox
            // 
            this.resultsListBox.FormattingEnabled = true;
            this.resultsListBox.Location = new System.Drawing.Point(12, 62);
            this.resultsListBox.Name = "resultsListBox";
            this.resultsListBox.Size = new System.Drawing.Size(752, 173);
            this.resultsListBox.TabIndex = 3;
            // 
            // catalogListBox
            // 
            this.catalogListBox.FormattingEnabled = true;
            this.catalogListBox.Location = new System.Drawing.Point(12, 258);
            this.catalogListBox.Name = "catalogListBox";
            this.catalogListBox.Size = new System.Drawing.Size(752, 173);
            this.catalogListBox.TabIndex = 4;
            // 
            // searchResultsLabel
            // 
            this.searchResultsLabel.AutoSize = true;
            this.searchResultsLabel.Location = new System.Drawing.Point(12, 47);
            this.searchResultsLabel.Name = "searchResultsLabel";
            this.searchResultsLabel.Size = new System.Drawing.Size(79, 13);
            this.searchResultsLabel.TabIndex = 5;
            this.searchResultsLabel.Text = "Search Results";
            // 
            // currentCatalogLabel
            // 
            this.currentCatalogLabel.AutoSize = true;
            this.currentCatalogLabel.Location = new System.Drawing.Point(9, 242);
            this.currentCatalogLabel.Name = "currentCatalogLabel";
            this.currentCatalogLabel.Size = new System.Drawing.Size(80, 13);
            this.currentCatalogLabel.TabIndex = 6;
            this.currentCatalogLabel.Text = "Current Catalog";
            // 
            // pointBox
            // 
            this.pointBox.Location = new System.Drawing.Point(297, 11);
            this.pointBox.Name = "pointBox";
            this.pointBox.Size = new System.Drawing.Size(71, 20);
            this.pointBox.TabIndex = 7;
            // 
            // pointConversionLabel
            // 
            this.pointConversionLabel.AutoSize = true;
            this.pointConversionLabel.Location = new System.Drawing.Point(217, 14);
            this.pointConversionLabel.Name = "pointConversionLabel";
            this.pointConversionLabel.Size = new System.Drawing.Size(74, 13);
            this.pointConversionLabel.TabIndex = 8;
            this.pointConversionLabel.Text = "Points/USD = ";
            // 
            // updatePointButton
            // 
            this.updatePointButton.Location = new System.Drawing.Point(297, 33);
            this.updatePointButton.Name = "updatePointButton";
            this.updatePointButton.Size = new System.Drawing.Size(71, 23);
            this.updatePointButton.TabIndex = 9;
            this.updatePointButton.Text = "Update";
            this.updatePointButton.UseVisualStyleBackColor = true;
            this.updatePointButton.Click += OnClick_UpdateButton;
            // 
            // EditCatalogScreen
            // 
            this.ClientSize = new System.Drawing.Size(776, 443);
            this.Controls.Add(this.updatePointButton);
            this.Controls.Add(this.pointConversionLabel);
            this.Controls.Add(this.pointBox);
            this.Controls.Add(this.currentCatalogLabel);
            this.Controls.Add(this.searchResultsLabel);
            this.Controls.Add(this.catalogListBox);
            this.Controls.Add(this.resultsListBox);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.titleLabel);
            this.Name = "EditCatalogScreen";
            this.Text = "Infinium";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void DisplayEditCatalog()
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

            titleLabel.Show();
            searchButton.Show();
            searchBox.Show();
            resultsListBox.Show();
            catalogListBox.Show();
            searchResultsLabel.Show();
            currentCatalogLabel.Show();
        }

        public void OnClick_SearchButton(object sender, System.EventArgs e)
        {
            string searchResult = searchBox.Text.Replace(" ", "%20");

            if (searchResult.Equals(string.Empty))
                return;

            searchedProducts.Clear();
            resultsListBox.Items.Clear();

            RestClient restClient = new RestClient();
            restClient.endPoint = "https://svcs.ebay.com/services/search/FindingService/v1?OPERATION-NAME=findItemsByKeywords&SERVICE-VERSION=1.0.0&SECURITY-APPNAME=PhilipMe-TestAPI-PRD-96c587f86-b4dfa0c0&RESPONSE-DATA-FORMAT=JSON&REST-PAYLOAD&keywords=" + searchResult + "&paginationInput.entriesPerPage=25";

            string response = restClient.MakeRequest();
            dynamic json = JsonConvert.DeserializeObject(response);
            List<Product> found = new List<Product>();

            foreach (dynamic item in json.findItemsByKeywordsResponse[0].searchResult[0].item)
            {
                string ebayId = item.itemId[0];
                string name = item.title[0];
                double price = item.sellingStatus[0].currentPrice[0].__value__;
                string description = item.subtitle != null ? item.subtitle[0] : "No Description";
                string imagePath = item.galleryURL[0];

                Product product = GetProductFromDB(ebayId);
                if (product == null)
                {
                    product = new Product(-1, ebayId, name, price, description, imagePath);
                    //better to return out of method here?
                }

                found.Add(product);
                searchedProducts.Add(product);
            }

            foreach (Product product in found)
            {
                string output = product.GetName() + " - " + product.GetDescription() + " - Price: $" + product.GetPrice();
                resultsListBox.Items.Add(output);
            }
        }

        public void OnClick_UpdateButton(object sender, System.EventArgs e)
        {
            var dbCon = DBServerInstance.Instance();
            string query = "UPDATE Sponsor SET USD_To_Points = @points WHERE Sponsor_ID = @sponsorID";
            List<string> targets = new List<string>();
            targets.Add("@points");
            targets.Add("@sponsorID");
            List<string> parms = new List<string>();
            parms.Add(pointBox.Text);
            parms.Add(sponsor.GetId().ToString());
            dbCon.ExecuteParameterizedQuery(query, targets, parms, false);

            sponsor.SetUsdToPoints(Double.Parse(pointBox.Text));
        }

            public void OnMouseDoubleClick_AddToCart(object sender, MouseEventArgs e)
        {
            int index = resultsListBox.IndexFromPoint(e.Location);

            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Product product = searchedProducts[index];

                if (product.GetId() == -1) // not in database yet
                {
                    DBServerInstance dbCon = DBServerInstance.Instance();
                    //dbCon.ExecuteQuery("INSERT INTO Product (Ebay_ID, Name, Price, Description, Display_IMG_Path) VALUES ('" + product.GetEbayId() + "','" + product.GetName() + "','" + product.GetPrice() + "','" + product.GetDescription() + "','" + product.GetImgPath() + "')", false);
                    string query = "INSERT INTO Product (Ebay_ID, Name, Price, Description, Display_IMG_Path) VALUES (@ebayID,@prodname,@prodprice,@prodDescription,@path)";
                    List<string> targets = new List<string>();
                    targets.Add("@ebayID");
                    targets.Add("@prodname");
                    targets.Add("@prodprice");
                    targets.Add("@prodDescription");
                    targets.Add("@path");
                    List<string> parms = new List<string>();
                    parms.Add(product.GetEbayId());
                    parms.Add(product.GetName());
                    parms.Add(product.GetPrice().ToString());
                    parms.Add(product.GetDescription());
                    parms.Add(product.GetImgPath());
                    dbCon.ExecuteParameterizedQuery(query, targets, parms, false);


                    //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT p.Product_ID, p.Ebay_ID, p.Name, p.Price, p.Description, p.Display_IMG_Path FROM Product p WHERE p.Ebay_ID = " + product.GetEbayId(), true);
                    string query2 = "SELECT p.Product_ID, p.Ebay_ID, p.Name, p.Price, p.Description, p.Display_IMG_Path FROM Product p WHERE p.Ebay_ID = @ebayID";
                    List<string> targets2 = new List<string>();
                    targets2.Add("@ebayID");
                    List<string> parms2 = new List<string>();
                    parms2.Add(product.GetEbayId());
                    MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query2, targets2, parms2, true);
                    if (rdr == null)
                    {
                        return;
                    }
                    if (rdr.Read())
                    {
                        product = new Product(rdr.GetInt32("Product_ID"), rdr.GetString("Ebay_ID"), rdr.GetString("Name"), rdr.GetDouble("Price"), rdr.GetString("Description"), rdr.GetString("Display_IMG_Path"));
                        rdr.Close();

                        searchedProducts[index] = product;
                        AddToCatalog(product);
                        PopulateCatalog();
                    }
                }
                else
                {
                    AddToCatalog(product);
                    PopulateCatalog();
                }
            }
        }

        public void OnMouseDoubleClick_RemoveToCart(object sender, MouseEventArgs e)
        {
            int index = catalogListBox.IndexFromPoint(e.Location);

            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Product product = catalog.GetProducts()[index];
                DBServerInstance dbCon = DBServerInstance.Instance();

                int catalogId = -1;
                //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT Catalog_ID FROM Fills_Catalog WHERE Sponsor_ID = " + sponsor.GetId(), true);
                string query = "SELECT Catalog_ID FROM Fills_Catalog WHERE Sponsor_ID = @sponsorID";
                List<string> targets = new List<string>();
                targets.Add("@sponsorID");
                List<string> parms = new List<string>();
                parms.Add(sponsor.GetId().ToString());
                MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
                if (rdr == null)
                {
                    return;
                }
                if (rdr.Read())
                {
                    catalogId = rdr.GetInt32("Catalog_ID");
                }
                rdr.Close();
                if (catalogId == -1)
                    return;

                //dbCon.ExecuteQuery("DELETE FROM Catalog_Contains WHERE Catalog_ID = " + catalogId + " AND Product_ID = " + product.GetId(), false);
                string query2 = "DELETE FROM Catalog_Contains WHERE Catalog_ID = @catalogID AND Product_ID = @prodID";
                List<string> targets2 = new List<string>();
                targets2.Add("@catalogID");
                targets2.Add("@prodID");
                List<string> parms2 = new List<string>();
                parms2.Add(catalogId.ToString());
                parms2.Add(product.GetId().ToString());
                dbCon.ExecuteParameterizedQuery(query2, targets2, parms2, false);
                PopulateCatalog();
            }
        }

        private void PopulateCatalog()
        {
            catalog.Load();
            catalogListBox.Items.Clear();

            foreach (Product product in catalog.GetProducts())
            {
                string output = product.GetName() + " - " + product.GetDescription() + " - Price: $" + product.GetPrice();
                catalogListBox.Items.Add(output);
            }
        }

        private Product GetProductFromDB(string ebayId)
        {
            DBServerInstance dbCon = DBServerInstance.Instance();
            //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT p.Product_ID, p.Ebay_ID, p.Name, p.Price, p.Description, p.Display_IMG_Path FROM Product p JOIN Catalog_Contains cc ON p.Product_ID = cc.Product_ID JOIN Catalog c ON c.Catalog_ID = cc.Catalog_ID JOIN Fills_Catalog fc ON fc.Sponsor_ID = " + sponsor.GetId() + " AND fc.Catalog_ID = c.Catalog_ID WHERE fc.Sponsor_ID = " + sponsor.GetId() + " AND p.Ebay_ID = " + ebayId, true);
            string query = "SELECT p.Product_ID, p.Ebay_ID, p.Name, p.Price, p.Description, p.Display_IMG_Path FROM Product p JOIN Catalog_Contains cc ON p.Product_ID = cc.Product_ID JOIN Catalog c ON c.Catalog_ID = cc.Catalog_ID JOIN Fills_Catalog fc ON fc.Sponsor_ID = @sponsorID AND fc.Catalog_ID = c.Catalog_ID WHERE fc.Sponsor_ID = @sponsorID2 AND p.Ebay_ID = @ebayID";
            List<string> targets = new List<string>();
            targets.Add("@sponsorID");
            targets.Add("@sponsorID2");
            targets.Add("@ebayID");
            List<string> parms = new List<string>();
            parms.Add(sponsor.GetId().ToString());
            parms.Add(sponsor.GetId().ToString());
            parms.Add(ebayId);
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return null;
            }
            while (rdr.Read())
            {
                Product product = new Product(rdr.GetInt32("Product_ID"), rdr.GetString("Ebay_ID"), rdr.GetString("Name"), rdr.GetDouble("Price"), rdr.GetString("Description"), rdr.GetString("Display_IMG_Path"));
                rdr.Close();

                return product;
            }

            rdr.Close();
            return null;
        }

        private void AddToCatalog(Product product)
        {
            DBServerInstance dbCon = DBServerInstance.Instance();
            //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT p.Product_ID FROM Product p JOIN Catalog_Contains cc ON p.Product_ID = cc.Product_ID JOIN Catalog c ON c.Catalog_ID = cc.Catalog_ID JOIN Fills_Catalog fc ON fc.Sponsor_ID = " + sponsor.GetId() + " AND fc.Catalog_ID = c.Catalog_ID WHERE fc.Sponsor_ID = " + sponsor.GetId() + " AND p.Product_ID = " + product.GetId(), true);
            string query = "SELECT p.Product_ID FROM Product p JOIN Catalog_Contains cc ON p.Product_ID = cc.Product_ID JOIN Catalog c ON c.Catalog_ID = cc.Catalog_ID JOIN Fills_Catalog fc ON fc.Sponsor_ID = @sponsorID AND fc.Catalog_ID = c.Catalog_ID WHERE fc.Sponsor_ID = @sponsorID2 AND p.Product_ID = @prodID";
            List<string> targets = new List<string>();
            targets.Add("@sponsorID");
            targets.Add("@sponsorID2");
            targets.Add("@prodID");
            List<string> parms = new List<string>();
            parms.Add(sponsor.GetId().ToString());
            parms.Add(sponsor.GetId().ToString());
            parms.Add(product.GetId().ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            if (rdr.Read()) // Already in catalog
            {
                rdr.Close();
                return;
            }

            rdr.Close();

            int catalogId = -1;
            //rdr = dbCon.ExecuteQuery("SELECT Catalog_ID FROM Fills_Catalog WHERE Sponsor_ID = " + sponsor.GetId(), true);
            string query2 = "SELECT Catalog_ID FROM Fills_Catalog WHERE Sponsor_ID = @sponsorID";
            List<string> targets2 = new List<string>();
            targets2.Add("@sponsorID");
            List<string> parms2 = new List<string>();
            parms2.Add(sponsor.GetId().ToString());
            rdr = dbCon.ExecuteParameterizedQuery(query2, targets2, parms2, true);
            if (rdr == null)
            {
                return;
            }
            if (rdr.Read())
            {
                catalogId = rdr.GetInt32("Catalog_ID");
            }

            rdr.Close();

            if (catalogId == -1)
                return;

            //dbCon.ExecuteQuery("INSERT INTO Catalog_Contains (Catalog_ID, Product_ID) VALUES ('" + catalogId + "', '" + product.GetId() + "')", false);
            string query3 = "INSERT INTO Catalog_Contains (Catalog_ID, Product_ID) VALUES (@catalogID, @prodID)";
            List<string> targets3 = new List<string>();
            targets3.Add("@catalogID");
            targets3.Add("@prodID");
            List<string> parms3 = new List<string>();
            parms3.Add(catalogId.ToString());
            parms3.Add(product.GetId().ToString());
            dbCon.ExecuteParameterizedQuery(query3, targets3, parms3, false);
        }
    }
}
