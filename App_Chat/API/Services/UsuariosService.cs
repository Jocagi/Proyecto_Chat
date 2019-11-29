using API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace API.Services
{
    public class UsuariosService
    {
        private readonly IMongoCollection<Usuarios> _usuarios;

        public UsuariosService (IUsuariosDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _usuarios = database.GetCollection<Usuarios>(settings.UsuariosCollectionName);

        }

        public List<Usuarios> Get() =>
            _usuarios.Find(usuario => true).ToList();
            
    }
}
