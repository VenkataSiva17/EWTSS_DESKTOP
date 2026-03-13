using Microsoft.EntityFrameworkCore;
using EWTSS_DESKTOP.Core.Models;

namespace EWTSS_DESKTOP.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql(
                "server=localhost;database=ewtss_db;user=root;password=DDD12",
                ServerVersion.AutoDetect("server=localhost;database=ewtss_db;user=root;password=DDD12")
            );
        }
    }
}