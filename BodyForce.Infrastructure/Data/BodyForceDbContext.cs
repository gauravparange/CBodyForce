﻿using Microsoft.AspNetCore.Identity;
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
    public class BodyForceDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
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
        public DbSet<MembershipView> MembershipView { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ✅ Renaming Identity tables
            builder.Entity<ApplicationUser>().ToTable("Users"); // Renaming AspNetUsers to Users
            builder.Entity<ApplicationRole>().ToTable("Roles"); // Renaming AspNetRoles to Roles
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles"); // Renaming AspNetUserRoles
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims"); // AspNetUserClaims
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins"); // AspNetUserLogins
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims"); // AspNetRoleClaims
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens"); // AspNetUserTokens

            builder.Entity<MembershipView>().ToView(nameof(MembershipView)).HasNoKey();
        }
    }
}
