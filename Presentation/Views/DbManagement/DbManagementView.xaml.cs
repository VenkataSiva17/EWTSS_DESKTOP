using System.Windows.Controls;
using System.Windows.Threading;
using EWTSS_DESKTOP.Helpers;
using EWTSS_DESKTOP.Infrastructure.Repositories;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Presentation.ViewModels;

namespace EWTSS_DESKTOP.Presentation.Views.DbManagement
{
    public partial class DbManagementView : Page
    {
        private readonly DispatcherTimer _clockTimer;

        public DbManagementView()
        {
            InitializeComponent();

            var repository = new DbManagementRepository(
                "localhost",
                "ewtss_db_march",
                "root",
                "DDD12");

            var service = new DbManagementService(repository);

            DataContext = new DbManagementViewModel(service);
            _clockTimer = ClockHelper.StartClock(TimeText);
        }
    }
}