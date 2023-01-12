namespace DevIO.Api.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpirationTime { get; set; }
        public string Issuer { get; set; }        
        public string ValidOn { get; set; }

    }
}
