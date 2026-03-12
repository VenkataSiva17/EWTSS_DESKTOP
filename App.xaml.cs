using System.Windows;

using EWTSS_DESKTOP.Data;

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