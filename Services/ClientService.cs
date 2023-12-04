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
        private readonly IQueueRepository _queueRepository;
        private readonly IQueueService _queueService;
        private readonly IMapper _mapper;
        public ClientService
            (
            IClientRepository clientRepository,
            IQueueRepository queueRepository,
            IQueueService queueService,
            IMapper mapper
            )
        {
            _clientRepository = clientRepository;
            _queueRepository = queueRepository;
            _queueService = queueService;
            _mapper = mapper;
        }

        public async Task CreateClient(CreateClientViewModel clientViewModel)
        {
            if (_clientRepository.Any(x => x.Name == clientViewModel.Name))
            {
                throw new AlreadyExistExpection("Client já existe");
            }

            var queueName = clientViewModel.Name + ".QUEUE";
            var verifyQueueExists = _queueRepository.Find(x => x.Name == queueName);

            if (verifyQueueExists == null)
            {
                verifyQueueExists = await _queueService.
                CreateQueue(new CreateQueueViewModel() { Name = queueName });
            }

            Client newClient = new()
            {
                Name = clientViewModel.Name,
                QueueId = verifyQueueExists.Id,
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
            await _queueService.DeleteQueue(client.Queue);
        }

        public async Task<List<ReadAllClientViewModel>> GetAllClient()
        {
            return _mapper.Map<List<ReadAllClientViewModel>>(await _clientRepository.GetAllInclude());
        }

        public async Task<ReadDetailsClientViewModel> GetClient(Guid id)
        {
            var clientDetails = await _clientRepository.GetDetailsById(id);

            return _mapper.Map<ReadDetailsClientViewModel>(clientDetails);
        }


        public async Task ChangeStatus(Guid id)
        {
            var client = _clientRepository.GetById(id);
            client.IsOnline = !client.IsOnline;
            _clientRepository.Update(client);
            _clientRepository.Commit();
        }

    }
}
