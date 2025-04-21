
namespace BodyForce
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardCount();
        Task<IEnumerable<MembersDto>> GetList(string category);
    }
}