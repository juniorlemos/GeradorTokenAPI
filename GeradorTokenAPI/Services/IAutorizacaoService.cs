using GeradorDeTokenAPI.Models;
using GeradorDeTokenAPI.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GeradorDeTokenAPI.Services
{
    public interface IAutorizacaoService
    {
        Task<string> CriarSenha();
        Task<bool> ValidarSenha(string senha);
        Task<bool> LogarUsuario(EntradaUsuario usuario);
        Task<IdentityResult> RegistrarUsuario(EntradaUsuario usuario);
        Task<TelaToken> GerarJwt(string nome);

    }
}
