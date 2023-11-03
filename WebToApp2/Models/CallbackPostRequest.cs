namespace WebToApp2.Models
{
    public class CallbackPostRequest
    {
        public string Type { get; set; }
        public string OperationId { get; set; }
        public string DataSignature { get; set; }
        public string SignedDataHash { get; set; }
        public string AlgName { get; set; }

        public string DataId { get; set; }
        public string KeyId { get; set; }
    }
}
