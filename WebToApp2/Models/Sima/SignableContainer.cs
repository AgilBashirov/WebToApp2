namespace WebToApp2.Models.Sima
{
    public class SignableContainer
    {
        public ProtoInfo ProtoInfo { get; set; } = new();
        public OperationInfo OperationInfo { get; set; } = new();
        public ClientInfo ClientInfo { get; set; } = new();
        public DataInfo DataInfo { get; set; } = new();
    }
}
