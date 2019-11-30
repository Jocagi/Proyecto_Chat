using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class Usuarios
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
    }
}
