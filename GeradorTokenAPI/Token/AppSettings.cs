namespace GeradorDeTokenAPI.Token
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpiracaoMinutos { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
