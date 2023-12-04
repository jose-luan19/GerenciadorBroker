using AutoMapper;
using Models.ViewModel;

namespace Models.Mapper
{
    public class ModelByViewModelMapping : Profile
    {
        public ModelByViewModelMapping()
        {
            CreateMap<Client, ReadAllClientViewModel>().ReverseMap();

            CreateMap<Client, ReadDetailsClientViewModel>()
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.MessagesRecevied));

            CreateMap<Message, ReadMessageViewModel>();

        }
    }
}
