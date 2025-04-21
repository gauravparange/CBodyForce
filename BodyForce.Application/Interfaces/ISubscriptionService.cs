

namespace BodyForce
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionDto>> GetAllSubscripitonAsync();
        Task<SubscriptionDto> GetSubscripitonTypeAsyncVyId(int subscriptionTypeId);
        Task<ResponseResult> AddSubscription(SubscriptionDto subscriptionDto);
        Task<ResponseResult> EditSubscription(SubscriptionDto subscriptionDto);
        Task<ResponseResult> DeleteSubscription(int Id);
    }
}