using System.Windows;
using Microsoft.EntityFrameworkCore;
using EWTSS_DESKTOP.Infrastructure.Data;

namespace EWTSS_DESKTOP
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                db.Database.Migrate();
                DbSeeder.Seed(db);
            }

            base.OnStartup(e);
        }
    }
}