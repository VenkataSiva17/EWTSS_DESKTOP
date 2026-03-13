using System.Windows;
using EWTSS_DESKTOP.Infrastructure.Data;
using EWTSS_DESKTOP.Core.Models;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();
        }

        base.OnStartup(e);
    }
}