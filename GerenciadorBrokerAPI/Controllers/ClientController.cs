using CrossCouting;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using Services.Interfaces;

namespace GerenciadorBrokerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /*[HttpGet("{id}")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            return Ok(await _clientService.GetClient(id));
        }*/

        [HttpGet]
        public async Task<IActionResult> GetClient()
        {
            return Ok(await _clientService.GetAllClient());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromQuery] CreateClientViewModel clientViewModel)
        {
            try
            {
                await _clientService.CreateClient(clientViewModel);
            }
            catch (AlreadyExistExpection ex)
            {
                return BadRequest(ex.Message);
            }
            return Created("", clientViewModel.Name);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            try
            {
                await _clientService.DeleteClient(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }

        [HttpPost("Subscribe")]
        public async Task<IActionResult> SubscribeClientInTopic(SubscribeTopicViewModel subscribeTopicViewModel)
        {
            try
            {
                await _clientService.SubscribeTopic(subscribeTopicViewModel);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }
    }
}