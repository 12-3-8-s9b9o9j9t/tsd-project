using back.Entities;
using Microsoft.EntityFrameworkCore;

namespace back.DAL;

public class UserStoryPropositionContext : DbContext
{
    public UserStoryPropositionContext(DbContextOptions<UserStoryPropositionContext> options)
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityColumns();
    }
    
    public DbSet<UserStoryPropositionEntity> UserStoriesProposition { get; set; }
}