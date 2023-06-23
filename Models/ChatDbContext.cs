namespace TestSignalR.Models;

using Microsoft.EntityFrameworkCore;

public class ChatDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;database=TestSignalR;User Id=sa;Password=Qu@ng2002;Persist Security Info=False;Encrypt=False;trusted_connection=false;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>();
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Chat> Chats { get; set; } = null!;
}