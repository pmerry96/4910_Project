using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infinium.Model
{
    public class User
    {
        private string _username { get; set; }
        private string _password { get; set; }

        private Boolean _hasAuth;

        public User(string user, string pass)
        {
            _username = user;
            _password = pass;
            _hasAuth = Authenticate();
        }

        private Boolean Authenticate()
        {
            //this will need to communicate with DB Server Application Connection module in order to verify the query 
            return (_username.Equals("Admin") && _password.Equals("password") ? true : false);
        }

        public Boolean IsAuth()
        {
                return _hasAuth;
        }
    }
}
