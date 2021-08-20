using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GeradorDeTokenAPI.Error
{
    public class PasswordValidacao : IPasswordValidator<IdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string password)
        {
            return Task.FromResult(Validate(user, password));
        }

        private IdentityResult Validate(IdentityUser user, string password)
        {
            if (password.Contains(user.UserName, System.StringComparison.OrdinalIgnoreCase))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "Nome do Usuario e Senha",
                    Description = "Senha não pode ser o mesmo nome do usuario"
                });
            }

            return IdentityResult.Success;
        }
    }

}