using Esport.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using YamlDotNet.Core.Tokens;

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

        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<EventsUser> EventsUsers { get; set; }

        public virtual DbSet<Entities.File> Files { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<GameServer> GameServers { get; set; }

        public virtual DbSet<News> News { get; set; }

        public virtual DbSet<NewsTag> NewsTags { get; set; }

        public virtual DbSet<Log> Logs { get; set; }

        public virtual DbSet<Entities.Tag> Tags { get; set; }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UsersGame> UsersGames { get; set; }

        public virtual DbSet<UsersTeam> UsersTeams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DbConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Latin1_General_100_CS_AS_SC");

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
                entity.Property(e => e.EndDateTime).HasColumnType("datetime");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Events)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Events_Users");
            });

            modelBuilder.Entity<EventsUser>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.UserId });

                entity.Property(e => e.Accepted).HasColumnType("datetime");
                entity.Property(e => e.Declined).HasColumnType("datetime");
                entity.Property(e => e.Invited).HasColumnType("datetime");

                entity.HasOne(d => d.Event).WithMany(p => p.EventsUsers)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventsUsers_Events");

                entity.HasOne(d => d.User).WithMany(p => p.EventsUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventsUsers_Users");
            });

            modelBuilder.Entity<Entities.File>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_File");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();
                entity.Property(e => e.Title).HasMaxLength(255);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.Logo).HasMaxLength(255);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<GameServer>(entity =>
            {
                entity.Property(e => e.Server)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Game).WithMany(p => p.GameServers)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameServers_Games");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.IsPublished).HasAnnotation("Relational:DefaultConstraintName", "DF_News_IsPublished");
                entity.Property(e => e.MetaDescription).HasMaxLength(160);
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.UrlSlug)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.NewsCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_News_Users");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.NewsUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_News_Users1");
            });

            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.HasKey(e => new { e.NewsId, e.TagId });

                entity.HasOne(d => d.News).WithMany(p => p.NewsTags)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsTags_News");

                entity.HasOne(d => d.Tag).WithMany(p => p.NewsTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsTags_Tags");
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

            modelBuilder.Entity<Entities.Tag>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.ValidFrom).HasColumnType("datetime");
                entity.Property(e => e.ValidTo).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "auth");

                entity.Property(e => e.AddressCity).HasMaxLength(50);
                entity.Property(e => e.AddressFloor).HasMaxLength(50);
                entity.Property(e => e.AddressPostalCode).HasMaxLength(10);
                entity.Property(e => e.AddressSide).HasMaxLength(50);
                entity.Property(e => e.AddressStreet).HasMaxLength(255);
                entity.Property(e => e.CanBringLaptop).HasAnnotation("Relational:DefaultConstraintName", "DF_Users_CanBringLaptop");
                entity.Property(e => e.CanBringPlaystation).HasAnnotation("Relational:DefaultConstraintName", "DF_Users_CanBringPlaystation");
                entity.Property(e => e.CanBringStationaryPc).HasAnnotation("Relational:DefaultConstraintName", "DF_Users_CanBringStationaryPc");
                entity.Property(e => e.ConsentShowImages).HasAnnotation("Relational:DefaultConstraintName", "DF_Users_ConsentShowImages");
                entity.Property(e => e.Mobile).HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Username).IsRequired();

                entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Users_Parent");
            });

            modelBuilder.Entity<UsersGame>(entity =>
            {
                entity.HasKey(e => new { e.GameId, e.UserId });

                entity.Property(e => e.InGameName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Game).WithMany(p => p.UsersGames)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersGames_Games");

                entity.HasOne(d => d.User).WithMany(p => p.UsersGames)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersGames_Users");
            });

            modelBuilder.Entity<UsersTeam>(entity =>
            {
                entity.HasKey(e => new { e.TeamId, e.MemberId });

                entity.HasOne(d => d.Member).WithMany(p => p.UsersTeams)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersTeams_Users");

                entity.HasOne(d => d.Team).WithMany(p => p.UsersTeams)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersTeams_Teams");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}