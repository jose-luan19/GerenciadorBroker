using CrossCouting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using Services.Interfaces;

namespace GerenciadorBrokerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService _queueService;
        public QueueController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQueue()
        {
            return Ok(await _queueService.GetAllQueues());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateQueue([FromQuery] CreateQueueViewModel queue)
        {
            try
            {
                await _queueService.CreateQueue(queue);
            }
            catch (AlreadyExistExpection ex)
            {
                return BadRequest(ex.Message);
            }
            return Created("", queue);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQueue(Guid id)
        {
            try
            {
                await _queueService.DeleteQueue(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(DeleteException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
    }
}