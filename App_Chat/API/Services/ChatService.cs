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
        private readonly IMongoCollection<Chat> _Chat;

        public ChatService(IChatDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Chat = database.GetCollection<Chat>(settings.ChatCollectionName);
        }

        public List<Chat> Get() =>
            _Chat.Find(Chat => true).ToList();

        //Obtener chat por nombre
        public Chat Get(string id) =>
            _Chat.Find<Chat>(Chat => Chat.ChatID == id).FirstOrDefault();

        //Agregar un mensaje
        public void Post(Message mensaje, Chat chat)
        {
            chat.mensajes.Add(mensaje);
            _Chat.ReplaceOne(Chat => Chat.ChatID == chat.ChatID, chat);
        }
        
        public void Update(string id, Chat ChatIn) =>
            _Chat.ReplaceOne(Chat => Chat.Id == id, ChatIn);

        public void Remove(Chat ChatIn) =>
            _Chat.DeleteOne(Chat => Chat.Id == ChatIn.Id);

        public void Remove(string id) =>
            _Chat.DeleteOne(Chat => Chat.Id == id);
    }
}

