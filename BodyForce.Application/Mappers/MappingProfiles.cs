﻿using System;
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
            CreateMap<User, SignUpDto>();

            CreateMap<User, UserDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<EditMemberDto, User>();
            CreateMap<User, EditMemberDto>();

            CreateMap<MembershipDto, MemberShip>();
            CreateMap<MemberShip, MembershipDto>();
            
            CreateMap<MembersDto, MembershipView>();
            CreateMap<MembershipView, MembersDto>();

            CreateMap<MembersDto, MemberShip>();
            CreateMap<MemberShip, MembersDto>();

            CreateMap<MembershipDto, Payment>();
            CreateMap<Payment, MembershipDto>();


        }
    }
}
