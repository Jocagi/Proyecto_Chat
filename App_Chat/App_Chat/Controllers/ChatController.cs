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
                nuevo.receptor = nuevo.receptor.Replace(" ", "");
                

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44316/api/chat");
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    var postJob = client.PostAsJsonAsync<MensajeModelo>("chat", nuevo);
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

            IEnumerable<Message> misMensajes = null;

            try
            {
                string token = HttpContext.Request.Cookies["userID"].Value;

                using (var client = new HttpClient())
                {
                    string adress = "/api/chat/" + id;
                    client.BaseAddress = new Uri("https://localhost:44316/api/chat");
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                    var responseTask = client.GetAsync(adress);

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readJob = result.Content.ReadAsAsync<IList<Message>>();
                        readJob.Wait();
                        misMensajes = readJob.Result;

                        //Convertir lista a html
                        string html = "";

                        foreach (var item in misMensajes)
                        {
                            if (item.recibido)
                            {
                                html += $"<li class=\"replies\"><p>{item.texto}</p></li>";
                            }
                            else
                            {
                                html += $"<li class=\"sent\"><p>{item.texto}</p></li>";
                            }
                        }

                        return html;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch (Exception e)
            {
                return "";
                throw;
            }
        }
    }
}