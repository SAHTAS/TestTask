using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.UserId);
            builder.Property(x => x.Login).IsRequired();
            builder.HasIndex(x => x.Login).IsUnique();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.CreatedDate);
            builder.Property(x => x.BlockedDate).IsRequired(false);
            builder.Property(x => x.LastUpdate).IsRequired(false);
            builder
                .HasOne(x => x.UserGroup)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.UserGroupId)
                .IsRequired();
            builder
                .HasOne(x => x.UserState)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.UserStateId)
                .IsRequired();

            var now = DateTime.UtcNow;
            builder.HasData
            (
                new User
                {
                    UserId = 1,
                    Login = "Admin",
                    Password = "AQAAAAEAACcQAAAAEOD6xq4/veVRP16vgU4e/SFOU6wQZMvK2e3RLYrcYw30HD4OoqflBzP4eaOq0ufEmw==",
                    CreatedDate = now,
                    LastUpdate = now,
                    UserGroupId = 2,
                    UserStateId = 2
                }
            );
        }
    }
}
