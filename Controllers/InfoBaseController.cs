using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using puceAsk_dev1.Models;

namespace puceAsk_dev1.Controllers
{
    public class InfoBaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if(User != null)
            {
                var context = new ApplicationDbContext();
                var username = User.Identity.Name;


                if (!string.IsNullOrEmpty(username))
                {
                    var user = context.Users.SingleOrDefault(u => u.UserName == username);
                    int cuenta = user.Cuenta.SaldoCuenta;
                    byte[] foto = user.Foto;
                    string imreBase64Data = Convert.ToBase64String(foto);
                    string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
                    System.Diagnostics.Debug.WriteLine(imgDataURL);
                    ViewData.Add("Puntaje", cuenta);
                    ViewData.Add("Foto", imgDataURL);
                }
            }
            base.OnActionExecuted(filterContext);

        }

        public InfoBaseController()
        {

        }
    }
}