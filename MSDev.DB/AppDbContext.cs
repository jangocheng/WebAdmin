using System;
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
		public void AutoSave(object model, string[] includeAttributions)
		{
			Attach(model);
			foreach (string item in includeAttributions)
			{
				Entry(model).Property(item).IsModified = true;
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region 添加索引:Add Index

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
			base.OnModelCreating(modelBuilder);

			#endregion

		}

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