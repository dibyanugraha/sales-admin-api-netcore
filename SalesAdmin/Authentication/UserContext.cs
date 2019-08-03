
namespace SalesAdmin.Authentication
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using SalesAdmin.Authentication.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserContext : IdentityDbContext<User, Role, int>
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserLogin<int>>(x =>
            {
                x.Property(a => a.LoginProvider).HasMaxLength(128);
                x.Property(a => a.ProviderKey).HasMaxLength(128);
            });

            builder.Entity<IdentityUserToken<int>>(x =>
            {
                x.Property(a => a.LoginProvider).HasMaxLength(128);
                x.Property(a => a.Name).HasMaxLength(128);
            });

            foreach (var item in builder.Model.GetEntityTypes())
            {
                foreach (var prop in item.GetProperties())
                {
                    if (prop.ClrType == typeof(bool))
                    {
                        prop.SetValueConverter(new BoolToInt());
                    }
                }
            }
        }
    }
}
