using Microsoft.EntityFrameworkCore;
using EWTSS_DESKTOP.Infrastructure.Data;

namespace EWTSS_DESKTOP
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(System.Windows.StartupEventArgs e)
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