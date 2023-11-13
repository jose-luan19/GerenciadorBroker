using AutoMapper;
using Models.ViewModel;

namespace Models.Mapper
{
    public class ModelByViewModelMapping : Profile
    {
        public ModelByViewModelMapping()
        {
            CreateMap<Client, ReadAllClientViewModel>().ReverseMap();

            CreateMap<Queues, ReadAllQueueViewModel>()
                .ForMember(dest => dest.TopicsNames, opt => opt.MapFrom(src => src.QueueTopics.Select(qt => qt.Topic.Name).ToList()))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name));

            CreateMap<Topic, ReadAllTopicsViewModel>()
                .ForMember(dest => dest.ClientNames, opt => opt.MapFrom(src => src.ClientTopic.Select(qt => qt.Client.Name).ToList()));

/*            CreateMap<Client, ReadClientQueueViewModel>()
                .ForMember(dest => dest.ClientId , opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClientName , opt => opt.MapFrom(src => src.Name));*/

            /*CreateMap<Queues, ReadQueueViewModel>().ReverseMap();

            CreateMap<QueueTopic, ReadQueueViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Queues.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Queues.Name));*/

        }
    }
}
