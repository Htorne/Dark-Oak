using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace RestAPITester
{

    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RestClient
    {
        public String endPoint { get; set; }
        public httpVerb httpMethod { get; set; }
        public RestClient()
        {
            endPoint = string.Empty;
            httpMethod = httpVerb.GET;


        }
        public string makeRequest()
        {
            string strResponseValue = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);

            request.Method = httpMethod.ToString();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ApplicationException("error code: " + response.StatusCode);
                    }
                    // Process the resoinse stream... (cold be JSON, HTML, XML etc..) 
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponseValue = reader.ReadToEnd();
                            }
                        }

                    }

                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                //FormMain.showError(Convert.ToString(ex));
                System.Diagnostics.Debug.WriteLine("Tried to find stuff and failed");
            }
            return strResponseValue;

        }
    }

}
