using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class UserGroupsConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.ToTable("UserGroups");

            builder.HasKey(x => x.UserGroupId);
            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.Description).IsRequired(false);

            builder.HasData
            (
                new UserGroup
                {
                    UserGroupId = 1,
                    Code = UserGroupCode.User,
                    Description = "Regular user."
                },
                new UserGroup
                {
                    UserGroupId = 2,
                    Code = UserGroupCode.Admin,
                    Description = "Administrator. User with additional rights."
                }
            );
        }
    }
}