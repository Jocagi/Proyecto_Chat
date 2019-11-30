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
            try
            {
                //Cifrar Contrase;a

                int cifradoValue = user.password.Length;

                SDES cipher = new SDES();
                string contrasenaCifrado = cipher.CifrarTexto(user.password, cifradoValue);

                user.password = contrasenaCifrado;


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44316/api/user");
                    
                    var postJob = client.PostAsJsonAsync<Usuario>("user", user);
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
