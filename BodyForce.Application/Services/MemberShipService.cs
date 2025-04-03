using AutoMapper;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
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
                             SubscriptionName = s.Name,
                             Payment = p.PaymentMethod
                         }).ToList();
            return result;

        }
        public async Task<MembershipDto> GetMemberShip(int userId)
        {
            var membership = (await _unitOfWork.Repository<MemberShip>().GetByConditionAsync(x => x.UserId == userId)).LastOrDefault();
            var result = _mapper.Map<MembershipDto>(membership);
            if (membership != null && membership.SubscriptionTypeId != 0)
            {
                var payment = (await _unitOfWork.Repository<Payment>().GetByConditionAsync(x => x.UserId == userId && x.MemberShipId == membership.MemberShipId)).FirstOrDefault();
                result.PaymentDate = payment.PaymentDate;
                result.PaymentMethod = payment.PaymentMethod;
                result.AmountPaid = payment.AmountPaid;
                result.Notes = payment.Notes;
            }
            return result;
        }
        public async Task<IdentityResult> AddMemberShip(MembershipDto membershipDto)
        {
            try
            {               
                var subscriptionType = await _unitOfWork.Repository<SubscriptionType>().GetByIdAsync(membershipDto.SubscriptionTypeId);

                var membership = new MemberShip()
                {
                    MemberShipId= membershipDto.MemberShipId,
                    UserId = membershipDto.UserId,
                    SubscriptionTypeId = membershipDto.SubscriptionTypeId,
                    StartDate = membershipDto.StartDate,
                    EndDate = membershipDto.StartDate?.AddDays(subscriptionType.DurationInDays),
                    Status = true,
                    RenewalDate = (membershipDto.StartDate?.AddDays(subscriptionType.DurationInDays))?.AddDays(1)
                };           

                if (membershipDto.MemberShipId != 0)
                {
                    membership.UpdatedOn = DateTime.Now;
                    membership = await _unitOfWork.Repository<MemberShip>().Update(membership);
                }
                else
                {
                    membership.CreatedOn = DateTime.Now;
                    membership = await _unitOfWork.Repository<MemberShip>().AddAsync(membership);
                }
                await  _unitOfWork.Repository<MemberShip>().SaveChangesAsync();

                var payment = await _unitOfWork.Repository<Payment>().AddAsync(new Payment()
                {
                    MemberShipId = membershipDto.MemberShipId,
                    UserId = membershipDto.UserId,
                    AmountPaid = membershipDto.AmountPaid ?? 0,
                    PaymentDate = membershipDto.PaymentDate ?? DateTime.MinValue,
                    PaymentMethod = membershipDto.PaymentMethod,
                    Notes = membershipDto.Notes ?? string.Empty,
                    CreatedOn = DateTime.Now,
                });

                await _unitOfWork.Repository<Payment>().SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = ex.Message.ToString()
                });
            }
        }        
    }
}
