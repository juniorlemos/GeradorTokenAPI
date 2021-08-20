namespace GeradorDeTokenAPI.Models.ViewModels
{
    public class TelaToken
    {
        public string Usuario { get; set; }
        public bool Autenticado { get; set; }
        public string Token { get; set; }
        public int DataDeExpiracaoEmMinutos { get; set; }

    }
}
