using AutoMapper;
using CrossCouting;
using Infra.Repository.Interfaces;
using Models;
using Models.ViewModel;
using Services.Interfaces;

namespace Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IClientTopicRepository _clientTopicRepository;
        private readonly IQueueService _queueService;
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;
        public ClientService
            (
            IClientRepository clientRepository,
            ITopicRepository topicRepository,
            IClientTopicRepository clientTopicRepository,
            IQueueService queueService,
            ITopicService topicService,
            IMapper mapper
            )
        {
            _clientRepository = clientRepository;
            _topicRepository = topicRepository;
            _clientTopicRepository = clientTopicRepository;
            _queueService = queueService;
            _topicService = topicService;
            _mapper = mapper;
        }

        public async Task CreateClient(CreateClientViewModel clientViewModel)
        {
            if (_clientRepository.Any(x => x.Name == clientViewModel.Name))
            {
                throw new AlreadyExistExpection("Client já existe");
            }
            var queue = await _queueService.
                CreateQueue(new CreateQueueViewModel() { Name = clientViewModel.Name + ".QUEUE" });

            Client newClient = new()
            {
                Name = clientViewModel.Name,
                QueueId = queue.Id,
            };
            _clientRepository.Insert(newClient);
            _clientRepository.Commit();
        }

        public async Task DeleteClient(Guid id)
        {
            Client client = _clientRepository.GetById(id);

            if (client == null)
            {
                throw new NotFoundException("O Client não foi encontrado.");
            }
            await _queueService.DeleteQueue(client.QueueId);
        }

        public async Task<List<ReadClientViewModel>> GetAllClient()
        {
            return _mapper.Map<List<ReadClientViewModel>>(await _clientRepository.GetAllInclude());
        }

        public async Task<ReadClientViewModel> GetClient(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task SubscribeTopic(SubscribeTopicViewModel subscribeTopicViewModel)
        {
            var client = _clientRepository.GetById(subscribeTopicViewModel.ClientId);
            var topic = _topicRepository.GetById(subscribeTopicViewModel.TopicId);

            if (client == null || topic == null)
            {
                throw new NotFoundException("Cliente ou Topico não existe");
            }
            List<CreateQueueViewModel> createQueueViewModels = new List<CreateQueueViewModel>()
            {
                new()
                {
                    Name = client.Queue.Name
                }
            };
            await _topicService.TopicBindQueues(createQueueViewModels, topic);
            ClientTopic newClientTopic = new()
            {
                ClientId = subscribeTopicViewModel.ClientId,
                TopicId = subscribeTopicViewModel.TopicId
            };
            _clientTopicRepository.Insert(newClientTopic);
            _clientTopicRepository.Commit();
        }
    }
}
