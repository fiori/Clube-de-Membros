using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clube_de_Membros.Controllers
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
            ViewBag.Message = "What do you think?";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(string message)
        {
            ViewBag.Message = "Thanks for the feedback";

            return View();
        }
    }
}