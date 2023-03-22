using back.Entities;
using Microsoft.EntityFrameworkCore;

namespace back.DAL;

public class UserStoryContext : DbContext
{
    public UserStoryContext(DbContextOptions<UserStoryContext> options)
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityColumns();
    }
    
    public DbSet<UserStoryEntity> UserStories { get; set; }
}