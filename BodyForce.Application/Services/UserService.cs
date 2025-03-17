using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> SignUpUserAsync(SignUpDto signUpDto)
        {
            string maxUserId = "0001";
            var allusers = _userManager.Users.ToList();
            if (allusers.Any())
            {
                maxUserId =  (Convert.ToInt32(maxUserId) + allusers.Max(x => x.Id)).ToString();
            }
            var user = new User()
            {
                UserName = "BF" + maxUserId,
                FirstName = signUpDto.FirstName,
                LastName = signUpDto.LastName,
                Email = signUpDto.Email,
                DOB = signUpDto.DOB,
                PhoneNumber = signUpDto.PhoneNo,
                ParentPhoneNo = signUpDto.ParentPhoneNo,
                Address = signUpDto.Address,
                Weight = signUpDto?.Weight,
                Height = signUpDto?.Height,
                CreatedOn = DateTime.Now
            };
            string password = user.FirstName + "@" + user.DOB.Year.ToString();
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var role = await _roleManager.FindByIdAsync(1.ToString());
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
            }
            return result;
        }

        public async Task<SignInResult> LogInUserAsync(LogInDto logInDto)
        {
            var user = await _userManager.FindByNameAsync(logInDto.UserName);
            if (user == null) return SignInResult.Failed;

            var result = await _signInManager.PasswordSignInAsync(user, logInDto.Password, false, false);
            return result;
        }
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
