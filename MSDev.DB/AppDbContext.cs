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
		//TODO:更改成指定更新的字
		public void AutoSave(object model, string[] includeAttributions)
		{
			Attach(model);
			foreach (string item in includeAttributions)
			{
				Entry(model).Property(item).IsModified = true;
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<BingNews>()
				.HasIndex(m => m.Title)
				.IsUnique();
			builder.Entity<Resource>()
				.HasIndex(m => m.Name)
				.IsUnique();
			builder.Entity<Catalog>()
				.HasIndex(m => m.Value)
				.IsUnique();

			base.OnModelCreating(builder);
		}

		public DbSet<C9Article> C9Articles { get; set; }
		public DbSet<RssNews> RssNews { get; set; }
		public DbSet<BingNews> BingNews { get; set; }

		public DbSet<Catalog> CataLog { get; set; }
		public DbSet<Resource> Resource { get; set; }
		public DbSet<Sources> Sources { get; set; }
		public DbSet<DevBlog> DevBlogs { get; set; }

	}
}