using CrossCouting;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using Services.Interfaces;

namespace GerenciadorBrokerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTopic()
        {
            return Ok(await _topicService.GetAllTopics());
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic(CreateTopicViewModel topicViewModel)
        {
            try
            {
                await _topicService.CreateTopic(topicViewModel);
            }
            catch (AlreadyExistExpection ex)
            {
                return BadRequest(ex.Message);
            }
            return Created("", topicViewModel.Name);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTopic(string topicName)
        {
            try
            {
                await _topicService.DeleteTopic(topicName);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }

    }
}
