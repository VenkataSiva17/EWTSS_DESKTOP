using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using EWTSS_DESKTOP.Commands;
using EWTSS_DESKTOP.Infrastructure.Services;

namespace EWTSS_DESKTOP.Presentation.ViewModels
{
    public class DbManagementViewModel : BaseViewModel
    {
        private readonly DbManagementService _service;

        public ICommand BackupCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand PurgeCommand { get; }
        public ICommand UploadCommand { get; }

        public DbManagementViewModel(DbManagementService service)
        {
            _service = service;

            BackupCommand = new RelayCommand(Backup);
            ImportCommand = new RelayCommand(Import);
            PurgeCommand = new RelayCommand(Purge);
            UploadCommand = new RelayCommand(Import);
        }

        private void Backup()
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "SQL File (*.sql)|*.sql",
                FileName = "backup.sql"
            };

            if (dialog.ShowDialog() != true)
                return;

            _service.Backup(dialog.FileName, out string message);
            MessageBox.Show(message);
        }

        private void Import()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "SQL File (*.sql)|*.sql"
            };

            if (dialog.ShowDialog() != true)
                return;

            if (!File.Exists(dialog.FileName))
            {
                MessageBox.Show("File not found.");
                return;
            }

            _service.Import(dialog.FileName, out string message);
            MessageBox.Show(message);
        }

        private void Purge()
        {
            if (MessageBox.Show("Delete all data?", "Confirm", MessageBoxButton.YesNo)
                != MessageBoxResult.Yes)
                return;

            _service.Purge(out string message);
            MessageBox.Show(message);
        }
    }
}