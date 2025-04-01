using AutoMapper;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MemberShipService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MembersDto>> GetAllMembers()
        {

            var result = from u in (await _unitOfWork.Repository<User>().GetAllAsync())
                         join m in (await _unitOfWork.Repository<MemberShip>().GetAllAsync()) on u.Id equals m.UserId
                         select new MembersDto
                         {
                             MemberId = m.MemberShipId,
                             Name = u.FirstName + " " + u.LastName,
                             MemberCode = u.UserName,
                             PhoneNo = u.PhoneNumber,
                             DOJ = u.DOJ,
                             MembershipStatus = m.Status == true ? "Active" : "Not Active",
                             MembershipStartDate = m.StartDate,
                             MembershipEndDate = m.EndDate,
                             UserId = m.UserId,
                         };
            return result;
        }
        public async Task<SignUpDto> GetMember(int UserId)
        {
            var user = await _unitOfWork.Repository<User>().GetAllAsync();
            var result = _mapper.Map<SignUpDto>(user.FirstOrDefault(x => x.Id == UserId));
            
            return result;
        }
        public async Task<List<ViewMembershipDto>> ViewMemberShip(int UserId)
        {
            var result = (from m in (await _unitOfWork.Repository<MemberShip>().GetAllAsync())
                         join p in (await _unitOfWork.Repository<Payment>().GetAllAsync()) on m.MemberShipId equals p.MemberShipId
                         join s in (await _unitOfWork.Repository<SubscriptionType>().GetAllAsync()) on m.SubscriptionTypeId equals s.SubscriptionTypeId
                         where m.UserId == UserId
                         select new ViewMembershipDto
                         {
                             MemberShipId = m.MemberShipId,
                             UserId = m.UserId,
                             StartDate = m.StartDate,
                             EndDate = m.EndDate,
                             RenewalDate = m.RenewalDate,
                             SubscriptionTypeId = m.SubscriptionTypeId,
                             SubscriptionName = s.Name
                         }).ToList();
            return result;

        }
        //public async Task<IActionResult> GetMember(SignUpDto signUpDto)
        //{

        //}
        public class MemberShipDto
        {
            public int MemberShipId { get; set; }
            public int SubscriptionType { get; set; }
            public int UserId { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
    }
}
