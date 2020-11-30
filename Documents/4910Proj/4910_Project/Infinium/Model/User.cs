using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Infinium.Model
{
    public class User
    {
        public const int PERM_USER = 0;
        public const int PERM_SPONSOR = 1;
        public const int PERM_ADMIN = 2;
        private int _id { get; set; }
        private string _email { get; set; }
        private string _username { get; set; }
        private string _password { get; set; }
        private string _notificationPreference { get; set; }

        private Boolean _hasAuth;

        private Sponsor sponsorInstance; // if the user is a sponsor account

        private string _name;

        private string _address;

        private string _streetNum;

        private string _street;

        private string _city;

        private string _state;

        private string _zip;

        private string _phone;

        private List<Sponsor> _sponsors = new List<Sponsor>();

        private int _permissions;

        private int _points = 0;

        private Sponsor _selectedSponsor;

        private Cart _cart;

        public User(int id, string email, string user, string pass, string notificationPreference, string phone, string streetNum, string street, string city, string state, string zip)
        {
            //name and username will not always be the same
            _id = id;
            _email = email;
            _username = _name = user;
            _password = pass;
            _notificationPreference = notificationPreference;
            _phone = phone;
            _hasAuth = Authenticate();
            //_permissions = getPerm(email); //_permissions = getUserPermsfromDB();
            _streetNum = streetNum;
            _street = street;
            _city = city;
            _state = state;
            _zip = zip;
            _address = streetNum + ' ' + street + ", " + city + ", " + state + ", " + zip;
            _cart = new Cart(_id);
        }

        private Boolean Authenticate()
        {
            //this will need to communicate with DB Server Application Connection module in order to verify the query 
            return (_username.Equals("Admin") && _password.Equals("password") ? true : false);
        }

        public void CheckSponsor()
        {
            DBServerInstance conn = DBServerInstance.Instance();
            //MySqlDataReader rdr = conn.ExecuteQuery("SELECT * FROM Sponsor WHERE UserID = " + _id, true);
            string query = "SELECT * FROM Sponsor WHERE UserID = @ID";
            List<string> targets = new List<string>();
            targets.Add("@ID");
            List<string> parms = new List<string>();
            parms.Add(_id.ToString());
            MySqlDataReader rdr = conn.ExecuteParameterizedQuery(query, targets, parms, true);
            int sponsorId = -1;
            if (rdr == null)
            {
                return;
            }
            while (rdr.Read())
            {
                sponsorId = rdr.GetInt32("Sponsor_ID");
            }

            rdr.Close();

            if (sponsorId == -1)
                return;
            //rdr = conn.ExecuteQuery("SELECT * from Sponsor WHERE Sponsor_ID = " + sponsorId, true);
            string query2 = "SELECT * from Sponsor WHERE Sponsor_ID = @sponsorID";
            List<string> targets2 = new List<string>();
            targets2.Add("@sponsorID");
            List<string> parms2 = new List<string>();
            parms2.Add(sponsorId.ToString());
            rdr = conn.ExecuteParameterizedQuery(query2, targets2, parms2, true);
            if (rdr == null)
            {
                return;
            }
            while (rdr.Read())
            {
                string name = rdr.GetString("Sponsor_Name");
                double points = rdr.GetDouble("USD_To_Points");
                sponsorInstance = new Sponsor(sponsorId, name, points);
            }

            rdr.Close();
        }

        public void UpdateSponsors()
        {
            _sponsors.Clear();

            List<int> sponsorIds = new List<int>();
            DBServerInstance conn = DBServerInstance.Instance();

            //MySqlDataReader rdr = conn.ExecuteQuery("SELECT * from Sponsored_By WHERE UserID = " + _id, true);
            string query = "SELECT* FROM Sponsored_By WHERE UserID = @ID";
            List<string> targets = new List<string>();
            targets.Add("@ID");
            List<string> parms = new List<string>();
            parms.Add(_id.ToString());
            MySqlDataReader rdr = conn.ExecuteParameterizedQuery(query, targets, parms, true);
            int point = 0;
            if (rdr == null)
            {
                return;
            }
            while (rdr.Read())
            {
                int id = rdr.GetInt32("SponsorID");
                sponsorIds.Add(id);
                point = rdr.GetInt32("Points");
            }
            _points = point;
            rdr.Close();

            foreach (int id in sponsorIds)
            {
                //rdr = conn.ExecuteQuery("SELECT * from Sponsor WHERE Sponsor_ID = " + id, true);
                string query2 = "SELECT * from Sponsor WHERE Sponsor_ID = @sponsorID";
                List<string> targets2 = new List<string>();
                targets2.Add("@sponsorID");
                List<string> parms2 = new List<string>();
                parms2.Add(id.ToString());
                rdr = conn.ExecuteParameterizedQuery(query2, targets2, parms2, true);
                if (rdr == null)
                {
                    return;
                }
                while (rdr.Read())
                {
                    string name = rdr.GetString("Sponsor_Name");
                    double points = rdr.GetDouble("USD_To_Points");
                    _sponsors.Add(new Sponsor(id, name, points));
                }
                rdr.Close();
            }
        }

        public int getId()
        {
            return _id;
        }

        public Sponsor getSponsorInstance()
        {
            return sponsorInstance;
        }

        public bool isSponsorAccount()
        {
            return sponsorInstance != null;
        }

        public string getName()
        {
            return _name;
        }

        public string getAddress()
        {
            return _address;
        }

        public Boolean IsAuth()
        {
            return _hasAuth;
        }

        public string getEmail()
        {
            return _email;
        }

        public string getPhone()
        {
            return _phone;
        }

        public string getNotificationPreference()
        {
            return _notificationPreference;
        }

        public Cart getCart()
        {
            return _cart;
        }

        public List<Sponsor> getSponsors()
        {
            return _sponsors;
        }

        public Sponsor GetSponsor(int id)
        {
            foreach (Sponsor sponsor in _sponsors)
            {
                if (sponsor.GetId() == id)
                    return sponsor;
            }

            return null;
        }

        public Sponsor GetSponsor(string name)
        {
            foreach (Sponsor sponsor in _sponsors)
            {
                if (sponsor.GetName().Equals(name))
                    return sponsor;
            }

            return null;
        }

        public int getPoints()
        {
            var dbcon = DBServerInstance.Instance();
            string query = "SELECT* FROM Sponsored_By WHERE UserID = @ID";
            List<string> targets = new List<string>();
            targets.Add("@ID");
            List<string> parms = new List<string>();
            parms.Add(_id.ToString());
            MySqlDataReader rdr = dbcon.ExecuteParameterizedQuery(query, targets, parms, true);
            int points = 0;
            if (rdr == null)
            {
                return 0;
            }
            while (rdr.Read())
            {
                points = rdr.GetInt32("Points");
            }
            if(_points > points)
                _points = points;
            rdr.Close();
            return _points;
        }
        public string getStreetNum()
        {
            return _streetNum;
        }

        public string getStreet()
        {
            return _street;
        }

        public string getCity()
        {
            return _city;
        }

        public string getState()
        {
            return _state;
        }

        public string getZip_Code()
        {
            return _zip;
        }

        public Sponsor getSelectedSponsor()
        {
            return _selectedSponsor;
        }

        public void setName(string name)
        {
            _name = name;
        }

        public void setAddress(string address)
        {
            _address = address;
            string[] parsed = address.Split(',');
            string[] strAddressParse = parsed[0].Split(' ');
            _streetNum = strAddressParse[0];
            string streetName = "";
            for (int i = 1; i < strAddressParse.Length; i++)
            {
                if (i != 1)
                {
                    streetName += ' ';
                }
                streetName += strAddressParse[i];
            }
            _street = streetName;
            _city = parsed[1];
            _state = parsed[2];
            _zip = parsed[3];
        }

        public void setEmail(string email)
        {
            _email = email;
        }

        public void setPhone(string phone)
        {
            _phone = phone;
        }

        public void setNotificationPreference(string notificationPreference)
        {
            _notificationPreference = notificationPreference;
        }

        public void setPoints(int points)
        {
            _points = points;
        }

        public void setSelectedSponsor(string sponsorName)
        {
            foreach (Sponsor sponsor in _sponsors)
            {
                if (sponsor.GetName().Equals(sponsorName))
                {
                    _selectedSponsor = sponsor;
                    break;
                }
            }
        }

        public Boolean isUser()
        {
            if (_permissions == PERM_ADMIN || _permissions == PERM_SPONSOR)
                return true;
            return _permissions == PERM_USER;
        }

        public Boolean isSponsor()
        {
            if (_permissions == PERM_ADMIN)
                return true;
            return _permissions == PERM_SPONSOR;
        }

        public Boolean isAdmin()
        {
            return _permissions == PERM_ADMIN;
        }

        public bool CheckCart()
        {
            DBServerInstance conn = DBServerInstance.Instance();
            string query = "SELECT * FROM FILLS_CART WHERE UserID = @ID";
            List<string> targets = new List<string>();
            targets.Add("@ID");
            List<string> parms = new List<string>();
            parms.Add(_id.ToString());
            MySqlDataReader rdr = conn.ExecuteParameterizedQuery(query, targets, parms, true);
            int cartID = -1;
            if (rdr == null)
            {
                return false;
            }
            while (rdr.Read())
            {
                cartID = rdr.GetInt32("Cart_ID");
            }
            rdr.Close();
            //conn.Close();
            if (cartID == -1)
                return false;
            else return true;
        }

        public void clearCart()
        {
            _cart = new Cart(_id);
        }

        private int getPerm(string email)
        {
            DBServerInstance dbcon = DBServerInstance.Instance();
            string query = "SELECT Permissions FROM Users WHERE Email=@email";
            List<string> targets = new List<string>();
            targets.Add("@email");
            List<string> parms = new List<string>();
            parms.Add(email);
            var rdr = dbcon.ExecuteParameterizedQuery(query, targets, parms, true);
            string perm = "";
            if (rdr == null)
            {
                return 0;
            }
            while (rdr.Read())
            {
                perm = rdr.GetString("Permissions");
            }
            rdr.Close();
            //dbcon.Close();
            if (perm == "Admin")
            {
                return 2;
            }
            if (perm == "Sponsor")
            {
                return 1;
            }
            return 0;
        }
    }
}
