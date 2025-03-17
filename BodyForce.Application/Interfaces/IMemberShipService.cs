
namespace BodyForce
{
    public interface IMemberShipService
    {
        Task<IEnumerable<MembersDto>> GetAllMembers();
        Task<SignUpDto> GetMember(int UserId);
    }
}