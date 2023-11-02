namespace WebToApp2.Models.Sima
{
    public class OperationInfo
    {
        public string Type { get; set; }
        public string OperationId { get; set; }
        public long NbfUTC { get; set; }
        public long ExpUTC { get; set; }
        public List<string> Assignee { get; set; } = new();
    }
}
