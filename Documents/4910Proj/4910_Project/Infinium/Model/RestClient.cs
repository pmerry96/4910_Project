using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace Infinium.Model
{
    public class RestClient
    {

        public string endPoint { get; set; }
        public HttpVerb method { get; set; }

        public RestClient()
        {
            endPoint = string.Empty;
            method = HttpVerb.GET;
        }

        public string MakeRequest()
        {
            string responseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(endPoint);

            request.Method = method.ToString();

            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("REST API Error: " + response.StatusCode.ToString());
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }
                }
            }

            return responseValue;
        }

    }

    public enum HttpVerb
    {
        GET, POST, PUT, DELETE
    }
}
