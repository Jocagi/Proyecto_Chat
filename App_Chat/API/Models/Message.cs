using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Message
    {
        public bool recibido { get; set; }
        public string texto { get; set; }
        public bool esArchivo { get; set; }
        public string path { get; set; }

        public Message(bool r, string t, bool eA, string p)
        {
            recibido = r;
            texto = t;
            esArchivo = eA;
            path = p;
        }
    }
}
