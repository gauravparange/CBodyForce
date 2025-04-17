using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SubscriptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SubscriptionDto>> GetAllSubscripitonAsync()
        {
            var result = await _unitOfWork.Repository<SubscriptionType>().GetByConditionAsync(x => x.IsDeleted == false);
            return _mapper.Map<IEnumerable<SubscriptionDto>>(result);
        }
        public async Task<SubscriptionDto> GetSubscripitonTypeAsyncVyId(int subscriptionTypeId)
        {
            var result = await _unitOfWork.Repository<SubscriptionType>().GetByIdAsync(subscriptionTypeId);
            return _mapper.Map<SubscriptionDto>(result);
        }
        public async Task<ResponseResult> AddSubscription(SubscriptionDto subscriptionDto)
        {
            try
            {
                var result = await _unitOfWork.Repository<SubscriptionType>().AddAsync(_mapper.Map<SubscriptionType>(subscriptionDto));
                await _unitOfWork.Repository<SubscriptionType>().SaveChangesAsync();
                return new ResponseResult(true);
            }

            catch (Exception ex)
            {
                return new ResponseResult(false, new List<string> { ex.Message.ToString() });
            }
        }
        public async Task<ResponseResult> EditSubscription(SubscriptionDto subscriptionDto)
        {
            try
            {
                var result = await  _unitOfWork.Repository<SubscriptionType>().Update(_mapper.Map<SubscriptionType>(subscriptionDto));
                await _unitOfWork.Repository<SubscriptionType>().SaveChangesAsync();
                return new ResponseResult(true);
            }
            catch (Exception ex)
            {
                return new ResponseResult(false, new List<string> { ex.Message.ToString() });
            }
        }
        public async Task<ResponseResult> DeleteSubscription(int Id)
        {
            try
            {
                var result = await  _unitOfWork.Repository<SubscriptionType>().GetByIdAsync(Id);
                result.IsDeleted = true;
                await _unitOfWork.Repository<SubscriptionType>().SaveChangesAsync();
                return new ResponseResult(true);
            }

            catch (Exception ex)
            {
                return new ResponseResult(false, new List<string> { ex.Message.ToString() });
            }
        }
    }
}
