namespace EWTSS_DESKTOP.Core.Interfaces
{
    public interface IDbManagementRepository
    {
        bool BackupDatabase(string outputFilePath, out string message);
        bool ImportDatabase(string inputFilePath, out string message);
        bool PurgeDatabase(string[] tables, out string message);
    }
}