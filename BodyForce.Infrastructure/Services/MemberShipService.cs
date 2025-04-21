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
            try
            {
                return _mapper.Map<IEnumerable<MembersDto>>(await _unitOfWork.Repository<MembershipView>().GetAllAsync());
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
            
        }
        public async Task<EditMemberDto> GetMember(int UserId)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().GetByConditionAsync(x => x.IsDeleted == false);
            var result = _mapper.Map<EditMemberDto>(user.FirstOrDefault(x => x.Id == UserId));
            
            return result;
        }
        public async Task<List<ViewMembershipDto>> ViewMemberShip(int UserId)
        {
            var result = (from m in (await _unitOfWork.Repository<MemberShip>().GetByConditionAsync(x => x.IsDeleted == false))
                         join p in (await _unitOfWork.Repository<Payment>().GetByConditionAsync(x => x.IsDeleted == false)) on m.MemberShipId equals p.MemberShipId
                         join s in (await _unitOfWork.Repository<SubscriptionType>().GetByConditionAsync(x => x.IsDeleted == false)) on m.SubscriptionTypeId equals s.SubscriptionTypeId
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
                             Payment = p.PaymentMethod,
                             PaymentId = p.PaymentId
                         }).ToList();
            return result;

        }
        public async Task<MembershipDto> GetMemberShip(int userId,bool forAdd)
        {
            var membership = (await _unitOfWork.Repository<MemberShip>().GetByConditionAsync(x => x.UserId == userId)).LastOrDefault();
            var result = _mapper.Map<MembershipDto>(membership);
            if (membership != null && membership.SubscriptionTypeId != 0 && forAdd == false)
            {
                var payment = (await _unitOfWork.Repository<Payment>().GetByConditionAsync(x => x.UserId == userId && x.MemberShipId == membership.MemberShipId)).FirstOrDefault();
                result.PaymentDate = payment.PaymentDate;
                result.PaymentMethod = payment.PaymentMethod;
                result.AmountPaid = payment.AmountPaid;
                result.Notes = payment.Notes;
                result.PaymentId = payment.PaymentId;
            }
            else
            {
                result.StartDate = null;
                result.SubscriptionTypeId = 0;
            }
            return result;
        }
        public async Task<ResponseResult> AddMemberShip(MembershipDto membershipDto)
        {
            try
            {

                var allMembership = await _unitOfWork.Repository<MemberShip>().GetByConditionAsync(x => x.UserId == membershipDto.UserId && x.IsDeleted == false);

                var subscriptionType = await _unitOfWork.Repository<SubscriptionType>().GetByIdAsync(membershipDto.SubscriptionTypeId);

                if (allMembership.Count() == 1)
                {
                    var membership = allMembership.First();

                    _mapper.Map(membershipDto, membership);
                    membership.EndDate = membershipDto.StartDate?.AddDays(subscriptionType.DurationInDays);
                    membership.Status = true;
                    membership.RenewalDate = (membershipDto.StartDate?.AddDays(subscriptionType.DurationInDays))?.AddDays(1);
                    membership.UpdatedOn = DateTime.Now;

                    await _unitOfWork.Repository<MemberShip>().Update(membership);
                }
                else if(allMembership.Count() > 1)
                {
                    var membership = _mapper.Map<MemberShip>(membershipDto);

                    membership.EndDate = membershipDto.StartDate?.AddDays(subscriptionType.DurationInDays);
                    membership.Status = true;
                    membership.RenewalDate = (membershipDto.StartDate?.AddDays(subscriptionType.DurationInDays))?.AddDays(1);
                    membership.CreatedOn = DateTime.Now;

                    membership = await _unitOfWork.Repository<MemberShip>().AddAsync(membership);

                    membershipDto.MemberShipId = membership.MemberShipId;
                }
                var payment = await _unitOfWork.Repository<Payment>().AddAsync(new Payment()
                {
                    MemberShipId = membershipDto.MemberShipId,
                    UserId = membershipDto.UserId,
                    AmountPaid = membershipDto.AmountPaid ?? 0,
                    PaymentDate = membershipDto.PaymentDate ?? DateTime.MinValue,
                    PaymentMethod = membershipDto.PaymentMethod,
                    Notes = membershipDto.Notes ?? string.Empty,
                    CreatedOn = DateTime.Now
                });
                await _unitOfWork.Repository<MemberShip>().SaveChangesAsync();
                await _unitOfWork.Repository<Payment>().SaveChangesAsync();
                return new ResponseResult(true);
            }
            catch (Exception ex)
            {
                return new ResponseResult(false, new List<string> { ex.Message });
            }
        }        

        public async Task<ResponseResult> EditMemberShip(MembershipDto membershipDto)
        {
            try
            {
                var membership = await _unitOfWork.Repository<MemberShip>().Update(new MemberShip()
                {
                    MemberShipId = membershipDto.MemberShipId,
                    UserId = membershipDto.UserId,
                    SubscriptionTypeId = membershipDto.SubscriptionTypeId,
                    StartDate = membershipDto.StartDate,
                    EndDate = membershipDto.StartDate?.AddDays(30),
                    Status = true,
                    RenewalDate = (membershipDto.StartDate?.AddDays(30))?.AddDays(1),
                    UpdatedOn = DateTime.Now
                });
                var payment = await _unitOfWork.Repository<Payment>().Update(new Payment()
                {
                    PaymentId = membershipDto.PaymentId,
                    MemberShipId = membershipDto.MemberShipId,
                    UserId = membershipDto.UserId,
                    AmountPaid = membershipDto.AmountPaid ?? 0,
                    PaymentDate = membershipDto.PaymentDate ?? DateTime.MinValue,
                    PaymentMethod = membershipDto.PaymentMethod,
                    Notes = membershipDto.Notes ?? string.Empty,
                    UpdatedOn = DateTime.Now
                });

                await _unitOfWork.Repository<MemberShip>().SaveChangesAsync();
                await _unitOfWork.Repository<Payment>().SaveChangesAsync();

                return new ResponseResult(true);
                
            }
            catch (Exception ex)
            {
                return new ResponseResult(true, new List<string> { ex.Message });
            }
        }
    }
}
