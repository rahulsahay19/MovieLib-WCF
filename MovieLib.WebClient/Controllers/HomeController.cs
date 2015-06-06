using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using MovieLib.Contracts;
using MovieLib.Proxies;

namespace MovieLib.WebClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private MovieClient proxyClient = null;

        [HttpGet]
        public ActionResult MovieList()
        {
            using (((WindowsIdentity)User.Identity).Impersonate())
            {
                proxyClient = new MovieClient("1stEP");
                proxyClient.Open();
                IEnumerable<MovieData> data = proxyClient.GetDirectorNames();
                proxyClient.Close();
            }
            return View();
        }
    }
}