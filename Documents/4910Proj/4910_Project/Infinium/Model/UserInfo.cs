using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinium.Model
{
    public class UserInfo
    {
        private int id;

        private string email;

        private string name;

        private string phone;

        private string streetNum;

        private string street;

        private string city;

        private string state;

        private string zip;

        public UserInfo(int id, string email, string name, string phone, string streetNum, string street, string city, string state, string zip)
        {
            this.id = id;
            this.email = email;
            this.name = name;
            this.phone = phone;
            this.streetNum = streetNum;
            this.street = street;
            this.city = city;
            this.state = state;
            this.zip = zip;
        }

        public int GetId()
        {
            return id;
        }

        public string GetEmail()
        {
            return email;
        }

        public string GetName()
        {
            return name;
        }

        public string GetPhone()
        {
            return phone;
        }

        public string GetStreetNum()
        {
            return streetNum;
        }

        public string GetStreet()
        {
            return street;
        }

        public string GetCity()
        {
            return city;
        }

        public string GetState()
        {
            return state;
        }

        public string GetZip()
        {
            return zip;
        }
    }
}
