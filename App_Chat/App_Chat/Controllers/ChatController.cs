using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using App_Chat.Models;
using Microsoft.Ajax.Utilities;
using Recursos;
using Utilities;

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
                int cifradoValue = Int16.Parse(HttpContext.Request.Cookies["cifrado"].Value);

                SDES cipher = new SDES();
                string mensajeCifrado = cipher.CifrarTexto(mensaje, cifradoValue);
                
                MensajeModelo nuevo = new MensajeModelo();
                nuevo.mensaje = mensajeCifrado;
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
            string usuario = id.Replace(" ", "");
            IEnumerable<Message> misMensajes = null;

            try
            {
                string token = HttpContext.Request.Cookies["userID"].Value;
                int cifradoValue = Int16.Parse(HttpContext.Request.Cookies["cifrado"].Value);

                using (var client = new HttpClient())
                {
                    string adress = "/api/chat/" + usuario;
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

                        SDES cipher = new SDES();

                        //Convertir lista a html
                        string html = " ";

                        foreach (var item in misMensajes)
                        {
                            //Descifrar
                            string mensaje = cipher.DescifrarTexto(item.texto, cifradoValue);

                            if (item.recibido)
                            {
                                html += $"<li class=\"replies\"><p>{mensaje}</p></li>";
                            }
                            else
                            {
                                html += $"<li class=\"sent\"><p>{mensaje}</p></li>";
                            }
                        }

                        return html;
                    }
                    else
                    {
                        return "Error en el servidor";
                    }
                }
            }
            catch (Exception e)
            {
                return "Porfavor autenticarse de nuevo";
                throw;
            }
        }
    }
}