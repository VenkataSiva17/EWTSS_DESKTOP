using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EWTSS_DESKTOP.Infrastructure.Repositories;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Presentation.ViewModels;
using EWTSS_DESKTOP.Presentation.Views;
using EWTSS_DESKTOP.Presentation.Views.Dashboard;
using EWTSS_DESKTOP.Data;
namespace EWTSS_DESKTOP;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var db = new AppDbContext();
        var userService = new UserService(new UserRepository(db));

        var loginVM = new LoginViewModel(userService);
        var loginPage = new LoginView { DataContext = loginVM };

        loginVM.LoginSucceeded += (user) =>
        {
            // Navigate to DashboardPage inside MainFrame
            var dashboardPage = new DashboardPage(user);
            MainFrame.Navigate(dashboardPage);
        };

        MainFrame.Navigate(loginPage);
    }
}