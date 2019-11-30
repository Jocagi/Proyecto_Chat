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
using Microsoft.AspNetCore.Http;
using Utilities;

namespace App_Chat.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {

        Directories.directorioUploads = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
        Directories.directorioArchivos = System.Web.HttpContext.Current.Server.MapPath("~/Files/");
        Directories.directorioDescargas = System.Web.HttpContext.Current.Server.MapPath("~/Downloads/");

            ViewBag.Message = "";
            HttpContext.Response.Cookies.Add(new HttpCookie("language", "SPANISH"));
            HttpContext.Response.Cookies.Add(new HttpCookie("cifrado", "38"));
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            SDES cipher = new SDES();

            bool correcto = false;
            var personas = obtenerPersonas();
            foreach (var item in personas)
            {
                if (item.username == username && item.password == cipher.CifrarTexto(password, password.Length))
                {
                    correcto = true;
                    break;
                }
            }

            if (correcto)
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
            else
            {
                ViewBag.Message = "Credenciales incorrectas";
                return View();
            }
        }

        private List<Usuario> obtenerPersonas()
        {
            List<Usuario> data = new List<Usuario>();

            try
            {
                using (var client = new HttpClient())
                {
                    string adress = "/api/users/";
                    client.BaseAddress = new Uri("https://localhost:44316/api/users");
                    var responseTask = client.GetAsync(adress);

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readJob = result.Content.ReadAsAsync<IList<Usuario>>();
                        readJob.Wait();

                        var lista = readJob.Result;

                        foreach (var item in lista)
                        {
                            data.Add(item);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return data;
            }

            return data;
        }

    }
}