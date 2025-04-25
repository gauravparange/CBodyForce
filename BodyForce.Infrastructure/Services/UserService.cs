using AutoMapper;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BodyForce
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseResult> SignUpUserAsync(SignUpDto signUpDto)
        {
            try
            {                
                var user =  UserRoleMapper.ToIdentityUser(_mapper.Map<User>(signUpDto));
                user.UserName = GetMemberCode();
                //var user = new ApplicationUser()
                //{
                //    FirstName = userEntity.FirstName,
                //    LastName = userEntity.LastName,
                //    Email = userEntity.Email,
                //    PhoneNumber = userEntity.PhoneNo,
                //    ParentPhoneNo = userEntity.ParentPhoneNo,
                //    DOB = userEntity.DOB,
                //    DOJ = userEntity.DOJ,
                //    Address = userEntity.Address,
                //    Height = userEntity.Height,
                //    Weight = userEntity.Weight,
                //    UserName = GetMemberCode(),
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
                    return new ResponseResult(true, new List<string> { "User created successfully" });
                }
                return new ResponseResult(false, result.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception ex)
            {
                return new ResponseResult(false, new List<string> {ex.Message.ToString()});
            }            
        }

        public async Task<ResponseResult> UpdateUserAsync(EditMemberDto memberDto)
        {
            try
            {
                // Step 1: Get tracked ApplicationUser (EF is tracking this)
                var existingIdentityUser = await _userManager.FindByIdAsync(memberDto.Id.ToString());
                if (existingIdentityUser == null)
                {
                    return new ResponseResult(false, new List<string> { $"User with ID {memberDto.Id} not found." });
                }

                // Step 2: Map to Domain User
                var domainUser = UserRoleMapper.ToDomain(existingIdentityUser);

                // Step 3: Map EditMemberDto to Domain User (your existing mapping)
                _mapper.Map(memberDto, domainUser);

                // Step 4: Map updated Domain User *onto existing IdentityUser*
                UserRoleMapper.MapToExistingIdentityUser(domainUser, existingIdentityUser);

                // Step 5: Update timestamp
                existingIdentityUser.UpdatedOn = DateTime.Now;

                // Step 6: Save update
                await _userManager.UpdateAsync(existingIdentityUser);

                return new ResponseResult(true, new List<string> { "User updated." });
            }
            catch (Exception ex)
            {
                return new ResponseResult(false, new List<string> { ex.Message.ToString() });
            }


        }
        public async Task<ResponseResult> LogInUserAsync(LogInDto logInDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(logInDto.UserName);
                if (user == null)
                {
                    return new ResponseResult(false, new List<string> { "User not found." });
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(user, logInDto.Password, false, false);

                    if (result.Succeeded)
                    {
                        return new ResponseResult(true, new List<string> { "" });
                    }
                    else if (result.IsNotAllowed)
                    {
                        return new ResponseResult(false, new List<string> { "LogIn Not Allowed" });
                    }
                    else
                    {
                        return new ResponseResult(false, new List<string> { "Invalid User Name or Password" });
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult(false, new List<string> { ex.Message.ToString() });
            }
        }
        public async Task<ResponseResult<IEnumerable<UserDto>>> GetUserList()
        {
            var response = new ResponseResult<IEnumerable<UserDto>>()
            {
                Success = false,
                Data = null,
                ErrorMessages = new List<string>()
            };
            try
            {
                var userList =  _mapper.Map<IEnumerable<UserDto>>(UserRoleMapper.ToDomainList(await _userManager.Users.ToListAsync()));
                if (userList.Any())
                {
                    response.Data = userList;
                    response.Success = true;                 
                }
                else
                {
                    response.ErrorMessages.Add("No Users Found");                  
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message.ToString());
            }

            return response;
        }
        public async Task<bool> IsEmailAvailableAsync(string Email)
        {
            if(await _userManager.FindByEmailAsync(Email) == null)
            {
                return true;
            }
            return false;
        }
        public async void LogOut()
        {
             await _signInManager.SignOutAsync();
        }

        private string GetMemberCode()
        {

            var users = _userManager.Users.ToList();

            // Extract valid usernames that start with "BF" and are followed by a number
            var validUserCodes = users
                .Select(u => u.UserName)
                .Where(name => name != null && name.StartsWith("BF") && name.Length > 2)
                .Select(name =>
                {
                    var numberPart = name.Substring(2);
                    return int.TryParse(numberPart, out int num) ? num : (int?)null;
                })
                .Where(num => num.HasValue)
                .Select(num => num.Value)
                .ToList();

            // Get next number
            int nextNumber = validUserCodes.Any() ? validUserCodes.Max() + 1 : 1;

            // Format it as "BF" + 4-digit number
            return "BF" + nextNumber.ToString().PadLeft(4, '0');
        }
    }
}
