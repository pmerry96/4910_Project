using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Infinium.Model
{
    public class Notification
    {

        public Notification()
        {

        }

        private static Notification _instance = null;
        public static Notification Instance()
        {
            if (_instance == null)
                _instance = new Notification();
            return _instance;
        }

        public void GlobalNotification(string message)
        {
            List<UserInfo> userInfo = GetAllUsers();
            if(userInfo == null)
            {
                MessageBox.Show("Could not send global notification because SQL injection was detected.");
                return;
            }
            foreach (UserInfo info in userInfo)
            {
                if (info.GetPref().Equals("Text") && info.GetPhone() != null)
                {
                    TextAPI.SendMessage(info.GetPhone(), message);
                }
                else if (info.GetPref().Equals("Email"))
                {
                    //TODO: Send email
                }
            }
        }

        public void SponsorNotification(Sponsor sponsor, string message)
        {
            List<UserInfo> userInfo = GetSponsorsUsers(sponsor);
            if (userInfo == null)
            {
                return;
            }
            foreach (UserInfo info in userInfo)
            {
                if (info.GetPref().Equals("Text") && info.GetPhone() != null)
                {
                    TextAPI.SendMessage(info.GetPhone(), message);
                } 
                else if (info.GetPref().Equals("Email"))
                {
                    //TODO: Send email
                }
            }
        }

        private List<UserInfo> GetSponsorsUsers(Sponsor sponsor)
        {
            List<UserInfo> userInfo = new List<UserInfo>();

            var dbCon = DBServerInstance.Instance();
            string query = "SELECT Phone_Num, Email, Notif_Preference FROM Users AS u JOIN Sponsored_By AS sb ON sb.UserID = u.UserID WHERE sb.SponsorID = @sponsorId";
            List<string> targets = new List<string>();
            targets.Add("@sponsorId");
            List<string> parms = new List<string>();
            parms.Add(sponsor.GetId().ToString());
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, targets, parms, true);
            if(rdr == null)
            {
                MessageBox.Show("Could not get the sponsors users due to detected SQL injection.");
                return null;
            }
            while (rdr.Read())
            {
                string phone = rdr.IsDBNull(0) ? null : rdr.GetString("Phone_Num");
                string email = rdr.GetString("Email");
                string pref = rdr.GetString("Notif_Preference");

                UserInfo info = new UserInfo(phone, email, pref);
                userInfo.Add(info);
            }

            rdr.Close();

            return userInfo;
        }

        private List<UserInfo> GetAllUsers()
        {
            List<UserInfo> userInfo = new List<UserInfo>();

            var dbCon = DBServerInstance.Instance();
            string query = "SELECT Phone_Num, Email, Notif_Preference FROM Users";
            MySqlDataReader rdr = dbCon.ExecuteParameterizedQuery(query, new List<string>(), new List<string>(), true);
            if (rdr == null)
            {
                return null;
            }
            while (rdr.Read())
            {
                string phone = rdr.IsDBNull(0) ? null : rdr.GetString("Phone_Num");
                string email = rdr.GetString("Email");
                string pref = rdr.GetString("Notif_Preference");

                UserInfo info = new UserInfo(phone, email, pref);
                userInfo.Add(info);
            }

            rdr.Close();

            return userInfo;
        }

        public class UserInfo
        {

            private string phone;
            private string email;
            private string pref;

            public UserInfo(string phone, string email, string pref)
            {
                this.phone = phone;
                this.email = email;
                this.pref = pref;
            }

            public string GetPhone()
            {
                return phone;
            }

            public string GetEmail()
            {
                return email;
            }

            public string GetPref()
            {
                return pref;
            }

        }

    }
}
