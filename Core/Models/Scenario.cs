namespace EWTSS_DESKTOP.Core.Models
{
    public enum ScenarioType
    {
        ORIGINAL,
        DUPLICATE
    }

    public enum ScenarioStatus
    {
        COMPLETED,
        PLANNED,
        INPROGRESS
    }

    public enum ExecuteRun
    {
        EXECUTE,
        RUN
    }

    public enum StartStop
    {
        START,
        STOP
    }

    public class Scenario : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? Duration { get; set; }

        public ScenarioType ScenarioType { get; set; }
        public ScenarioStatus ScenarioStatus { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime? ExecuteDate { get; set; }
        public TimeSpan? ExecuteTime { get; set; }

        public ExecuteRun ExecuteRun { get; set; } = ExecuteRun.EXECUTE;
        public StartStop StartStop { get; set; } = StartStop.START;

        public ICollection<AreaOperation> AreaOperations { get; set; } = new List<AreaOperation>();
    }
}