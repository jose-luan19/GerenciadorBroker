using CrossCouting;
using Microsoft.AspNetCore.Mvc;
using Models;
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

        [HttpPost]
        public async Task<IActionResult> CreateQueue([FromQuery] CreateQueueViewModel queue)
        {
            await _queueService.CreateQueue(queue);
            return Created("",queue);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQueue([FromQuery] Guid id)
        {
            try
            {
                await _queueService.DeleteQueue(id);
            }catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }
    }
}