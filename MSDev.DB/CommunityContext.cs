using Microsoft.EntityFrameworkCore;
using MSDev.DB.Entities;

namespace MSDev.DB
{
    public class CommunityContext : DbContext
    {
        #region DBSet

        public DbSet<Config> Config { get; set; }
        #endregion
        public CommunityContext(DbContextOptions<CommunityContext> options) : base(options)
        {

        }
    }
}
