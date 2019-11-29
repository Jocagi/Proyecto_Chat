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
        public ActionResult<List<Chat>> Get() =>
            _ChatService.Get();

        [HttpGet("{id:length(24)}", Name = "GetChat")]
        [Authorize]
        public ActionResult<List<Message>> Get(string id)
        {
            var Chat = _ChatService.Get(id);

            if (Chat == null)
            {
                return NotFound();
            }

            return Chat.mensajes;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<Chat> NewMessage(string mensaje, string receptor)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            string emisor = claimsIdentity.Name;
            string id = emisor + receptor;
            string id2 = receptor + emisor;

            try
            {
                //Post en el chat de ambas personas
                _ChatService.Post(new Message(false, mensaje, false, ""), _ChatService.Get(id));
                _ChatService.Post(new Message(true, mensaje, false, ""), _ChatService.Get(id2));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var Chat = _ChatService.Get(id);

            if (Chat == null)
            {
                return NotFound();
            }

            _ChatService.Remove(Chat.Id);

            return NoContent();
        }
    }
}
