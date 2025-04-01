using AutoMapper;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BodyForce
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IdentityResult> SignUpUserAsync(SignUpDto signUpDto)
        {

            var user = _mapper.Map<User>(signUpDto);
            user.UserName = GetMemberCode();
            //{
            //    UserName = GetMemberCode(),
            //    FirstName = signUpDto.FirstName,
            //    LastName = signUpDto.LastName,
            //    Email = signUpDto.Email,
            //    DOB = signUpDto.DOB,
            //    PhoneNumber = signUpDto.PhoneNo,
            //    ParentPhoneNo = signUpDto.ParentPhoneNo,
            //    Address = signUpDto.Address,
            //    Weight = signUpDto?.Weight,
            //    Height = signUpDto?.Height,
            //    CreatedOn = DateTime.Now
            //};
            string password = user.FirstName + "@" + user.DOB.Year.ToString();
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var role = await _roleManager.FindByIdAsync(signUpDto.RoleId.ToString());
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                var userID = await _userManager.FindByEmailAsync(user.Email);
                var member  = new MemberShip()
                {
                    MemberShipId = 0,
                    UserId = userID.Id,
                    Status = false
                };
                var _result = await _unitOfWork.Repository<MemberShip>().AddAsync(member);
                await _unitOfWork.Repository<MemberShip>().SaveChangesAsync();
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
        public async Task<User> IsEmailAvailableAsync(string Email)
        {
            return await _userManager.FindByEmailAsync(Email);
        }
        public async void LogOut()
        {
             await _signInManager.SignOutAsync();
        }


        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
        private string GetMemberCode()
        {
            return "BF" + (_userManager.Users.ToList().Any() ? _userManager.Users.ToList().Max(x => x.Id) + 1 : 1).ToString().PadLeft(4, '0');
        }
    }
}
