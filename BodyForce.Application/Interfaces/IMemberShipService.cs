


namespace BodyForce
{
    public interface IMemberShipService
    {
        Task<IEnumerable<MembersDto>> GetAllMembers();
        Task<EditMemberDto> GetMember(int UserId);
        Task<List<ViewMembershipDto>> ViewMemberShip(int UserId);
        Task<MembershipDto> GetMemberShip(int membershipId);
        Task<ResponseResult> AddMemberShip(MembershipDto membershipDto);
        Task<ResponseResult> EditMemberShip(MembershipDto membershipDto);
    }
}