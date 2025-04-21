
using Microsoft.AspNetCore.Identity;

namespace BodyForce
{
    public interface IMemberShipService
    {
        Task<IEnumerable<MembersDto>> GetAllMembers();
        Task<SignUpDto> GetMember(int UserId);
        Task<List<ViewMembershipDto>> ViewMemberShip(int UserId);
        Task<MembershipDto> GetMemberShip(int userId);
        Task<IdentityResult> AddMemberShip(MembershipDto membershipDto);
    }
}