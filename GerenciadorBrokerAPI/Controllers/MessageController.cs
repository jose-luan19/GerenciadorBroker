using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using Services.Interfaces;

namespace GerenciadorBrokerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService clientService)
        {
            _messageService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageViewModel createMessageViewModel)
        {
            await _messageService.PostMessage(createMessageViewModel);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCountMessage()
        {
            return Ok(await _messageService.GetCountMessagesInQueues());
        }
    }
}
