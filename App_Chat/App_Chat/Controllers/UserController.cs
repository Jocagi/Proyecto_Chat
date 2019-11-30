using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using App_Chat.Models;
using Utilities;

namespace App_Chat.Controllers
{
    public class UserController : Controller
    {
        public ActionResult CrearUsuario()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CrearUsuario(Usuario user)
        {
            bool repetido = false;

            List<Usuario> users = obtenerPersonas();
            foreach (var item in users)
            {
                if (user.username == item.username)
                {
                    repetido = true;
                    break;
                }
            }

            if (!repetido)
            {


                try
                {
                    //Cifrar Contrase;a

                    int cifradoValue = user.password.Length;

                    SDES cipher = new SDES();
                    string contrasenaCifrado = cipher.CifrarTexto(user.password, cifradoValue);

                    user.password = contrasenaCifrado;


                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:44316/api/users");

                        var postJob = client.PostAsJsonAsync<Usuario>("users", user);
                        postJob.Wait();

                        var postResult = postJob.Result;
                        if (postResult.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Login", "Login");
                        }
                        else
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('Error');</script>");
                        }
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Prueba con otro nombre');</script>");
            }
        }

        private List<Usuario> obtenerPersonas()
        {
            List<Usuario> data = new List<Usuario>();
            
            try
            {
                int cifradoValue = Int16.Parse(HttpContext.Request.Cookies["cifrado"].Value);

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

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
