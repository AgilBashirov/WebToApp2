namespace WebToApp2.Models.Sima
{
    public class ClientInfo
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string IconURI { get; set; }
        public string Callback { get; set; }
        public string[]? HostName { get; set; }
        public string? RedirectURI { get; set; }
    }
}
