using back.Classes;
using back.DAL;
using back.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace back.Services;

public interface IUserStoryPropositionService
{
    public IEnumerable<UserStoryPropositionEntity> getAll();
    public Task<UserStoryPropositionEntity> getByID(int id);
    public Task<UserStoryPropositionEntity> create(UserStoryPropositionInput userStoryP);
    public Task<bool> delete(int id);
    public Task<UserStoryPropositionEntity> update(int id, UserStoryPropositionInput input);
}

public class UserStoryPropositionService : IUserStoryPropositionService
{
    private readonly DatabaseContext _userStoryPropositionContext;

    public UserStoryPropositionService(DatabaseContext context)
    {
        _userStoryPropositionContext = context;
    }

    public IEnumerable<UserStoryPropositionEntity> getAll()
    {
        return _userStoryPropositionContext.UserStoriesProposition.OrderBy(u => u.id);
    }
    
    public async Task<UserStoryPropositionEntity> getByID(int id)
    {
        var userStoryP = await _userStoryPropositionContext.UserStoriesProposition.FindAsync(id);
        return userStoryP;
    }
    
    public async Task<UserStoryPropositionEntity> create(UserStoryPropositionInput userStoryP)
    {
        UserStoryPropositionEntity userStoryPropositionToAdd = new UserStoryPropositionEntity(userStoryP.description);
        _userStoryPropositionContext.UserStoriesProposition.Add(userStoryPropositionToAdd);
        await _userStoryPropositionContext.SaveChangesAsync();

        return userStoryPropositionToAdd;
    }
    
    public async Task<bool> delete(int id)
    {
        var userStoryPropositionToDelete = await getByID(id);

        if (userStoryPropositionToDelete == null)
        {
            return false;
        }

        _userStoryPropositionContext.UserStoriesProposition.Remove(userStoryPropositionToDelete);
        await _userStoryPropositionContext.SaveChangesAsync();
        return true;
    }
    
    public async Task<UserStoryPropositionEntity> update(int id, UserStoryPropositionInput input)
    {
        var userStoryP = await getByID(id);

        if (userStoryP == null)
        {
            return null;
        }
        
        if (input.description != null)
        {
            userStoryP.description = input.description;
        }

        if (input.tasks != null)
        {
            userStoryP.tasks = input.tasks;
        }
        
        await _userStoryPropositionContext.SaveChangesAsync();
        
        Session? session = SessionList.Sessions.Find(s => s._allUserStories.Select(usp => usp.id).Contains(id));

        if (session != null)
        {
            var ups = session._allUserStories.Where(usp => usp.id == id);

            ups.First().description = input.description;
            ups.First().tasks = input.tasks;
            
            await session.sendSessionToAllWS();
        }

        return userStoryP;
    }
}