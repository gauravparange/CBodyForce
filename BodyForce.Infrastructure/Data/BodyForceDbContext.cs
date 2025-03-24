using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class BodyForceDbContext : IdentityDbContext<User, Role, int,
                                                        IdentityUserClaim<int>,
                                                        IdentityUserRole<int>,
                                                        IdentityUserLogin<int>,
                                                        IdentityRoleClaim<int>,
                                                        IdentityUserToken<int>>
    {
        public BodyForceDbContext(DbContextOptions<BodyForceDbContext> options) : base(options)
        {
                
        }

        public DbSet<MemberShip> MemberShip { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<SubscriptionType> SubscriptionType { get; set; }
        //public DbSet<Attendance> Attendance { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ✅ Renaming Identity tables
            builder.Entity<User>().ToTable("Users"); // Renaming AspNetUsers to Users
            builder.Entity<Role>().ToTable("Roles"); // Renaming AspNetRoles to Roles
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles"); // Renaming AspNetUserRoles
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims"); // AspNetUserClaims
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins"); // AspNetUserLogins
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims"); // AspNetRoleClaims
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens"); // AspNetUserTokens
        }
    }
}
