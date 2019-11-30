using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        private readonly ChatService _ChatService;

        public ChatController(ChatService ChatService)
        {
            _ChatService = ChatService;
        }

        [HttpGet]
        public ActionResult<List<Message>> Get() =>
            _ChatService.Get();

        [HttpGet("{id}", Name = "GetChat")]
        [Authorize]
        public ActionResult<List<Message>> Get(string id)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            string emisor = claimsIdentity.Name;
            string busqueda = emisor + id;
            
            var Chat = _ChatService.Get(busqueda);

            if (Chat == null)
            {
                return NotFound();
            }

            return Chat;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Chat> NewMessage(MensajeModelo received)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            string emisor = claimsIdentity.Name;
            string id = emisor + received.receptor;
            string id2 = received.receptor + emisor;

            try
            {
                //Post en el chat de ambas personas
                _ChatService.Post(new Message(id,false, received.mensaje, received.esArchivo, ""));
                _ChatService.Post(new Message(id2,true, received.mensaje, received.esArchivo, ""));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id, string mensaje)
        {
            var Chat = _ChatService.Get(id);

            if (Chat == null)
            {
                return NotFound();
            }

            _ChatService.Remove(id, mensaje);

            return NoContent();
        }
    }
}
