﻿using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosService _UsuariosService;

        public UsuariosController(UsuariosService UsuariosService)
        {
            _UsuariosService = UsuariosService;
        }
        //http://localhost:<port>/api/Usuarios
        // GET: api/Usuarios
        [HttpGet("{id:length(24)}", Name = "GetUsuario")]

        public ActionResult<Usuarios> Get(string id)
        {
            var Usuario = _UsuariosService.Get(id);

            if (Usuario==null)
            {
                return NotFound();
            }
            return Usuario;
        }

        [HttpPost]
        public ActionResult<Usuarios> Create(Usuarios Usuario)
        {
            _UsuariosService.Create(Usuario);
            return CreatedAtRoute("GetUsuario", new { id = Usuario.Id.ToString() }, Usuario);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Usuarios UsuarioIn)
        {
            var Usuario = _UsuariosService.Get(id);

            if(Usuario == null)
            {
                return NotFound();
            }

            _UsuariosService.Update(id, UsuarioIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var Usuario = _UsuariosService.Get(id);

            if(Usuario==null)
            {
                return NotFound();
            }
            _UsuariosService.Remove(Usuario.Id);

            return NoContent();
        }
    }
}