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
        public ActionResult<Chat> Get(string id)
        {
            var Chat = _ChatService.Get(id);

            if (Chat == null)
            {
                return NotFound();
            }

            return Chat;
        }

        [HttpPost]
        public ActionResult<Chat> Create(Chat Chat)
        {
            _ChatService.Create(Chat);

            return CreatedAtRoute("GetChat", new { id = Chat.Id.ToString() }, Chat);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Chat ChatIn)
        {
            var Chat = _ChatService.Get(id);

            if (Chat == null)
            {
                return NotFound();
            }

            _ChatService.Update(id, ChatIn);

            return NoContent();
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
