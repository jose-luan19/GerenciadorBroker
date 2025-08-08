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
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.MessagesRecevied))
                .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Contacts.Select(c => c.ClientContact)));

            CreateMap<Message, ReadMessageViewModel>()
                .ForMember(dest => dest.ClientSend, opt => opt.MapFrom(src => src.ClientSend)); ;

        }
    }
}
