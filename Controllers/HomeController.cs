using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace puceAsk_dev1.Controllers
{
    public class HomeController : InfoBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Tu página de aplicación de descripción.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Tu página de contacto.";

            return View();
        }
    }
}