using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App_Chat.Models;

namespace App_Chat.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLogin user)
        {
            //To Do... Validar en Base de Datos y redirigir al chat
            return View();
        }
    }
}