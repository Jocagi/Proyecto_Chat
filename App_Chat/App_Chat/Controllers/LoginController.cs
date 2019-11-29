using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using App_Chat.Models;

using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.WebPages;
//using Microsoft.AspNetCore.Http;

namespace App_Chat.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            ViewBag.Message = "";
            HttpContext.Response.Cookies.Add(new HttpCookie("language", "SPANISH"));
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            
            string strUrl = String.Format("https://localhost:44316/api/token");

            string postUrl = $"{strUrl}/?username={username}&password={password}";
            
            WebRequest requestObjPost = WebRequest.Create(postUrl);
            requestObjPost.Method = "POST";
            requestObjPost.ContentType = "application/json";

            try
            {
                using (var streamWriter = new StreamWriter(requestObjPost.GetRequestStream()))
                {
                    streamWriter.Write("");
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)requestObjPost.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();

                        //Guardar cookie con el token de inicio de sesion
                        HttpContext.Response.Cookies.Add(new HttpCookie("userID", result));
                        
                        return RedirectToAction("Index", "Chat");
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.Message = "Usuario o contraseña incorrecto";
                
                return View();
            }
        }
    }
}