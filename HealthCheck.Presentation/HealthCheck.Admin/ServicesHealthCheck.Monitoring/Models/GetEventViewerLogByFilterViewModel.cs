namespace ServicesHealthCheck.Monitoring.Models
{
    public class GetEventViewerLogByFilterViewModel
    {
        public string ServiceName { get; set; }
        public string EventType { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
    }
}
