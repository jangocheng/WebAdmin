using System;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MSDev.DB.Models;

namespace MSDev.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public void Update(object entity, object newObject)
        {
            Attach(entity);
            foreach (PropertyInfo properity in newObject.GetType().GetProperties())
            {
                if (Entry(entity).Property(properity.Name).Metadata.IsPrimaryKey()) continue;
                object value = properity.GetValue(newObject);
                if (value == null) continue;

                entity.GetType().GetProperty(properity.Name).SetValue(entity, value);
                Entry(entity).Property(properity.Name).IsModified = true;
            }
        }
        public void Add<TEntity>(object formData) where TEntity : class
        {
            var entity = Activator.CreateInstance<TEntity>();

            foreach (PropertyInfo properity in formData.GetType().GetProperties())
            {
                object value = properity.GetValue(formData);
                if (value == null) continue;

                entity.GetType().GetProperty(properity.Name).SetValue(entity, value);
            }
            entity.GetType().GetProperty("Id")?.SetValue(entity, Guid.NewGuid());
            entity.GetType().GetProperty("CreatedTime")?.SetValue(entity, DateTime.Now);
            entity.GetType().GetProperty("UpdatedTime")?.SetValue(entity, DateTime.Now);
            Set<TEntity>().Add(entity);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 添加索引:Add Index

            modelBuilder.Entity<Config>()
                .HasIndex(m => m.Type);

            modelBuilder.Entity<BingNews>()
                .HasIndex(m => m.Title)
                .IsUnique();
            modelBuilder.Entity<BingNews>()
                .HasIndex(m => m.UpdatedTime);
            modelBuilder.Entity<Resource>()
                .HasIndex(m => m.Name)
                .IsUnique();
            modelBuilder.Entity<Catalog>()
                .HasIndex(m => m.Value)
                .IsUnique();

            modelBuilder.Entity<C9Article>()
                .HasIndex(m => m.UpdatedTime);

            modelBuilder.Entity<C9Article>()
                .HasIndex(m => m.Title);
            modelBuilder.Entity<C9Article>()
                .HasIndex(m => m.SeriesTitle);

            modelBuilder.Entity<C9Video>()
                .HasIndex(m => m.UpdatedTime);
            modelBuilder.Entity<C9Video>()
                .HasIndex(m => m.Title);
            modelBuilder.Entity<C9Video>()
                .HasIndex(m => m.SeriesTitle);
            modelBuilder.Entity<C9Video>()
                .HasIndex(m => m.Language);
            modelBuilder.Entity<MvaVideo>()
                .HasIndex(m => m.Title);
            modelBuilder.Entity<MvaVideo>()
                .HasIndex(m => m.UpdatedTime);
            modelBuilder.Entity<MvaVideo>()
                .HasIndex(m => m.LanguageCode);
            base.OnModelCreating(modelBuilder);
            #endregion
        }


        public DbSet<Config> Config { set; get; }
        public DbSet<MvaVideo> MvaVideos { get; set; }
        public DbSet<C9Article> C9Articles { get; set; }
        public DbSet<C9Video> C9Videos { get; set; }

        public DbSet<RssNews> RssNews { get; set; }
        public DbSet<BingNews> BingNews { get; set; }

        public DbSet<Catalog> CataLog { get; set; }
        public DbSet<Resource> Resource { get; set; }
        public DbSet<Sources> Sources { get; set; }
        public DbSet<DevBlog> DevBlogs { get; set; }

    }
}