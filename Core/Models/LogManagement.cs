using EWTSS_DESKTOP.Core.Enums;

namespace EWTSS_DESKTOP.Core.Models
{
    public class LogManagement : BaseEntity
    {
        public LogNameEnum LogName { get; set; }

        public string LogType { get; set; }

        public string Module { get; set; }

        public string FunctionName { get; set; }

        public string Message { get; set; }
    }
}