using EcommerceDataLayer.AppDbContex;
using EcommerceDataLayer.Entities.Roles;
using EcommerceLogicalLayer.Helpers;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyManagementSystemApi.Abstractions.Consts;

namespace Api.Auth
{
    public class RoleService(ApplicationDbContext context, RoleManager<UserRoles> roleManager) : IRoleService
    {
        private readonly RoleManager<UserRoles> _roleManager = roleManager;
        private readonly ApplicationDbContext _context  = context;

        public async Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request)
        {
            var roleIsExists = await _roleManager.RoleExistsAsync(request.Name);

            if (roleIsExists)
                return Result<RoleDetailResponse>.Failure<RoleDetailResponse>(new Error("Duplicated Name ", StatusCodes.Status404NotFound));

            var allowedPermissions = Permissions.GetAllPermissions();
            
            if (request.Permissions.Except(allowedPermissions).Any())
                return Result<RoleDetailResponse>.Failure<RoleDetailResponse>(new Error("Invalid Permissions", StatusCodes.Status404NotFound));

            var role = new UserRoles
            {
                Name = request.Name,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                var permissions = request.Permissions
                    .Select(x => new IdentityRoleClaim<string>
                    {
                        ClaimType = Permissions.Type,
                        ClaimValue = x,
                        RoleId = role.Id
                    });

                await _context.AddRangeAsync(permissions);
                await _context.SaveChangesAsync();

                var response = new RoleDetailResponse
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsDeleted = role.IsDeleted,
                    Permissions = request.Permissions
                };

                return Result<RoleDetailResponse>.Seccuss(response);
            }
            var error = result.Errors.First();
            return Result<RoleDetailResponse>.Failure<RoleDetailResponse>(new Error("Bad Request", StatusCodes.Status400BadRequest));
        }

        public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default)
        {
            return await _roleManager.Roles
              .Where(x => !x.IsDefault && (!x.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))
              .ProjectToType<RoleResponse>() 
              .ToListAsync(cancellationToken);
        }

        public async Task<Result<RoleDetailResponse>> GetAsync(string id)
        {
            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result<RoleDetailResponse>.Failure<RoleDetailResponse>(new Error("Not Found", StatusCodes.Status404NotFound));

            var permissions = await _roleManager.GetClaimsAsync(role);

            var response = new RoleDetailResponse
            {
                Id = role.Id,
                Name = role.Name!,
                IsDeleted = role.IsDeleted,
                Permissions = permissions.Select(x => x.Value).ToArray()
            };
            return Result<RoleDetailResponse>.Seccuss(response);
        }

        public async Task<Result> ToggleStatusAsync(string id)
        {
            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result.Failure(new Error("Not Found", StatusCodes.Status404NotFound));
            role.IsDeleted = !role.IsDeleted;
            await _roleManager.UpdateAsync(role);
            return Result.Seccuss();
        }

        public async Task<Result> UpdateAsync(string id, RoleRequest request)
        {
            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result.Failure(new Error("Not Found", StatusCodes.Status404NotFound));
            var Enable = await _roleManager.Roles.AnyAsync(x => x.Name == request.Name && x.Id != id);
            if (Enable)
                return Result.Failure(new Error("Duplicated Roles", StatusCodes.Status400BadRequest));
            var allowedPermissions = Permissions.GetAllPermissions();

            if (request.Permissions.Except(allowedPermissions).Any())
                return Result.Failure(new Error("InValid Permissions", StatusCodes.Status404NotFound));
            role.Name = request.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                var currentPermissions = await _context.RoleClaims
                    .Where(x => x.RoleId == id && x.ClaimType == Permissions.Type)
                    .Select(x => x.ClaimValue)
                    .ToListAsync();

                var newPermissions = request.Permissions.Except(currentPermissions)
                    .Select(x => new IdentityRoleClaim<string>
                    {
                        ClaimType = Permissions.Type,
                        ClaimValue = x,
                        RoleId = role.Id
                    });

                var removedPermissions = currentPermissions.Except(request.Permissions);

                await _context.RoleClaims
                    .Where(x => x.RoleId == id && removedPermissions.Contains(x.ClaimValue))
                .ExecuteDeleteAsync();

                await _context.AddRangeAsync(newPermissions);
                await _context.SaveChangesAsync();
            }
            return Result.Seccuss();

        }

    }
}
