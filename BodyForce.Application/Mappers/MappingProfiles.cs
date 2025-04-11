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

            CreateMap<SignUpDto, User>();
            CreateMap<User, SignUpDto>()
                .ForMember(dest => dest.PhoneNo, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<MembershipDto, MemberShip>();
            CreateMap<MemberShip, MembershipDto>();
            
            CreateMap<MembersDto, MembershipView>();
            CreateMap<MembershipView, MembersDto>();


        }
    }
}
