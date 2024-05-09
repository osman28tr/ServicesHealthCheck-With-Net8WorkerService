namespace ServicesHealthCheck.Monitoring.Models
{
    public class HealthCheckByFilterViewModel
    {
        public string? ServiceName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
