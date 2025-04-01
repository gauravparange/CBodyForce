using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
namespace BodyForce
{ 
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<SubscriptionType, SubscriptionDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsActive ? "Active" : "Not Active"));
            CreateMap<SubscriptionDto, SubscriptionType>();

            CreateMap<SignUpDto, User>()
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow))
            //.ForMember(dest => dest.DOJ, opt => opt.MapFrom(src => DateTime.UtcNow))
            //.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Assuming this comes from the context
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());
            CreateMap<User, SignUpDto>()
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.PhoneNumber));


        }
    }
}
