

namespace BodyForce
{
    public interface IUserService
    {
        Task<ResponseResult<IEnumerable<UserDto>>> GetUserList();
        Task<ResponseResult> SignUpUserAsync(SignUpDto signUpDto);
        Task<ResponseResult> LogInUserAsync(LogInDto logInDto);
        Task<bool> IsEmailAvailableAsync(string Email);
        Task<ResponseResult> UpdateUserAsync(EditMemberDto memberDto);
        void LogOut();
    }
}