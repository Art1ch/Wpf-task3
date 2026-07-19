using TestApp.Application.Filters;
using TestApp.Core.Entities;

namespace TestApp.Application.Abstractions;

public interface IUserRepository
{
    Task InsertBulkOfUsersAsync(IEnumerable<UserEntity> users, CancellationToken cancellationToken = default);
    Task CreateUserAsync(UserEntity user, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateUserAsync(UserEntity user, CancellationToken cancellationToken = default);
    Task<UserEntity> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserEntity>> GetUsersByFilterAsync(UserFilter filter, CancellationToken cancellationToken = default);
}
