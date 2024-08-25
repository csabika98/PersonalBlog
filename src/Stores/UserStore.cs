using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using PersonalBlogCsabaSallai.Models;

namespace PersonalBlogCsabaSallai.Stores
{
    public class UserStore : IUserStore<ApplicationUser>,
                             IUserPasswordStore<ApplicationUser>,
                             IUserRoleStore<ApplicationUser>,
                             IUserEmailStore<ApplicationUser>
    {
        private readonly IMongoCollection<ApplicationUser> _users;

        public UserStore(IMongoDatabase database)
        {
            _users = database.GetCollection<ApplicationUser>("Users");
        }

        // IUserStore<ApplicationUser> implementation

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            await _users.InsertOneAsync(user, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var result = await _users.DeleteOneAsync(u => u.Id == user.Id, cancellationToken);
            return result.DeletedCount > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            // Ensure the possibility of a null return value is handled
            return await _users.Find(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            // Ensure the possibility of a null return value is handled
            return await _users.Find(u => u.NormalizedUserName == normalizedUserName).FirstOrDefaultAsync(cancellationToken);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            // Ensure that the Id is never null
            if (user.Id == null)
            {
                throw new InvalidOperationException("User Id cannot be null.");
            }
            return Task.FromResult(user.Id);
        }

        public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var result = await _users.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: cancellationToken);
            return result.ModifiedCount > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public void Dispose()
        {
            // Dispose resources if needed
        }

        // IUserPasswordStore<ApplicationUser> implementation

        public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        // IUserRoleStore<ApplicationUser> implementation

        public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Add(roleName);
            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            user.Roles.Remove(roleName);
            return Task.CompletedTask;
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult((IList<string>)user.Roles);
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Roles.Contains(roleName));
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var users = await _users.Find(u => u.Roles.Contains(roleName)).ToListAsync(cancellationToken);
            return users;
        }

        // IUserEmailStore<ApplicationUser> implementation

        public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return _users.Find(u => u.NormalizedEmail == normalizedEmail).FirstOrDefaultAsync(cancellationToken) as Task<ApplicationUser?>;
        }

        public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }
    }
}


