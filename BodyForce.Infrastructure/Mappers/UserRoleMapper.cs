using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public static class UserRoleMapper
    {
        public static User ToDomain(ApplicationUser identityUser)
        {
            if (identityUser == null) return null;

            return new User
            {
                Id = identityUser.Id,
                FirstName = identityUser.FirstName,
                LastName = identityUser.LastName,
                Email = identityUser.Email,
                ParentPhoneNo = identityUser.ParentPhoneNo,
                DOJ = identityUser.DOJ,
                DOB = identityUser.DOB,
                Address = identityUser.Address,
                Height = identityUser.Height,
                Weight = identityUser.Weight,
                UserName = identityUser.UserName,
                PhoneNumber = identityUser.PhoneNumber,
                NormalizedUserName = identityUser.NormalizedUserName,
                NormalizedEmail = identityUser.NormalizedEmail,
                EmailConfirmed = identityUser.EmailConfirmed,
                PasswordHash = identityUser.PasswordHash,
                SecurityStamp = identityUser.SecurityStamp,
                ConcurrencyStamp = identityUser.ConcurrencyStamp,
                PhoneNumberConfirmed = identityUser.PhoneNumberConfirmed,
                TwoFactorEnabled = identityUser.TwoFactorEnabled,
                LockoutEnd = identityUser.LockoutEnd,
                LockoutEnabled = identityUser.LockoutEnabled,
                AccessFailedCount = identityUser.AccessFailedCount
            };
        }
        public static ApplicationUser ToIdentityUser(User domainUser)
        {
            if (domainUser == null) return null;

            return new ApplicationUser
            {
                Id = domainUser.Id, // IdentityUser.Id is string
                FirstName = domainUser.FirstName,
                LastName = domainUser.LastName,
                Email = domainUser.Email,
                ParentPhoneNo = domainUser.ParentPhoneNo,
                DOJ = domainUser.DOJ,
                DOB = domainUser.DOB,
                Address = domainUser.Address,
                Height = domainUser.Height,
                Weight = domainUser.Weight,
                UserName = domainUser.UserName,
                PhoneNumber = domainUser.PhoneNumber,
                NormalizedUserName = domainUser.NormalizedUserName,
                NormalizedEmail = domainUser.NormalizedEmail,
                EmailConfirmed = domainUser.EmailConfirmed,
                PasswordHash = domainUser.PasswordHash,
                SecurityStamp = domainUser.SecurityStamp,
                ConcurrencyStamp = domainUser.ConcurrencyStamp,
                PhoneNumberConfirmed = domainUser.PhoneNumberConfirmed,
                TwoFactorEnabled = domainUser.TwoFactorEnabled,
                LockoutEnd = domainUser.LockoutEnd,
                LockoutEnabled = domainUser.LockoutEnabled,
                AccessFailedCount = domainUser.AccessFailedCount
            };
        }
        public static Role ToDomain(ApplicationRole identityRole)
        {
            if (identityRole == null) return null;

            return new Role
            {
                Id = identityRole.Id,
                Name = identityRole.Name,
                NormalizedName = identityRole.NormalizedName,
                ConcurrencyStamp = identityRole.ConcurrencyStamp
            };
        }
        public static ApplicationRole ToIdentityRole(Role domainRole)
        {
            if(domainRole == null) return null;

            return new ApplicationRole
            {
                Id = domainRole.Id,
                Name = domainRole.Name,
                NormalizedName = domainRole.NormalizedName,
                ConcurrencyStamp = domainRole.ConcurrencyStamp
            };
        }
        public static IEnumerable<User> ToDomainList(IEnumerable<ApplicationUser> identityUsers)
        {
            return identityUsers?.Select(ToDomain).ToList() ?? new List<User>();
        }

        public static List<ApplicationUser> ToIdentityUserList(IEnumerable<User> domainUsers)
        {
            return domainUsers?.Select(ToIdentityUser).ToList() ?? new List<ApplicationUser>();
        }
        public static void MapToExistingIdentityUser(User user, ApplicationUser identityUser)
        {
            identityUser.FirstName = user.FirstName;
            identityUser.LastName = user.LastName;
            identityUser.Email = user.Email;
            identityUser.ParentPhoneNo = user.ParentPhoneNo;
            identityUser.DOJ = user.DOJ;
            identityUser.DOB = user.DOB;
            identityUser.Address = user.Address;
            identityUser.Height = user.Height;
            identityUser.Weight = user.Weight;
            identityUser.UserName = user.UserName;
            identityUser.PhoneNumber = user.PhoneNumber;

            // Identity inherited props
            identityUser.NormalizedUserName = user.NormalizedUserName;
            identityUser.NormalizedEmail = user.NormalizedEmail;
            identityUser.EmailConfirmed = user.EmailConfirmed;
            identityUser.PasswordHash = user.PasswordHash;
            identityUser.SecurityStamp = user.SecurityStamp;
            identityUser.ConcurrencyStamp = user.ConcurrencyStamp;
            identityUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            identityUser.TwoFactorEnabled = user.TwoFactorEnabled;
            identityUser.LockoutEnd = user.LockoutEnd;
            identityUser.LockoutEnabled = user.LockoutEnabled;
            identityUser.AccessFailedCount = user.AccessFailedCount;
        }

    }
}
