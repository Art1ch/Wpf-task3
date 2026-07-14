using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TestApp.Application.Abstractions;
using TestApp.Core.Entities;
using TestApp.Infrastructure.Context;

namespace TestApp.Infrastructure.Repository;

internal sealed class UserRepository : IUserRepository
{
    private readonly UserContext _context;

    public UserRepository(UserContext context)
    {
        _context = context;
    }

    public async Task InsertBulkOfUsersAsync(IEnumerable<UserEntity> users, CancellationToken cancellationToken = default)
    {
        await _context.BulkInsertAsync(users, cancellationToken : cancellationToken);
    }

    public async Task CreateUserAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UpdateUserAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
}
