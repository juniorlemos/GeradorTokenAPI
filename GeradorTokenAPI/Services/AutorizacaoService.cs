using GeradorDeTokenAPI.Models;
using GeradorDeTokenAPI.Models.ViewModels;
using GeradorDeTokenAPI.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GeradorDeTokenAPI.Services
{
    public class AutorizacaoService : IAutorizacaoService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly AppSettings _appSettings;


        public AutorizacaoService(IOptions<IdentityOptions> identityOptions,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOptions<AppSettings> appSettings)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _identityOptions = identityOptions;
            _appSettings = appSettings.Value;

        }



        public async Task<IdentityResult> RegistrarUsuario(EntradaUsuario usuario)
        {
            var registro = new IdentityUser
            {
                UserName = usuario.nome,
                PasswordHash = usuario.senha,
            };

            return await _userManager.CreateAsync(registro, usuario.senha);


        }
        public async Task<string> CriarSenha()
        {

            PasswordOptions opts = _identityOptions.Value.Password;



            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@_-#"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return  new string(chars.ToArray());


        }

        public async Task<bool> LogarUsuario(EntradaUsuario usuario)
        {
            var resultado = await _signInManager.PasswordSignInAsync(usuario.nome,
                usuario.senha, isPersistent: false, lockoutOnFailure: false);

            return resultado.Succeeded ; 
          

        }








        public async Task<bool> ValidarSenha(string senha)
        {
            PasswordOptions opts = _identityOptions.Value.Password;


            if (!(senha.Length >= opts.RequiredLength)) return false;
            if (!(senha.Any(char.IsUpper))) return false;
            if (!(senha.Any(char.IsLower))) return false;
            if (!(new string[] { "!", "@", "_", "-", "#" }.Any(s => senha.Contains(s)))) return false;
            if ((senha.Distinct().Count() == opts.RequiredUniqueChars)) return false;

            return true;


        }



        public async Task<TelaToken> GerarJwt(string nome)
        {
            var user = await _userManager.FindByNameAsync(nome);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.ExpiracaoMinutos),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)

            };
            return new TelaToken
            {
                Usuario = nome,
                Autenticado = true,
                Token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                DataDeExpiracaoEmMinutos = _appSettings.ExpiracaoMinutos

            };


        }


    }



}
