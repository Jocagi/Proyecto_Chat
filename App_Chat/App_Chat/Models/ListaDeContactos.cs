using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Chat.Models
{
    public class ListaDeContactos
    {
        public string usuario { get; set; }
        public string foto { get; set; } 

        public List<Contacto> Contactos { get; set; }

        public ListaDeContactos(List<Contacto> nombres)
        {
            Contactos = nombres;
        }
    }
}