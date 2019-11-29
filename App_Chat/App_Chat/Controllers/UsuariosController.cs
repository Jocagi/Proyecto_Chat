using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using App_Chat.Models;

namespace App_Chat.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            IEnumerable<UsuariosController> Usuario = null;
            using (var usuario = new HttpClient())
            {
                usuario.BaseAddress = new Uri("http://localhost:44316/");
                var responseTask = usuario.GetAsync("usuario");
                responseTask.Wait();

                var result = responseTask.Result;
                if(result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadAsAsync<IList<UsuariosController>>();
                    readJob.Wait();
                    Usuario = readJob.Result;
                }
                else
                {
                    Usuario = Enumerable.Empty<UsuariosController>();
                    ModelState.AddModelError(string.Empty, "No se puede ingresar el usuario");
                }
            }
            return View(Usuario);
        }
    }
}