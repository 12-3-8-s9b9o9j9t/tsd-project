using back.DAL;
using back.Entities;
using Microsoft.EntityFrameworkCore;

namespace back.Services;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<UserDTO> CreateUserAsync(UserInput userInput);
    Task<UserDTO> GetUserByNameAsync(string name);

    Task<UserDTO> GetByID(int id);

    Task<List<SessionEntity>> getUserSessions(int userId);

}

public class UserService : IUserService
{
    private readonly DatabaseContext _databaseContext;

    public UserService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        var users = await _databaseContext.Users.OrderBy(u => u.id).ToListAsync();
        return users.Select(u => new UserDTO { id = u.id, name = u.name });
    }

    public async Task<UserDTO> CreateUserAsync(UserInput userInput)
    {
        if (string.IsNullOrEmpty(userInput.name) || userInput.name.Contains(' '))
        {
            throw new BadHttpRequestException("Name must be valid.");
        }

        var existingUser = await _databaseContext.Users.SingleOrDefaultAsync(u => u.name == userInput.name);
        if (existingUser != null)
        {
            throw new BadHttpRequestException($"User with name '{userInput.name}' already exists.");
        }

        var userToAdd = new UserEntity { name = userInput.name };
        _databaseContext.Users.Add(userToAdd);
        await _databaseContext.SaveChangesAsync();

        return new UserDTO { id = userToAdd.id, name = userToAdd.name };
    }

    public async Task<UserDTO> GetUserByNameAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name must be valid.");
        }

        var user = await _databaseContext.Users.SingleOrDefaultAsync(u => u.name.Equals(name));
        if (user == null)
        {
            return null;
        }

        return new UserDTO { id = user.id, name = user.name };
    }

    public async Task<UserDTO> GetByID(int id)
    {
        var user = await _databaseContext.Users.FindAsync(id);

        if (user == null)
        {
            return null;
        }

        return new UserDTO { id = user.id, name = user.name };
    }

    public async Task<List<SessionEntity>> getUserSessions(int userId)
    {
        var sessions = await _databaseContext.Sessions
            .Include(s => s.users)
            .Where(s => s.users.Select(u => u.id).Contains(userId))
            .ToListAsync();

        return sessions;
    }
}