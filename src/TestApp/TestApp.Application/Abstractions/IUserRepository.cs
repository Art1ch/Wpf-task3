using TestApp.Core.Entities;

namespace TestApp.Application.Abstractions;

public interface IUserRepository
{
    Task InsertBulkOfUsersAsync(IEnumerable<UserEntity> users, CancellationToken cancellationToken = default);
    Task CreateUserAsync(UserEntity user, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateUserAsync(UserEntity user, CancellationToken cancellationToken = default);
    Task GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
