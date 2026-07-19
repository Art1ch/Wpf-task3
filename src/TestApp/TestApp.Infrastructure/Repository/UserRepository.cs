using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TestApp.Application.Abstractions;
using TestApp.Application.Filters;
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

    public async Task<UserEntity> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        return user!;
    }

    public async Task<IEnumerable<UserEntity>> GetUsersByFilterAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsNoTracking();

        if (!string.IsNullOrEmpty(filter.FirstName))
            query = query.Where(x => x.FirstName == filter.FirstName);

        if (!string.IsNullOrEmpty(filter.LastName))
            query = query.Where(x => x.LastName == filter.LastName);

        if (!string.IsNullOrEmpty(filter.MiddleName))
            query = query.Where(x => x.MiddleName == filter.MiddleName);

        if (filter.DataCollectedDateFrom != null)
            query = query.Where(x => x.DataCollectedDate >= filter.DataCollectedDateFrom);

        if (filter.DataCollectedDateTo != null)
            query = query.Where(x => x.DataCollectedDate <= filter.DataCollectedDateTo);

        if (!string.IsNullOrEmpty(filter.Country))
            query = query.Where(x => x.Country == filter.Country);

        if (!string.IsNullOrEmpty(filter.City))
            query = query.Where(x => x.City == filter.City);

        var users = await query.ToListAsync(cancellationToken);

        return users;
    }
}
