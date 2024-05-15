namespace ServicesHealthCheck.Monitoring.Models
{
    public class HealthCheckByFilterModel
    {
        public string? ServiceName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
