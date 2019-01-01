using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGF.Models;

namespace SGF.Controllers
{
    public class UserController : Controller
    {

        public ActionResult UserPage()
        {
            if (Session["User"] == null) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        public ActionResult Sair() {
            Session["User"] = null;
            Session["Nome"] = null;
           return RedirectToAction("Index", "Home");
        }

        
    }
}