using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Configuration;

namespace BitFlipping.UComments.Core.Helpers
{
    public static class IPAddressHelper
    {
        public static IPAddress GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return new IPAddress(Convert.ToByte(addresses[0]));
                }
            }

            string REMOTE_ADDR = context.Request.ServerVariables["REMOTE_ADDR"];
            if (REMOTE_ADDR.EndsWith("::1"))
                return GetLocalIPAddress();

            return new IPAddress(Convert.ToByte(REMOTE_ADDR));
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static async Task<bool> IsBad(IPAddress ipAddress)
        {
            double treshold = 0.95;
            IPIntel intel = new IPIntel();

            using (HttpClient client = new HttpClient())
            {
                string strIPAadress = ipAddress.ToString();
                string contactEmailAddress = UmbracoConfig.For.UmbracoSettings().Content.NotificationEmailAddress;

                Uri requestUri = new Uri($"http://check.getipintel.net/check.php?ip={strIPAadress}&contact={contactEmailAddress}&format=json");
                var response = await client.GetStringAsync(requestUri);
                intel = JsonConvert.DeserializeObject<IPIntel>(response);
            }
            double result = Double.Parse(intel.result);
            return result > treshold;
        }
    }

    public class IPIntel
    {
        public string status { get; set; }
        public string result { get; set; }
        public string queryIP { get; set; }
        public object queryFlags { get; set; }
        public object queryOFlags { get; set; }
        public string queryFormat { get; set; }
        public string contact { get; set; }
    }
}
