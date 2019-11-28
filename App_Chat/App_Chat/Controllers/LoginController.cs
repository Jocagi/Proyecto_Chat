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
        public ActionResult Login(string username, string password)
        {
            //To Do... Validar en Base de Datos y redirigir al chat

            string strUrl = String.Format("https://localhost:44316/api/token");

            string postUrl = $"{strUrl}/?username={username}&password={password}";
            
            WebRequest requestObjPost = WebRequest.Create(postUrl);
            requestObjPost.Method = "POST";
            requestObjPost.ContentType = "application/json";
            
            using (var streamWriter = new StreamWriter(requestObjPost.GetRequestStream()))
            {
                streamWriter.Write("");
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse) requestObjPost.GetResponse();
                
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }

            return View();
        }
    }
}