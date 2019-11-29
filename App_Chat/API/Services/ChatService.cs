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

        public Chat Get(string id) =>
            _Chat.Find<Chat>(Chat => Chat.Id == id).FirstOrDefault();

        public Chat Create(Chat Chat)
        {
            _Chat.InsertOne(Chat);
            return Chat;
        }

        public void Update(string id, Chat ChatIn) =>
            _Chat.ReplaceOne(Chat => Chat.Id == id, ChatIn);

        public void Remove(Chat ChatIn) =>
            _Chat.DeleteOne(Chat => Chat.Id == ChatIn.Id);

        public void Remove(string id) =>
            _Chat.DeleteOne(Chat => Chat.Id == id);
    }
}

