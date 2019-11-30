using System;
using System.Collections.Generic;
using System.IO;
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
                nuevo.esArchivo = false;

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

        [HttpGet]
        public ActionResult SubirArchivo(string id)
        {
            Contacto user = new Contacto();
            user.username = id;

            return View(user);
        }
        [HttpPost]
        public ActionResult SubirArchivo(HttpPostedFileBase file, string persona)
        {
            try
            {
                string token = HttpContext.Request.Cookies["userID"].Value;
                int cifradoValue = Int16.Parse(HttpContext.Request.Cookies["cifrado"].Value);

                //Subir archivo al servidor
                string path = Path.Combine(Directories.directorioUploads, Path.GetFileName(file.FileName) ?? "");
                UploadFile(path, file);
                
                //Comprimir
                string rutaComprimido = LZW.comprimirArchivo(path, Directories.directorioTemporal);

                //Cifrar
                SDES cipher = new SDES();
                string rutaCifrado = cipher.CifrarArchivo(rutaComprimido,Directories.directorioArchivos, cifradoValue);

                //Pasar ruta como parametro
                string rutaAmigable = rutaCifrado.Replace("/", "~");
                rutaAmigable = rutaAmigable.Replace(@"\", "~");

                string mensaje = $"<a href=\"/Chat/Descargar?path={rutaAmigable}\" onclick=\"clickAndDisable(this);\">{file.FileName}</a>";

                MensajeModelo nuevo = new MensajeModelo();
                nuevo.mensaje = cipher.CifrarTexto(mensaje, cifradoValue);
                nuevo.receptor = persona;
                nuevo.receptor = nuevo.receptor.Replace(" ", "");
                nuevo.esArchivo = true;

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

                return RedirectToAction("Index", "Chat");
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public void UploadFile(string path, HttpPostedFileBase file)
        {
            //Subir archivos al servidor

            if (file != null && file.ContentLength > 0)
                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    file.SaveAs(path);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message;
                }
            else
            {
                ViewBag.Message = "No ha especificado un archivo.";
            }
        }
        
        public ActionResult Descargar(string path)
        {
            //Recuperar path
            path = path.Replace("~", "/");

            int cifradoValue = Int16.Parse(HttpContext.Request.Cookies["cifrado"].Value);

            //Descifrar
            SDES cipher = new SDES();
            string rutaCifrado = cipher.DescifrarArchivo(path, Directories.directorioTemporal, cifradoValue);

            //Descomprimir
            string rutaComprimido = LZW.descomprimirArchivo(rutaCifrado, Directories.directorioDescargas);

            //Descargar

            if (!String.IsNullOrEmpty(rutaComprimido))
            {
                byte[] filedata = System.IO.File.ReadAllBytes(rutaComprimido);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = Path.GetFileName(rutaComprimido),
                    Inline = true,
                };
                
                Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(filedata, "application/force-download");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}