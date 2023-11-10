using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using Services.Interfaces;

namespace GerenciadorBrokerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IClientService _clientService;
        public MessageController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateMessageViewModel createMessageViewModel)
        {
            await _clientService.PostMessage(createMessageViewModel);
            return Ok();
        }
    }
}
