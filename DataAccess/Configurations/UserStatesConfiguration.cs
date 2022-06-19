using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class UserStatesConfiguration : IEntityTypeConfiguration<UserState>
    {
        public void Configure(EntityTypeBuilder<UserState> builder)
        {
            builder.ToTable("UserStates");

            builder.HasKey(x => x.UserStateId);
            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.Description).IsRequired(false);

            builder.HasData
            (
                new UserState
                {
                    UserStateId = 1,
                    Code = UserStateCode.Blocked,
                    Description = "Deleted user."
                },
                new UserState
                {
                    UserStateId = 2,
                    Code = UserStateCode.Active,
                    Description = "Active user."
                }
            );
        }
    }
}