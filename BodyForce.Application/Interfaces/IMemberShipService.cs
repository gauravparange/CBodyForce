


namespace BodyForce
{
    public interface IMemberShipService
    {
        Task<IEnumerable<MembersDto>> GetAllMembers();
        Task<EditMemberDto> GetMember(int UserId);
        Task<List<ViewMembershipDto>> ViewMemberShip(int UserId);
        Task<MembershipDto> GetMemberShip(int userId, bool forAdd);
        Task<ResponseResult> AddMemberShip(MembershipDto membershipDto);
        Task<ResponseResult> EditMemberShip(MembershipDto membershipDto);
    }
}