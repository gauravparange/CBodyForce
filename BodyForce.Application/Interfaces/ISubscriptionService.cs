using Microsoft.AspNetCore.Identity;

namespace BodyForce
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionDto>> GetAllSubscripitonAsync();
        Task<SubscriptionDto> GetSubscripitonTypeAsyncVyId(int subscriptionTypeId);
        Task<IdentityResult> AddSubscription(SubscriptionDto subscriptionDto);
        Task<IdentityResult> EditSubscription(SubscriptionDto subscriptionDto);
    }
}