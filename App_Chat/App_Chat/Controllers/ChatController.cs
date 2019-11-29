using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using App_Chat.Models;
using Microsoft.Ajax.Utilities;

namespace App_Chat.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View(obtenerContactos());
        }

        [HttpPost]
        public ActionResult Index(string persona, string mensaje)
        {
            try
            {
                string token = HttpContext.Request.Cookies["userID"].Value;

                MensajeModelo nuevo = new MensajeModelo();
                nuevo.mensaje = mensaje;
                nuevo.receptor = persona;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44316/api/chat/NewMessage/");
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    var postJob = client.PostAsJsonAsync<MensajeModelo>("", nuevo);
                    postJob.Wait();

                    var postResult = postJob.Result;
                    if (postResult.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Login");
                    }
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");
            }
        }

        private ListaDeContactos obtenerContactos()
        {
            List<Contacto> contactos = new List<Contacto>() {new Contacto("Jose", "~/Images/Gaticornio.png"), new Contacto("Genesis", "~/Images/pato.jpg") };
            ListaDeContactos nuevo = new ListaDeContactos(contactos);
            nuevo.usuario = "Bob";
            nuevo.foto = "~/Images/pato.jpg";

            return nuevo;
        }

        [HttpGet]
        public string Mensajes(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return $"<li class=\"sent\"><p>{id}</p> </li> <li class=\"replies\"><p>Mucho gusto</p></li>";
            }
            else
            {
                return "";
            }
        }
    }
}