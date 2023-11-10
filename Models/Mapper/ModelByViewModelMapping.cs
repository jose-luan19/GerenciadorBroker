using AutoMapper;
using Models.ViewModel;

namespace Models.Mapper
{
    public class ModelByViewModelMapping : Profile
    {
        public ModelByViewModelMapping()
        {
            CreateMap<ReadClientViewModel, Client>().ReverseMap();
            CreateMap<ReadQueueViewModel, Queues>().ReverseMap();
            //CreateMap<Topic, ReadAllTopicsViewModel>().ReverseMap();
            CreateMap<QueueTopic, ReadQueueViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Queues.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Queues.Name));

            CreateMap<Topic, ReadAllTopicsViewModel>()
                .ForMember(dest => dest.Queues, opt => opt.MapFrom(src => src.QueueTopics.Select(qt => qt.Queues)));
        }
    }
}
