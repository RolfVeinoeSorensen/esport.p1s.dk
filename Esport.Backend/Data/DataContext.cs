using Microsoft.EntityFrameworkCore;
using Esport.Backend.Entities;

namespace Esport.Backend.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DbConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "auth");

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.Username).IsRequired();
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Log", "log");
                entity.Property(e => e.MachineName)
                .IsRequired()
                .HasMaxLength(255);
                entity.Property(e => e.Level)
                .IsRequired()
                .HasMaxLength(5);
                entity.Property(e => e.Logger)
                .IsRequired()
                .HasMaxLength(300);
                entity.Property(e => e.Callsite)
                .IsRequired()
                .HasMaxLength(300);
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}