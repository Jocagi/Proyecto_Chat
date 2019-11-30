using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id;

        public string chatID { get; set; }
        public bool recibido { get; set; }
        public string texto { get; set; }
        public bool esArchivo { get; set; }
        public string path { get; set; }

        public Message(string id, bool r, string t, bool eA, string p)
        {
            chatID = id;
            recibido = r;
            texto = t;
            esArchivo = eA;
            path = p;
        }
    }
}
