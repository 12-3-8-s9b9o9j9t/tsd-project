using back.DAL;
using back.Entities;
using Microsoft.EntityFrameworkCore;

namespace back.Services;


public interface IUserStoryService
{
    Task<IEnumerable<UserStoryEntity>> GetAllUserStoriesAsync();
    Task<UserStoryEntity> GetUserStoryByIdAsync(int id);
    Task<UserStoryEntity> CreateUserStoryAsync(UserStoryInput userStory);
    Task<UserStoryEntity> DeleteUserStoryAsync(int id);
    Task<UserStoryEntity> UpdateUserStoryAsync(int id, UserStoryInput userStory);
}


public class UserStoryService : IUserStoryService
{
    private readonly DatabaseContext _userStoryContext;

    public UserStoryService(DatabaseContext userStoryContext)
    {
        _userStoryContext = userStoryContext;
    }

    public async Task<IEnumerable<UserStoryEntity>> GetAllUserStoriesAsync()
    {
        return await _userStoryContext.UserStories.OrderBy(u => u.id).ToListAsync();
    }

    public async Task<UserStoryEntity> GetUserStoryByIdAsync(int id)
    {
        return await _userStoryContext.UserStories.FindAsync(id);
    }

    public async Task<UserStoryEntity> CreateUserStoryAsync(UserStoryInput userStory)
    {
        UserStoryEntity userStoryToAdd = new UserStoryEntity(userStory.description, userStory.estimatedCost);
        _userStoryContext.UserStories.Add(userStoryToAdd);
        await _userStoryContext.SaveChangesAsync();

        return userStoryToAdd;
    }

    public async Task<UserStoryEntity> DeleteUserStoryAsync(int id)
    {
        var userStory = await _userStoryContext.UserStories.FindAsync(id);

        if (userStory == null)
        {
            return null;
        }

        _userStoryContext.UserStories.Remove(userStory);
        await _userStoryContext.SaveChangesAsync();

        return userStory;
    }

    public async Task<UserStoryEntity> UpdateUserStoryAsync(int id, UserStoryInput userStory)
    {
        var userStoryToUpdate = await _userStoryContext.UserStories.FindAsync(id);

        if (userStoryToUpdate == null)
        {
            return null;
        }

        if (userStory.description != null)
        {
            userStoryToUpdate.description = userStory.description;
        }

        if (userStory.estimatedCost != null)
        {
            userStoryToUpdate.estimatedCost = userStory.estimatedCost;
        }

        await _userStoryContext.SaveChangesAsync();

        return userStoryToUpdate;
    }
}