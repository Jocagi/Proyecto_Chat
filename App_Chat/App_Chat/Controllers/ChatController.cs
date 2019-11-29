using System;
using System.Collections.Generic;
using System.Linq;
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