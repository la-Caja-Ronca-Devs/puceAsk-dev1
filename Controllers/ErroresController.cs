using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace puceAsk_dev1.Controllers
{
    public class ErroresController : Controller
    {
        // GET: Errores
        public ActionResult Error404()
        {
            return View();
        }
    }
}