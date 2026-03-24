using EWTSS_DESKTOP.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EWTSS_DESKTOP.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<AreaOperation> AreaOperations { get; set; }
        public DbSet<AreaOperationPolygon> AreaOperationPolygons { get; set; }
        public DbSet<ScenarioLine> ScenarioLines { get; set; }
        public DbSet<Cc> Ccs { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<EntityPolygon> EntityPolygons { get; set; }
        public DbSet<Emitter> Emitters { get; set; }

        public DbSet<LogManagement> LogManagements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = "server=localhost;database=ewtss_db_march;user=root;password=admin@123";

            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table names
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Role>().ToTable("role");
            modelBuilder.Entity<Feature>().ToTable("feature");
            modelBuilder.Entity<RolePermission>().ToTable("role_permission");

            modelBuilder.Entity<Scenario>().ToTable("scenario");
            modelBuilder.Entity<AreaOperation>().ToTable("area_operation");
            modelBuilder.Entity<AreaOperationPolygon>().ToTable("area_operation_polygon");
            modelBuilder.Entity<ScenarioLine>().ToTable("line");
            modelBuilder.Entity<Cc>().ToTable("cc");
            modelBuilder.Entity<Entity>().ToTable("entity");
            modelBuilder.Entity<EntityPolygon>().ToTable("entity_polygon");
            modelBuilder.Entity<Emitter>().ToTable("emitter");

            // Enum conversions
            modelBuilder.Entity<Scenario>().Property(x => x.ScenarioType).HasConversion<string>();
            modelBuilder.Entity<Scenario>().Property(x => x.ScenarioStatus).HasConversion<string>();
            modelBuilder.Entity<Scenario>().Property(x => x.ExecuteRun).HasConversion<string>();
            modelBuilder.Entity<Scenario>().Property(x => x.StartStop).HasConversion<string>();

            modelBuilder.Entity<ScenarioLine>().Property(x => x.LineType).HasConversion<string>();

            modelBuilder.Entity<Entity>().Property(x => x.EntityType).HasConversion<string>();

            modelBuilder.Entity<Emitter>().Property(x => x.PlatformType).HasConversion<string>();
            modelBuilder.Entity<Emitter>().Property(x => x.Type).HasConversion<string>();

            // Relationships

            modelBuilder.Entity<User>()
                .HasMany(x => x.CreatedScenarios)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolePermission>()
                .HasOne(x => x.Role)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolePermission>()
                .HasOne(x => x.Feature)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.FeatureId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Scenario>()
                .HasMany(x => x.AreaOperations)
                .WithOne(x => x.Scenario)
                .HasForeignKey(x => x.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AreaOperation>()
                .HasMany(x => x.Polygons)
                .WithOne(x => x.AreaOperation)
                .HasForeignKey(x => x.AreaOperationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AreaOperation>()
                .HasMany(x => x.Lines)
                .WithOne(x => x.AreaOperation)
                .HasForeignKey(x => x.AreaOperationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ScenarioLine>()
                .HasMany(x => x.Ccs)
                .WithOne(x => x.Line)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ScenarioLine>()
                .HasMany(x => x.Emitters)
                .WithOne(x => x.Line)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cc>()
                .HasMany(x => x.Entities)
                .WithOne(x => x.Cc)
                .HasForeignKey(x => x.CcId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entity>()
                .HasMany(x => x.Polygons)
                .WithOne(x => x.Entity)
                .HasForeignKey(x => x.EntityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Helpful column mappings for renamed properties
            modelBuilder.Entity<User>().Property(x => x.FirstName).HasColumnName("first_name");
            modelBuilder.Entity<User>().Property(x => x.LastName).HasColumnName("last_name");
            modelBuilder.Entity<User>().Property(x => x.UserName).HasColumnName("user_name");
            modelBuilder.Entity<User>().Property(x => x.CreatedBy).HasColumnName("created_by");
            modelBuilder.Entity<User>().Property(x => x.RoleId).HasColumnName("role_id");
            modelBuilder.Entity<User>().Property(x => x.CreatedOn).HasColumnName("created_on");
            modelBuilder.Entity<User>().Property(x => x.UpdatedOn).HasColumnName("updated_on");
            modelBuilder.Entity<User>().Property(x => x.IsActive).HasColumnName("is_active");

            modelBuilder.Entity<Scenario>().Property(x => x.StartDate).HasColumnName("start_date");
            modelBuilder.Entity<Scenario>().Property(x => x.StartTime).HasColumnName("start_time");
            modelBuilder.Entity<Scenario>().Property(x => x.ScenarioType).HasColumnName("scenario_type");
            modelBuilder.Entity<Scenario>().Property(x => x.ScenarioStatus).HasColumnName("scenario_status");
            modelBuilder.Entity<Scenario>().Property(x => x.ExecuteDate).HasColumnName("execute_date");
            modelBuilder.Entity<Scenario>().Property(x => x.ExecuteTime).HasColumnName("execute_time");
            modelBuilder.Entity<Scenario>().Property(x => x.UserId).HasColumnName("user_id");
            modelBuilder.Entity<Scenario>().Property(x => x.ExecuteRun).HasColumnName("execute_run");
            modelBuilder.Entity<Scenario>().Property(x => x.StartStop).HasColumnName("start_stop");
            modelBuilder.Entity<Scenario>().Property(x => x.CreatedOn).HasColumnName("created_on");
            modelBuilder.Entity<Scenario>().Property(x => x.UpdatedOn).HasColumnName("updated_on");
            modelBuilder.Entity<Scenario>().Property(x => x.IsActive).HasColumnName("is_active");

            modelBuilder.Entity<AreaOperation>().Property(x => x.ScenarioId).HasColumnName("scenario_id");
            modelBuilder.Entity<AreaOperation>().Property(x => x.CreatedOn).HasColumnName("created_on");
            modelBuilder.Entity<AreaOperation>().Property(x => x.UpdatedOn).HasColumnName("updated_on");
            modelBuilder.Entity<AreaOperation>().Property(x => x.IsActive).HasColumnName("is_active");

            modelBuilder.Entity<AreaOperationPolygon>().Property(x => x.AreaOperationId).HasColumnName("area_operation_id");
            modelBuilder.Entity<AreaOperationPolygon>().Property(x => x.CreatedOn).HasColumnName("created_on");
            modelBuilder.Entity<AreaOperationPolygon>().Property(x => x.UpdatedOn).HasColumnName("updated_on");
            modelBuilder.Entity<AreaOperationPolygon>().Property(x => x.IsActive).HasColumnName("is_active");

            modelBuilder.Entity<ScenarioLine>().Property(x => x.AreaOperationId).HasColumnName("area_operation_id");
            modelBuilder.Entity<ScenarioLine>().Property(x => x.LineType).HasColumnName("line_type");
            modelBuilder.Entity<ScenarioLine>().Property(x => x.CreatedOn).HasColumnName("created_on");
            modelBuilder.Entity<ScenarioLine>().Property(x => x.UpdatedOn).HasColumnName("updated_on");
            modelBuilder.Entity<ScenarioLine>().Property(x => x.IsActive).HasColumnName("is_active");

            modelBuilder.Entity<Cc>().Property(x => x.CcName).HasColumnName("cc_name");
            modelBuilder.Entity<Cc>().Property(x => x.LineId).HasColumnName("line_id");

            modelBuilder.Entity<Entity>().Property(x => x.StartFrequencyValue).HasColumnName("start_frequency_value");
            modelBuilder.Entity<Entity>().Property(x => x.StopFrequencyValue).HasColumnName("stop_frequency_value");
            modelBuilder.Entity<Entity>().Property(x => x.AntennaType).HasColumnName("antenna_type");
            modelBuilder.Entity<Entity>().Property(x => x.AntennaHeight).HasColumnName("antenna_height");
            modelBuilder.Entity<Entity>().Property(x => x.ScanType).HasColumnName("scan_type");
            modelBuilder.Entity<Entity>().Property(x => x.CcId).HasColumnName("cc_id");
            modelBuilder.Entity<Entity>().Property(x => x.EntityType).HasColumnName("entity_type");

            modelBuilder.Entity<EntityPolygon>().Property(x => x.EntityId).HasColumnName("entity_id");
            modelBuilder.Entity<EntityPolygon>().Property(x => x.CreatedOn).HasColumnName("created_on");
            modelBuilder.Entity<EntityPolygon>().Property(x => x.UpdatedOn).HasColumnName("updated_on");
            modelBuilder.Entity<EntityPolygon>().Property(x => x.IsActive).HasColumnName("is_active");

            modelBuilder.Entity<Emitter>().Property(x => x.PlatformType).HasColumnName("platform_type");
            modelBuilder.Entity<Emitter>().Property(x => x.ModeType).HasColumnName("mode_type");
            modelBuilder.Entity<Emitter>().Property(x => x.PowerTransmitted).HasColumnName("power_transmitted");
            modelBuilder.Entity<Emitter>().Property(x => x.StartFrequencyValue).HasColumnName("start_frequency_value");
            modelBuilder.Entity<Emitter>().Property(x => x.StopFrequencyValue).HasColumnName("stop_frequency_value");
            modelBuilder.Entity<Emitter>().Property(x => x.HopPeriodValue).HasColumnName("hop_period_value");
            modelBuilder.Entity<Emitter>().Property(x => x.HopPeriodUnit).HasColumnName("hop_period_unit");
            modelBuilder.Entity<Emitter>().Property(x => x.HopInterPeriodValue).HasColumnName("hop_inter_period_value");
            modelBuilder.Entity<Emitter>().Property(x => x.HopInterPeriodUnit).HasColumnName("hop_inter_period_unit");
            modelBuilder.Entity<Emitter>().Property(x => x.ModulationType).HasColumnName("modulation_type");
            modelBuilder.Entity<Emitter>().Property(x => x.PatternType).HasColumnName("pattern_type");
            modelBuilder.Entity<Emitter>().Property(x => x.ScanRate).HasColumnName("scan_rate");
            modelBuilder.Entity<Emitter>().Property(x => x.AntennaType).HasColumnName("antenna_type");
            modelBuilder.Entity<Emitter>().Property(x => x.LineId).HasColumnName("line_id");
            modelBuilder.Entity<Emitter>().Property(x => x.EmitterType).HasColumnName("emitter_type");
            modelBuilder.Entity<Emitter>().Property(x => x.Type).HasColumnName("type");
            modelBuilder.Entity<Emitter>().Property(x => x.CreatedOn).HasColumnName("created_on");
            modelBuilder.Entity<Emitter>().Property(x => x.UpdatedOn).HasColumnName("updated_on");
            modelBuilder.Entity<Emitter>().Property(x => x.IsActive).HasColumnName("is_active");
        }
    }
}