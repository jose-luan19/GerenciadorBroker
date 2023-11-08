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

        [HttpPost]
        public async Task<IActionResult> CreateQueue(CreateTopicViewModel topicViewModel)
        {
            await _topicService.CreateTopic(topicViewModel);
            return Created("", topicViewModel.Name);
        }

    }
}
