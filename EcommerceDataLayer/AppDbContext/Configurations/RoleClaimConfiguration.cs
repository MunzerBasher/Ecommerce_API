﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace SurveyManagementSystemApi.Configurations
{

    //public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    //{
    //    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    //    {
    //        //Default Data
    //        var permissions = Permissions.GetAllPermissions();
    //        var adminClaims = new List<IdentityRoleClaim<string>>();

    //        for (var i = 0; i < permissions.Count; i++)
    //        {
    //            adminClaims.Add(new IdentityRoleClaim<string>
    //            {
    //                Id = i + 1,
    //                ClaimType = Permissions.Type,
    //                ClaimValue = permissions[i],
    //                RoleId = DefaultRoles.ClientRoleId
    //            });
    //        }

           
    //    }
    //}
}
