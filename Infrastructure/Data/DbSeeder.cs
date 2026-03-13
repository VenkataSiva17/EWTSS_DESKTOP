using System.Collections.Generic;
using System.Linq;
using EWTSS_DESKTOP.Core.Models;

namespace EWTSS_DESKTOP.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            SeedFeatures(db);
            SeedRoles(db);
            SeedAdminUser(db);
            SeedAdminPermissions(db);
        }

        private static void SeedFeatures(AppDbContext db)
        {
            var featuresData = new List<(string Name, string Description)>
            {
                ("USER_CREATE", "User creation"),
                ("USER_DELETE", "User delete"),
                ("USER_UPDATE", "User update"),
                ("FORGOT_PASSWORD", "User forgot password"),
                ("CHANGE_PASSWORD", "User change password"),
                ("SCENARIO_CREATE", "Scenario creation"),
                ("SCENARIO_DUPLICATE", "Scenario duplicate"),
                ("SCENARIO_UPDATE", "Scenario update"),
                ("SCENARIO_DELETE", "Scenario delete"),
                ("DB_PURGE", "DB purge"),
                ("DB_IMPORT", "DB import"),
                ("DB_RESTORE", "DB restore"),
                ("DRS_SENT_LOG", "DRS sent log"),
                ("USER_LOG", "User log"),
                ("DRS_RECEIVE_LOG", "DRS receive log"),
                ("SYSTEM_LOG", "System log"),
                ("CREATE_EMITTER_LIBRARY", "Create emitter library"),
                ("UPDATE_EMITTER_LIBRARY", "Update emitter library"),
                ("DELETE_EMITTER_LIBRARY", "Delete emitter library"),
                ("VIEW_EMITTER_LIBRARY", "View emitter library"),
                ("CREATE_REPORT", "Create report"),
                ("UPDATE_REPORT", "Update report"),
                ("DELETE_REPORT", "Delete report"),
                ("VIEW_REPORT", "View report"),
                ("CREATE_REPLAY", "Create replay"),
                ("VIEW_REPLAY", "View replay"),
                ("CREATE_IP_CONFIGURATION", "Create IP configuration"),
                ("UPDATE_IP_CONFIGURATION", "Update IP configuration"),
                ("DELETE_IP_CONFIGURATION", "Delete IP configuration"),
                ("VERIFY_IP_CONFIGURATION", "Verify IP configuration"),
            };

            foreach (var item in featuresData)
            {
                bool exists = db.Features.Any(f => f.Name == item.Name);
                if (!exists)
                {
                    db.Features.Add(new Feature
                    {
                        Name = item.Name,
                        Description = item.Description
                    });
                }
            }

            db.SaveChanges();
        }

        private static void SeedRoles(AppDbContext db)
        {
            var rolesData = new List<(string Name, string Description)>
            {
                ("ADMIN", "admin"),
                ("OPERATOR", "operator"),
                ("INSTRUCTOR", "instructor")
            };

            foreach (var item in rolesData)
            {
                bool exists = db.Roles.Any(r => r.Name == item.Name);
                if (!exists)
                {
                    db.Roles.Add(new Role
                    {
                        Name = item.Name,
                        Description = item.Description
                    });
                }
            }

            db.SaveChanges();
        }

        private static void SeedAdminUser(AppDbContext db)
        {
            var adminRole = db.Roles.FirstOrDefault(r => r.Name == "ADMIN");
            if (adminRole == null)
                return;

            var adminUser = db.Users.FirstOrDefault(u => u.UserName == "admin");

            if (adminUser == null)
            {
                adminUser = new User
                {
                    FirstName = "admin",
                    LastName = "admin",
                    UserName = "admin",
                    Password = "admin",   // later replace with hashed password
                    RoleId = adminRole.Id,
                    CreatedBy = 0
                };

                db.Users.Add(adminUser);
                db.SaveChanges();

                // optional: make CreatedBy point to self after first save
                adminUser.CreatedBy = adminUser.Id;
                db.SaveChanges();
            }
        }

        private static void SeedAdminPermissions(AppDbContext db)
        {
            var adminRole = db.Roles.FirstOrDefault(r => r.Name == "ADMIN");
            var adminUser = db.Users.FirstOrDefault(u => u.UserName == "admin");

            if (adminRole == null || adminUser == null)
                return;

            var featureIds = db.Features.Select(f => f.Id).ToList();

            foreach (var featureId in featureIds)
            {
                bool exists = db.RolePermissions.Any(rp =>
                    rp.RoleId == adminRole.Id &&
                    rp.FeatureId == featureId);

                if (!exists)
                {
                    db.RolePermissions.Add(new RolePermission
                    {
                        RoleId = adminRole.Id,
                        FeatureId = featureId,
                        IsPermit = true,
                        UserId = adminUser.Id
                    });
                }
            }

            db.SaveChanges();
        }
    }
}