using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Chat.Models
{
    public class Contacto
    {
        public string username { get; set; }
        public string imagen { get; set; }

        public Contacto()
        { }

        public Contacto(string usr, string img)
        {
            this.username = usr;
            this.imagen = img;
        }
    }
}