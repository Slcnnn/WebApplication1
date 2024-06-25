using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class AppDbContext: IdentityDbContext<AppUser,AppRole,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<Chat> chats { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUsers> GroupUsers { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Sender)
                .WithMany()
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Recipient)
                .WithMany()
                .HasForeignKey(c => c.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
