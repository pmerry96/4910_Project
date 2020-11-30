using Google.Protobuf.Collections;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinium.Model
{
    public class Catalog
    {

        private User user;
        private Sponsor sponsor;

        private List<Product> products = new List<Product>();

        public Catalog(User user, Sponsor sponsor)
        {
            this.user = user;
            this.sponsor = sponsor;
        }

        public User getUser()
        {
            return user;
        }

        public Sponsor GetSponsor()
        {
            return sponsor;
        }

        public List<Product> GetProducts()
        {
            return products;
        }

        public void Load()
        {
            products.Clear();

            DBServerInstance dbCon = DBServerInstance.Instance();
            //MySqlDataReader rdr = dbCon.ExecuteQuery("SELECT p.Product_ID, p.Ebay_ID, p.Name, p.Price, p.Description, p.Display_IMG_Path FROM Product p JOIN Catalog_Contains cc ON p.Product_ID = cc.Product_ID JOIN Catalog c ON c.Catalog_ID = cc.Catalog_ID JOIN Fills_Catalog fc ON fc.Sponsor_ID = " + sponsor.GetId() + " AND fc.Catalog_ID = c.Catalog_ID WHERE fc.Sponsor_ID = " + sponsor.GetId(), true);

            string query = "SELECT p.Product_ID, p.Ebay_ID, p.Name, p.Price, p.Description, p.Display_IMG_Path FROM Product p JOIN Catalog_Contains cc ON p.Product_ID = cc.Product_ID JOIN Catalog c ON c.Catalog_ID = cc.Catalog_ID JOIN Fills_Catalog fc ON fc.Sponsor_ID = @sponsorID AND fc.Catalog_ID = c.Catalog_ID WHERE fc.Sponsor_ID = @sponsorID2";
            List<string> targets = new List<string>();
            targets.Add("@sponsorID");
            targets.Add("@sponsorID2");
            List<string> parms = new List<string>();
            parms.Add(sponsor.GetId().ToString());
            parms.Add(sponsor.GetId().ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if (rdr == null)
            {
                return;
            }
            while (rdr.Read())
            {
                Product product = new Product(rdr.GetInt32("Product_ID"), rdr.GetString("Ebay_ID"), rdr.GetString("Name"), rdr.GetDouble("Price"), rdr.GetString("Description"), rdr.GetString("Display_IMG_Path"));
                products.Add(product);
            }

            rdr.Close();
        }
    }
}