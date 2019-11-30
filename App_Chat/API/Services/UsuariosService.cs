using API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace API.Services
{
    public class UsuariosService
    {
        private readonly IMongoCollection<Usuarios> _usuarios;

        public UsuariosService(IUsuariosDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _usuarios = database.GetCollection<Usuarios>(settings.UsuariosCollectionName);
        }

        public List<Usuarios> Get() =>
            _usuarios.Find(usuario => true).ToList();

        public Usuarios Get(string id) =>
            _usuarios.Find<Usuarios>(usuario => usuario.Id == id).FirstOrDefault();

        public Usuarios Create (Usuarios usuario)
        {
            _usuarios.InsertOne(usuario);
            return usuario;
        }
        public void Update(string id, Usuarios usuariosIn) =>
            _usuarios.ReplaceOne(usuario => usuario.Id == id, usuariosIn);

        public void Remove(Usuarios usuariosIn) =>
            _usuarios.DeleteOne(usuarios => usuarios.Id == usuariosIn.Id);

        public void Remove(string id) =>
            _usuarios.DeleteOne(usuario => usuario.Id == id);
    }
}
