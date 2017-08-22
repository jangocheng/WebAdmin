using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MSDev.DB;

namespace WebAdmin.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170807093807_mvadetail-sequence")]
    partial class mvadetailsequence
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MSDev.DB.Entities.AspNetRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("RoleId")
                        .HasName("IX_AspNetRoleClaims_RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("MSDev.DB.Entities.AspNetRoles", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(450);

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("MSDev.DB.Entities.AspNetUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .HasName("IX_AspNetUserClaims_UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("MSDev.DB.Entities.AspNetUserLogins", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(450);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(450);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId")
                        .HasName("IX_AspNetUserLogins_UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("MSDev.DB.Entities.AspNetUserRoles", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(450);

                    b.Property<string>("RoleId")
                        .HasMaxLength(450);

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId")
                        .HasName("IX_AspNetUserRoles_RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("MSDev.DB.Entities.AspNetUsers", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(450);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MSDev.DB.Entities.AspNetUserTokens", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(450);

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(450);

                    b.Property<string>("Name")
                        .HasMaxLength(450);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MSDev.DB.Entities.BingNews", b =>
                {
                    b.Property<Guid>("Id");

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

                    b.HasIndex("UpdatedTime")
                        .HasName("IX_BingNews_UpdatedTime");

                    b.ToTable("BingNews");
                });

            modelBuilder.Entity("MSDev.DB.Entities.C9Articles", b =>
                {
                    b.Property<Guid>("Id");

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

                    b.HasIndex("SeriesTitle")
                        .HasName("IX_C9Articles_SeriesTitle");

                    b.HasIndex("Title")
                        .HasName("IX_C9Articles_Title");

                    b.HasIndex("UpdatedTime")
                        .HasName("IX_C9Articles_UpdatedTime");

                    b.ToTable("C9Articles");
                });

            modelBuilder.Entity("MSDev.DB.Entities.C9Videos", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Author")
                        .HasMaxLength(256);

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Description")
                        .HasColumnType("ntext");

                    b.Property<string>("Duration")
                        .HasMaxLength(32);

                    b.Property<string>("Language")
                        .HasMaxLength(32);

                    b.Property<string>("Mp3Url")
                        .HasMaxLength(512);

                    b.Property<string>("Mp4HigUrl")
                        .HasMaxLength(512);

                    b.Property<string>("Mp4LowUrl")
                        .HasMaxLength(512);

                    b.Property<string>("Mp4MidUrl")
                        .HasMaxLength(512);

                    b.Property<string>("SeriesTitle")
                        .HasMaxLength(512);

                    b.Property<string>("SeriesTitleUrl")
                        .HasMaxLength(512);

                    b.Property<string>("SourceUrl")
                        .HasMaxLength(512);

                    b.Property<int?>("Status");

                    b.Property<string>("Tags")
                        .HasMaxLength(512);

                    b.Property<string>("ThumbnailUrl")
                        .HasMaxLength(512);

                    b.Property<string>("Title")
                        .HasMaxLength(512);

                    b.Property<DateTime>("UpdatedTime");

                    b.Property<int?>("Views");

                    b.HasKey("Id");

                    b.HasIndex("Language")
                        .HasName("IX_C9Videos_Language");

                    b.HasIndex("SeriesTitle")
                        .HasName("IX_C9Videos_SeriesTitle");

                    b.HasIndex("Title")
                        .HasName("IX_C9Videos_Title");

                    b.HasIndex("UpdatedTime")
                        .HasName("IX_C9Videos_UpdatedTime");

                    b.ToTable("C9Videos");
                });

            modelBuilder.Entity("MSDev.DB.Entities.Catalog", b =>
                {
                    b.Property<Guid>("Id");

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

                    b.HasIndex("TopCatalogId")
                        .HasName("IX_CataLog_TopCatalogId");

                    b.ToTable("Catalog");
                });

            modelBuilder.Entity("MSDev.DB.Entities.Config", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(32);

                    b.Property<string>("Type")
                        .HasMaxLength(32);

                    b.Property<string>("Value")
                        .HasMaxLength(4000);

                    b.HasKey("Id");

                    b.ToTable("Config");
                });

            modelBuilder.Entity("MSDev.DB.Entities.DevBlogs", b =>
                {
                    b.Property<Guid>("Id");

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

            modelBuilder.Entity("MSDev.DB.Entities.MvaDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<DateTime>("Duration");

                    b.Property<string>("HighDownloadUrl")
                        .HasMaxLength(500);

                    b.Property<string>("LowDownloadUrl")
                        .HasMaxLength(500);

                    b.Property<string>("MidDownloadUrl")
                        .HasMaxLength(500);

                    b.Property<string>("MvaId")
                        .HasMaxLength(32);

                    b.Property<Guid?>("MvaVideoId");

                    b.Property<int>("Sequence")
                        .HasMaxLength(3);

                    b.Property<string>("SourceUrl")
                        .HasMaxLength(500);

                    b.Property<int?>("Status");

                    b.Property<string>("Title")
                        .HasMaxLength(128);

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("MvaVideoId");

                    b.ToTable("MvaDetails");
                });

            modelBuilder.Entity("MSDev.DB.Entities.MvaVideos", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Author")
                        .HasMaxLength(768);

                    b.Property<string>("AuthorCompany")
                        .HasMaxLength(384);

                    b.Property<string>("AuthorJobTitle")
                        .HasMaxLength(1024);

                    b.Property<string>("CourseDuration")
                        .HasMaxLength(32);

                    b.Property<string>("CourseImage")
                        .HasMaxLength(512);

                    b.Property<string>("CourseLevel")
                        .HasMaxLength(32);

                    b.Property<string>("CourseNumber")
                        .HasMaxLength(128);

                    b.Property<string>("CourseStatus")
                        .HasMaxLength(32);

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Description");

                    b.Property<string>("DetailDescription")
                        .HasMaxLength(4000);

                    b.Property<bool>("IsRecommend");

                    b.Property<string>("LanguageCode")
                        .HasMaxLength(16);

                    b.Property<int?>("MvaId");

                    b.Property<int?>("ProductPackageVersionId");

                    b.Property<string>("SourceUrl")
                        .HasMaxLength(512);

                    b.Property<int?>("Status");

                    b.Property<string>("Tags")
                        .HasMaxLength(384);

                    b.Property<string>("Technologies")
                        .HasMaxLength(384);

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("LanguageCode")
                        .HasName("IX_MvaVideos_LanguageCode");

                    b.HasIndex("Technologies")
                        .HasName("IX_MvaVideos_Technologies");

                    b.HasIndex("Title")
                        .HasName("IX_MvaVideos_Title");

                    b.HasIndex("UpdatedTime")
                        .HasName("IX_MvaVideos_UpdatedTime");

                    b.ToTable("MvaVideos");
                });

            modelBuilder.Entity("MSDev.DB.Entities.Resource", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("AbsolutePath")
                        .HasMaxLength(256);

                    b.Property<Guid?>("CatalogId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Description")
                        .HasMaxLength(1024);

                    b.Property<string>("Imgurl")
                        .HasColumnName("IMGUrl")
                        .HasMaxLength(256);

                    b.Property<bool>("IsRecommend");

                    b.Property<int>("Language")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Path")
                        .HasMaxLength(128);

                    b.Property<string>("Provider")
                        .HasMaxLength(128);

                    b.Property<int?>("Status");

                    b.Property<string>("Tag")
                        .HasMaxLength(128);

                    b.Property<int>("Type");

                    b.Property<DateTime>("UpdatedTime");

                    b.Property<int>("ViewNumber");

                    b.HasKey("Id");

                    b.HasIndex("CatalogId")
                        .HasName("IX_Resource_CatelogId");

                    b.ToTable("Resource");
                });

            modelBuilder.Entity("MSDev.DB.Entities.RssNews", b =>
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

            modelBuilder.Entity("MSDev.DB.Entities.Sources", b =>
                {
                    b.Property<Guid>("Id");

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

                    b.HasIndex("ResourceId")
                        .HasName("IX_Sources_ResourceId");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("MSDev.DB.Entities.AspNetRoleClaims", b =>
                {
                    b.HasOne("MSDev.DB.Entities.AspNetRoles", "Role")
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MSDev.DB.Entities.AspNetUserClaims", b =>
                {
                    b.HasOne("MSDev.DB.Entities.AspNetUsers", "User")
                        .WithMany("AspNetUserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MSDev.DB.Entities.AspNetUserLogins", b =>
                {
                    b.HasOne("MSDev.DB.Entities.AspNetUsers", "User")
                        .WithMany("AspNetUserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MSDev.DB.Entities.AspNetUserRoles", b =>
                {
                    b.HasOne("MSDev.DB.Entities.AspNetRoles", "Role")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MSDev.DB.Entities.AspNetUsers", "User")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MSDev.DB.Entities.Catalog", b =>
                {
                    b.HasOne("MSDev.DB.Entities.Catalog", "TopCatalog")
                        .WithMany("InverseTopCatalog")
                        .HasForeignKey("TopCatalogId");
                });

            modelBuilder.Entity("MSDev.DB.Entities.MvaDetails", b =>
                {
                    b.HasOne("MSDev.DB.Entities.MvaVideos", "MvaVideo")
                        .WithMany("Details")
                        .HasForeignKey("MvaVideoId");
                });

            modelBuilder.Entity("MSDev.DB.Entities.Resource", b =>
                {
                    b.HasOne("MSDev.DB.Entities.Catalog", "Catalog")
                        .WithMany("Resource")
                        .HasForeignKey("CatalogId")
                        .HasConstraintName("FK_Resource_CataLog_CatelogId");
                });

            modelBuilder.Entity("MSDev.DB.Entities.Sources", b =>
                {
                    b.HasOne("MSDev.DB.Entities.Resource", "Resource")
                        .WithMany("Sources")
                        .HasForeignKey("ResourceId");
                });
        }
    }
}
