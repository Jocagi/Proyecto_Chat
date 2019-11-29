using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Message
    {
        public string emisor { get; set; }
        public string texto { get; set; }
        public DateTime fecha { get; set; }
        public bool esArchivo { get; set; }
        public string path { get; set; }
    }
}
