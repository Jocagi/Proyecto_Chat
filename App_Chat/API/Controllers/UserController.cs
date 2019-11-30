using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _UserService;

        public UsersController(UserService UserService)
        {
            _UserService = UserService;
        }

        [HttpGet]
        public ActionResult<List<Usuario>> Get() =>
            _UserService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<Usuario> Get(string id)
        {
            var User = _UserService.Get(id);

            if (User == null)
            {
                return NotFound();
            }

            return User;
        }

        [HttpPost]
        public ActionResult<Usuario> Create(Usuario User)
        {
            _UserService.Create(User);

            return CreatedAtRoute("GetUser", new { id = User.Id.ToString() }, User);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Usuario UserIn)
        {
            var User = _UserService.Get(id);

            if (User == null)
            {
                return NotFound();
            }

            _UserService.Update(id, UserIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var User = _UserService.Get(id);

            if (User == null)
            {
                return NotFound();
            }

            _UserService.Remove(User.Id);

            return NoContent();
        }
    }
}