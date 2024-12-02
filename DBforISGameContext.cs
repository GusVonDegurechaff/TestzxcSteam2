using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ihatedthiswork
{
    public partial class DBforISGameContext : DbContext
    {
        public DBforISGameContext()
        {
        }

        public DBforISGameContext(DbContextOptions<DBforISGameContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Friend> Friends { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Profile> Profiles { get; set; } = null!;
        public virtual DbSet<Statistic> Statistics { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserGame> UserGames { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS01;Database=DBforISGame;TrustServerCertificate=True;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasKey(e => e.FriendshipId)
                    .HasName("PK__Friends__BC802BCF826EA37E");

                entity.Property(e => e.FriendshipId).HasColumnName("friendship_id");

                entity.Property(e => e.UserId1).HasColumnName("user_id_1");

                entity.Property(e => e.UserId2).HasColumnName("user_id_2");

                entity.HasOne(d => d.UserId1Navigation)
                    .WithMany(p => p.Friends)
                    .HasForeignKey(d => d.UserId1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Friends_Users");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.CoverImage)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("cover_image");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Developer)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("developer");

                entity.Property(e => e.GameName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("game_name");

                entity.Property(e => e.Genre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("genre");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("date")
                    .HasColumnName("release_date");
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.ToTable("Profile");

                entity.Property(e => e.ProfileId).HasColumnName("profile_id");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.ProfileName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("profile_name");

                entity.Property(e => e.StatId).HasColumnName("stat_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profile_Games");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Profiles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Profile_Users");
            });

            modelBuilder.Entity<Statistic>(entity =>
            {
                entity.HasKey(e => e.StatId);

                entity.ToTable("Statistic");

                entity.Property(e => e.StatId).HasColumnName("stat_id");

                entity.Property(e => e.HoursInGame)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("hoursInGame");

                entity.Property(e => e.Lvl)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lvl");

                entity.Property(e => e.ProfileId).HasColumnName("profile_id");

                entity.Property(e => e.Rank)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("rank");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Statistics)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Statistic_Profile");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__AB6E616434C1B7EA")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "UQ__Users__F3DBC572ADF4D87E")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.LastLogin).HasColumnName("last_login");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.ProfilePicture)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("profile_picture");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnName("registration_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<UserGame>(entity =>
            {
                entity.HasIndex(e => new { e.UserId, e.GameId }, "UQ__UserGame__564026F23659E585")
                    .IsUnique();

                entity.Property(e => e.UserGameId).HasColumnName("user_game_id");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.UserGames)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserGames__game___4BAC3F29");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGames)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserGames__user___4CA06362");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
