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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            return Ok(await _clientService.GetClient(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetClient()
        {
            return Ok(await _clientService.GetAllClient());
        }
        

        [HttpGet("OthersContacts/{id}")]
        public async Task<IActionResult> GetPossiblesContacts(Guid id)
        {
            return Ok(await _clientService.GetPossiblesContactsOfClient(id));
        }
        
        [HttpPost("AddContact")]
        public async Task<IActionResult> AddContact(ContactViewModel bindContactViewModel)
        {
            await _clientService.AddContact(bindContactViewModel);
            return Ok();
        }

        [HttpDelete("RemoveContact")]
        public async Task<IActionResult> RemoveContact(ContactViewModel bindContactViewModel)
        {
            await _clientService.RemoveContact(bindContactViewModel);
            return Ok();
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
            return Created("", clientViewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            try
            {
                await _clientService.DeleteClient(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            try
            {
                await _clientService.ChangeStatus(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }
    }
}