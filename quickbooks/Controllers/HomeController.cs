using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Web.Mvc;

using Intuit.Ipp.Data;

namespace quickbooks.Controllers
{
    public class HomeController : Controller
    {


        public List<Dictionary<string, object>> WorkOrders = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> Products = new List<Dictionary<string, object>>();

        public object[] GetDictionaryObjects(string url)
        {
            HttpClient client = new HttpClient();

            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest request = this.GetRequest(url);
            WebResponse webResponse = request.GetResponse();
            string responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd().ToString();

            var json = new JavaScriptSerializer().DeserializeObject(responseText);
            Dictionary<string, object> obj2 = (Dictionary<string, object>)json;

            object[] obj3 = (object[])obj2.Select(x => x.Value).ToList<object>().ElementAt(0);
            return obj3;

        }
        public void WorkOrderGet(string url)
        {

            foreach (object ob in GetDictionaryObjects(url))
            {
                Dictionary<string, object> ob4 = (Dictionary<string, object>)ob;
                WorkOrders.Add(ob4);
            }
        }
        public void ProductGet(string url)
        {
            foreach (object ob in GetDictionaryObjects(url))
            {
                Dictionary<string, object> ob4 = (Dictionary<string, object>)ob;
                Products.Add(ob4);
            }
        }
        private HttpWebRequest GetRequest(string url, string httpMethod = "GET", bool allowAutoRedirect = true)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";

            request.Timeout = Convert.ToInt32(new TimeSpan(0, 5, 0).TotalMilliseconds);
            request.Method = httpMethod;
            return request;
        }
        public ActionResult Report(string APIKEY)
        {

            WorkOrderGet("https://api.megaventory.com/v2017a/json/reply/WorkOrderGet?APIKEY=" + APIKEY);
            ProductGet("https://api.megaventory.com/v2017a/json/reply/ProductGet?APIKEY=" + APIKEY);
            Session["WorkOrders"] = WorkOrders;
            Session["Products"] = Products;

            return View();
        }


        public ActionResult Index()
        {
            CustomerBalanceDetail cbd = new CustomerBalanceDetail();
            
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
