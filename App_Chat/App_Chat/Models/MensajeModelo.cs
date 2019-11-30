using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Chat.Models
{
    public class MensajeModelo
    {
        public string mensaje { get; set; }
        public string receptor { get; set; }
        public bool esArchivo { get; set; }
    }
}