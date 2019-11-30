using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using MongoDB.Driver;

namespace API.Services
{
    public class UserService
    {
        private readonly IMongoCollection<Usuario> _Users;

        public UserService(IUserDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Users = database.GetCollection<Usuario>(settings.UsersCollectionName);
        }

        public List<Usuario> Get() =>
            _Users.Find(User => true).ToList();

        public Usuario Get(string id) =>
            _Users.Find<Usuario>(User => User.Id == id).FirstOrDefault();

        public Usuario Create(Usuario User)
        {
            _Users.InsertOne(User);
            return User;
        }

        public void Update(string id, Usuario UserIn) =>
            _Users.ReplaceOne(User => User.Id == id, UserIn);

        public void Remove(Usuario UserIn) =>
            _Users.DeleteOne(User => User.Id == UserIn.Id);

        public void Remove(string id) =>
            _Users.DeleteOne(User => User.Id == id);
    }
}
