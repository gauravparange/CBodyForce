﻿using Microsoft.AspNetCore.Identity;

namespace BodyForce
{
    public interface IUserService
    {
        Task<IdentityResult> SignUpUserAsync(SignUpDto signUpDto);
        Task<SignInResult> LogInUserAsync(LogInDto logInDto);
        Task<User> IsEmailAvailableAsync(string Email);
        void LogOut();
    }
}