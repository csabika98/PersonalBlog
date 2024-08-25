using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using PersonalBlogCsabaSallai.Models;


namespace PersonalBlogCsabaSallai.Stores
{
    public class RoleStore : IRoleStore<ApplicationRole>
    {
        private readonly IMongoCollection<ApplicationRole> _roles;

        public RoleStore(IMongoDatabase database)
        {
            _roles = database.GetCollection<ApplicationRole>("Roles");
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            await _roles.InsertOneAsync(role, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            var result = await _roles.DeleteOneAsync(r => r.Id == role.Id, cancellationToken);
            return result.DeletedCount > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<ApplicationRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await _roles.Find(r => r.Id == roleId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ApplicationRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await _roles.Find(r => r.NormalizedName == normalizedRoleName).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            var result = await _roles.ReplaceOneAsync(r => r.Id == role.Id, role, cancellationToken: cancellationToken);
            return result.ModifiedCount > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            // Ensure that the Id is never null
            if (role.Id == null)
            {
                throw new InvalidOperationException("Role Id cannot be null.");
            }
            return Task.FromResult(role.Id);
        }

        public Task<string?> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string? roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Dispose resources if needed
        }
    }
}
