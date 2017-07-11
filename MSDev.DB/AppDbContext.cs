using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MSDev.DB.Entities;

namespace MSDev.DB
{
    public partial class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        #region DbSet
        public DbSet<Config> Config { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<BingNews> BingNews { get; set; }
        public virtual DbSet<C9Articles> C9Articles { get; set; }
        public virtual DbSet<C9Videos> C9Videos { get; set; }
        public virtual DbSet<Catalog> Catalog { get; set; }
        public virtual DbSet<DevBlogs> DevBlogs { get; set; }
        public virtual DbSet<MvaVideos> MvaVideos { get; set; }
        public virtual DbSet<Resource> Resource { get; set; }
        public virtual DbSet<RssNews> RssNews { get; set; }
        public virtual DbSet<Sources> Sources { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=msdev;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetUserRoles_RoleId");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<BingNews>(entity =>
            {
                entity.HasIndex(e => e.UpdatedTime)
                    .HasName("IX_BingNews_UpdatedTime");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<C9Articles>(entity =>
            {
                entity.ToTable("C9Articles");

                entity.HasIndex(e => e.SeriesTitle)
                    .HasName("IX_C9Articles_SeriesTitle");

                entity.HasIndex(e => e.Title)
                    .HasName("IX_C9Articles_Title");

                entity.HasIndex(e => e.UpdatedTime)
                    .HasName("IX_C9Articles_UpdatedTime");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Duration).HasMaxLength(32);

                entity.Property(e => e.SeriesTitle).HasMaxLength(128);

                entity.Property(e => e.SeriesTitleUrl).HasMaxLength(256);

                entity.Property(e => e.SourceUrl).HasMaxLength(256);

                entity.Property(e => e.ThumbnailUrl).HasMaxLength(256);

                entity.Property(e => e.Title).HasMaxLength(256);
            });

            modelBuilder.Entity<C9Videos>(entity =>
            {
                entity.ToTable("C9Videos");

                entity.HasIndex(e => e.Language)
                    .HasName("IX_C9Videos_Language");

                entity.HasIndex(e => e.SeriesTitle)
                    .HasName("IX_C9Videos_SeriesTitle");

                entity.HasIndex(e => e.Title)
                    .HasName("IX_C9Videos_Title");

                entity.HasIndex(e => e.UpdatedTime)
                    .HasName("IX_C9Videos_UpdatedTime");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Author).HasMaxLength(256);

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Duration).HasMaxLength(32);

                entity.Property(e => e.Language).HasMaxLength(32);

                entity.Property(e => e.Mp3Url).HasMaxLength(512);

                entity.Property(e => e.Mp4HigUrl).HasMaxLength(512);

                entity.Property(e => e.Mp4LowUrl).HasMaxLength(512);

                entity.Property(e => e.Mp4MidUrl).HasMaxLength(512);

                entity.Property(e => e.SeriesTitle).HasMaxLength(512);

                entity.Property(e => e.SeriesTitleUrl).HasMaxLength(512);

                entity.Property(e => e.SourceUrl).HasMaxLength(512);

                entity.Property(e => e.Tags).HasMaxLength(512);

                entity.Property(e => e.ThumbnailUrl).HasMaxLength(512);

                entity.Property(e => e.Title).HasMaxLength(512);
            });

            modelBuilder.Entity<Catalog>(entity =>
            {
                entity.HasIndex(e => e.TopCatalogId)
                    .HasName("IX_CataLog_TopCatalogId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.TopCatalog)
                    .WithMany(p => p.InverseTopCatalog)
                    .HasForeignKey(d => d.TopCatalogId);
            });

            modelBuilder.Entity<DevBlogs>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Author).HasMaxLength(64);

                entity.Property(e => e.Category).HasMaxLength(32);

                entity.Property(e => e.Link).HasMaxLength(128);

                entity.Property(e => e.SourceTitle).HasMaxLength(128);

                entity.Property(e => e.Title).HasMaxLength(128);
            });

            modelBuilder.Entity<MvaVideos>(entity =>
            {
                entity.HasIndex(e => e.LanguageCode)
                    .HasName("IX_MvaVideos_LanguageCode");

                entity.HasIndex(e => e.Title)
                    .HasName("IX_MvaVideos_Title");

                entity.HasIndex(e => e.UpdatedTime)
                    .HasName("IX_MvaVideos_UpdatedTime");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Author).HasMaxLength(768);

                entity.Property(e => e.AuthorCompany).HasMaxLength(384);

                entity.Property(e => e.AuthorJobTitle).HasMaxLength(1024);

                entity.Property(e => e.CourseDuration).HasMaxLength(32);

                entity.Property(e => e.CourseImage).HasMaxLength(512);

                entity.Property(e => e.CourseLevel).HasMaxLength(32);

                entity.Property(e => e.CourseNumber).HasMaxLength(128);

                entity.Property(e => e.CourseStatus).HasMaxLength(32);

                entity.Property(e => e.LanguageCode).HasMaxLength(16);

                entity.Property(e => e.SourceUrl).HasMaxLength(512);

                entity.Property(e => e.Tags).HasMaxLength(384);

                entity.Property(e => e.Technologies).HasMaxLength(384);

                entity.Property(e => e.Title).HasMaxLength(256);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasIndex(e => e.CatalogId)
                    .HasName("IX_Resource_CatelogId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AbsolutePath).HasMaxLength(256);

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.Imgurl)
                    .HasColumnName("IMGUrl")
                    .HasMaxLength(256);

                entity.Property(e => e.Language).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Path).HasMaxLength(128);

                entity.HasOne(d => d.Catalog)
                    .WithMany(p => p.Resource)
                    .HasForeignKey(d => d.CatalogId)
                    .HasConstraintName("FK_Resource_CataLog_CatelogId");
            });

            modelBuilder.Entity<Sources>(entity =>
            {
                entity.HasIndex(e => e.ResourceId)
                    .HasName("IX_Sources_ResourceId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Hash).HasMaxLength(256);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.Property(e => e.Tag).HasMaxLength(32);

                entity.Property(e => e.Type).HasMaxLength(32);

                entity.Property(e => e.Url).HasMaxLength(256);

                entity.HasOne(d => d.Resource)
                    .WithMany(p => p.Sources)
                    .HasForeignKey(d => d.ResourceId);
            });
        }
    }
}