
using Hotel.Models.Context;
using Hotel.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IRepository<User>
{
    private readonly HotelContext _context;

    public UserRepository(HotelContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(string id) // Change from int to string
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
    }

    public Task UpdateAsync(User entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string id) // Change from int to string
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}