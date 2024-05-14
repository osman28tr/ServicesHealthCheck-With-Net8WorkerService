namespace ServicesHealthCheck.Monitoring.Models
{
    public class CreateServiceRuleModel
    {
        public string ServiceName { get; set; }
        public string EventMessage { get; set; }
        public string EventType { get; set; }
        public bool IsRestarted { get; set; }
        public string Time { get; set; }
        public string Period { get; set; }
    }
}
