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
            var user = UserRoleMapper.ToDomainList(await _unitOfWork.Repository<ApplicationUser>().GetByConditionAsync(x => x.IsDeleted == false));    
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
        public async Task<MembershipDto> GetMemberShip(int membershipId)
        {
            var membership = (await _unitOfWork.Repository<MemberShip>().GetByConditionAsync(x => x.MemberShipId == membershipId)).FirstOrDefault();
            var payment = (await _unitOfWork.Repository<Payment>().GetByConditionAsync(x =>  x.MemberShipId == membership.MemberShipId)).FirstOrDefault();
            if(membership != null || payment != null)
            {
                var result = _mapper.Map<MembershipDto>(membership);

                result.PaymentDate = payment.PaymentDate;
                result.PaymentMethod = payment.PaymentMethod;
                result.AmountPaid = payment.AmountPaid;
                result.Notes = payment.Notes;
                result.PaymentId = payment.PaymentId;

                return result;
            }
            return null;
            
        }
        public async Task<ResponseResult> AddMemberShip(MembershipDto membershipDto)
        {
            try
            {

                var subscriptionType = await _unitOfWork.Repository<SubscriptionType>().GetByIdAsync(membershipDto.SubscriptionTypeId);
                

                var membership =  _mapper.Map<MemberShip>(membershipDto);
                membership.EndDate = membershipDto.StartDate?.AddDays(subscriptionType.DurationInDays);
                membership.Status = true;
                membership.RenewalDate = (membershipDto.StartDate?.AddDays(subscriptionType.DurationInDays))?.AddDays(1);
                membership.CreatedOn = DateTime.Now;

                membership = await _unitOfWork.Repository<MemberShip>().AddAsync(membership);
                await _unitOfWork.Repository<MemberShip>().SaveChangesAsync();

                var payment = _mapper.Map<Payment>(membershipDto);

                payment.MemberShipId = membership.MemberShipId;
                payment.CreatedOn = DateTime.Now;

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
                var subscriptionType = await _unitOfWork.Repository<SubscriptionType>().GetByIdAsync(membershipDto.SubscriptionTypeId);

                var membership = _mapper.Map<MemberShip>(membershipDto);

                membership.EndDate = membershipDto.StartDate?.AddDays(subscriptionType.DurationInDays);
                membership.Status = true;
                membership.RenewalDate = (membershipDto.StartDate?.AddDays(subscriptionType.DurationInDays))?.AddDays(1);
                membership.UpdatedOn = DateTime.Now;

                await _unitOfWork.Repository<MemberShip>().Update(membership);

                var payment = _mapper.Map<Payment>(membershipDto);

        
                payment.UpdatedOn = DateTime.Now;

                await _unitOfWork.Repository<Payment>().Update(payment);

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
