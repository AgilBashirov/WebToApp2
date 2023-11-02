namespace WebToApp2.Models.Sima
{
    public class Contract
    {
        public Contract()
        {
            SignableContainer = new SignableContainer();
            Header = new Header();
        }
        public SignableContainer SignableContainer { get; set; }
        public Header Header { get; set; }
    }
}
