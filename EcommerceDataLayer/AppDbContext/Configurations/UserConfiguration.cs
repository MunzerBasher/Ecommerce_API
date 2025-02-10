using EcommerceDataLayer.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyManagementSystemApi.Abstractions.Consts;


namespace Api.AppDbContext.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserIdentity>
    {
        public void Configure(EntityTypeBuilder<UserIdentity> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.OwnsMany(x => x.RefreshTokens).ToTable("RefreshTokens").WithOwner().HasForeignKey("UseId");

           
        }
    }
}