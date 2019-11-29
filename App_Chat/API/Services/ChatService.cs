using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using MongoDB.Driver;

namespace API.Services
{
    public class ChatService
    {
        private readonly IMongoCollection<Message> _Chat;

        public ChatService(IChatDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Chat = database.GetCollection<Message>(settings.ChatCollectionName);
        }

        public List<Message> Get() =>
            _Chat.Find(Chat => true).ToList();

        //Obtener chat por nombre
        public List<Message> Get(string id) =>
            _Chat.Find<Message>(Chat => Chat.chatID == id).ToList();

        //Agregar un mensaje
        public void Post(Message mensaje)
        {
            _Chat.InsertOne(mensaje);
        }
        
        public void Remove(string id, string contenido) =>
            _Chat.DeleteOne(Chat => Chat.chatID == id && Chat.texto == contenido);
        
    }
}

