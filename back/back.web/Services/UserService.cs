using back.Classes;
using back.Classes.SessionState;
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

    Task<List<SessionSecondDTO>> getUserSessions(int userId);

    Task<UserDTO> authLogin(UserInput userInput);


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

        var existingUser = await _databaseContext.Users.SingleOrDefaultAsync(u => u.name.Equals(userInput.name));
        if (existingUser != null)
        {
            throw new BadHttpRequestException($"User with name '{userInput.name}' already exists.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userInput.password); 
        
        var userToAdd = new UserEntity { name = userInput.name, password = passwordHash };
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

    public async Task<UserDTO> authLogin(UserInput userInput)
    {
        UserEntity? user = await _databaseContext.Users.SingleOrDefaultAsync(u => u.name.Equals(userInput.name));

        if (user == null)
        {
            throw new BadHttpRequestException("");
        }

        if (!BCrypt.Net.BCrypt.Verify(userInput.password, user.password))
        {
            throw new UnauthorizedAccessException("");
        }

        return await GetByID(user.id);
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

    public async Task<List<SessionSecondDTO>> getUserSessions(int userId)
    {
        if (await GetByID(userId) == null)
        {
            return null;
        }

        var sessions = SessionList.Sessions.Where(s => s._joinedUsers.Select(u => u.id).Contains(userId)).Where(s => s._state is EndState);

        List<SessionSecondDTO> sdto = new List<SessionSecondDTO>();

        foreach (var se in sessions)
        {
            sdto.Add(new SessionSecondDTO
            {
                identifier = se.Identifier,
                userStories = se._allVotedUserStories.ToList(),
                users = se._joinedUsers.Select(u => new UserDTO { id = u.id, name = u.name }).ToList()
            } );
        }

        return sdto;
    }
}