using back.Entities;
using Microsoft.EntityFrameworkCore;

namespace back.DAL;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) 
        : base(options)
    {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityColumns();
    }
    
    public DbSet<UserStoryPropositionEntity> UserStoriesProposition { get; set; }
    
    public DbSet<UserStoryEntity> UserStories { get; set; }
    
    public DbSet<NoteEntity> Notes { get; set; }
    
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<SessionEntity> Sessions { get; set; }
}