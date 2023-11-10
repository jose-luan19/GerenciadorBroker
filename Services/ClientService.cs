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
        private readonly IQueueService _queueService;
        private readonly IMapper _mapper;
        public ClientService
            (
            IClientRepository clientRepository,
            IQueueService queueService,
            IMapper mapper
            )
        {
            _clientRepository = clientRepository;
            _queueService = queueService;
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
            var clients = await _clientRepository.GetAllInclude();
            var clientViewModel = _mapper.Map<List<ReadClientViewModel>>(clients);

            return clientViewModel;
        }

        public async Task<ReadClientViewModel> GetClient(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
