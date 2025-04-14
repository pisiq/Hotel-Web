using Hotel.ViewsModels;
using System.Threading.Tasks;

namespace Hotel.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(SignModel model);
        Task<bool> AuthenticateUserAsync(LoginModel model);
        Task<bool> EmailExistsAsync(string email);
    }
}