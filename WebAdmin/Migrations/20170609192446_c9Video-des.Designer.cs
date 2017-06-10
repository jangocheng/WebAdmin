using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MSDev.DB;

namespace WebAdmin.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170609192446_c9Video-des")]
    partial class c9Videodes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MSDev.DB.Models.BingNews", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Description");

                    b.Property<string>("Provider");

                    b.Property<int?>("Status");

                    b.Property<string>("Tags");

                    b.Property<string>("ThumbnailUrl");

                    b.Property<string>("Title");

                    b.Property<DateTime>("UpdatedTime");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.HasIndex("UpdatedTime");

                    b.ToTable("BingNews");
                });

            modelBuilder.Entity("MSDev.DB.Models.C9Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Duration")
                        .HasMaxLength(32);

                    b.Property<string>("SeriesTitle")
                        .HasMaxLength(128);

                    b.Property<string>("SeriesTitleUrl")
                        .HasMaxLength(256);

                    b.Property<string>("SourceUrl")
                        .HasMaxLength(256);

                    b.Property<int?>("Status");

                    b.Property<string>("ThumbnailUrl")
                        .HasMaxLength(256);

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("SeriesTitle");

                    b.HasIndex("Title");

                    b.HasIndex("UpdatedTime");

                    b.ToTable("C9Articles");
                });

            modelBuilder.Entity("MSDev.DB.Models.C9Video", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .HasMaxLength(128);

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Description")
                        .HasColumnType("ntext");

                    b.Property<string>("Duration")
                        .HasMaxLength(32);

                    b.Property<string>("Language")
                        .HasMaxLength(16);

                    b.Property<string>("Mp3Url")
                        .HasMaxLength(256);

                    b.Property<string>("Mp4HigUrl")
                        .HasMaxLength(256);

                    b.Property<string>("Mp4LowUrl")
                        .HasMaxLength(256);

                    b.Property<string>("Mp4MidUrl")
                        .HasMaxLength(256);

                    b.Property<string>("SeriesTitle")
                        .HasMaxLength(256);

                    b.Property<string>("SeriesTitleUrl")
                        .HasMaxLength(256);

                    b.Property<string>("SourceUrl")
                        .HasMaxLength(256);

                    b.Property<int?>("Status");

                    b.Property<string>("Tags")
                        .HasMaxLength(256);

                    b.Property<string>("ThumbnailUrl")
                        .HasMaxLength(256);

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.Property<DateTime>("UpdatedTime");

                    b.Property<int?>("Views");

                    b.HasKey("Id");

                    b.HasIndex("Language");

                    b.HasIndex("SeriesTitle");

                    b.HasIndex("Title");

                    b.HasIndex("UpdatedTime");

                    b.ToTable("C9Videos");
                });

            modelBuilder.Entity("MSDev.DB.Models.Catalog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int>("IsTop");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("Status");

                    b.Property<Guid?>("TopCatalogId");

                    b.Property<string>("Type");

                    b.Property<DateTime>("UpdatedTime");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("TopCatalogId");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("CataLog");
                });

            modelBuilder.Entity("MSDev.DB.Models.DevBlog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .HasMaxLength(64);

                    b.Property<string>("Category")
                        .HasMaxLength(32);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Link")
                        .HasMaxLength(128);

                    b.Property<string>("SourcContent");

                    b.Property<string>("SourceTitle")
                        .HasMaxLength(128);

                    b.Property<int>("Status");

                    b.Property<string>("Title")
                        .HasMaxLength(128);

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("Id");

                    b.ToTable("DevBlogs");
                });

            modelBuilder.Entity("MSDev.DB.Models.Resource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AbsolutePath")
                        .HasMaxLength(256);

                    b.Property<Guid?>("CatalogId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Description")
                        .HasMaxLength(1024);

                    b.Property<string>("IMGUrl")
                        .HasMaxLength(256);

                    b.Property<int>("Language");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Path")
                        .HasMaxLength(128);

                    b.Property<int?>("Status");

                    b.Property<int>("Type");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("CatalogId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Resource");
                });

            modelBuilder.Entity("MSDev.DB.Models.RssNews", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Categories");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Description");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Link");

                    b.Property<string>("MobileContent");

                    b.Property<int>("PublishId");

                    b.Property<int>("Status");

                    b.Property<string>("Title");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("RssNews");
                });

            modelBuilder.Entity("MSDev.DB.Models.Sources", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Hash")
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<Guid?>("ResourceId");

                    b.Property<int?>("Status");

                    b.Property<string>("Tag")
                        .HasMaxLength(32);

                    b.Property<string>("Type")
                        .HasMaxLength(32);

                    b.Property<DateTime>("UpdatedTime");

                    b.Property<string>("Url")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("ResourceId");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("MSDev.DB.Models.Catalog", b =>
                {
                    b.HasOne("MSDev.DB.Models.Catalog", "TopCatalog")
                        .WithMany()
                        .HasForeignKey("TopCatalogId");
                });

            modelBuilder.Entity("MSDev.DB.Models.Resource", b =>
                {
                    b.HasOne("MSDev.DB.Models.Catalog", "Catalog")
                        .WithMany("Resource")
                        .HasForeignKey("CatalogId");
                });

            modelBuilder.Entity("MSDev.DB.Models.Sources", b =>
                {
                    b.HasOne("MSDev.DB.Models.Resource", "Resource")
                        .WithMany("SourcesUrls")
                        .HasForeignKey("ResourceId");
                });
        }
    }
}
