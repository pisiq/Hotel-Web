using Hotel.Data;
using Hotel.ViewsModels;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Hotel.Models;

namespace Hotel.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUserAsync(SignModel model)
        {
            if (await EmailExistsAsync(model.Email))
            {
                return false; // Email already in use
            }

            var user = new User
            {
                Email = model.Email,
                Name = model.Name,
                Phone = model.Phone,
                Password = HashPassword(model.Password)
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AuthenticateUserAsync(LoginModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);

            if (user == null)
            {
                return false; // User not found
            }

            // Verify password hash
            return user.Password == HashPassword(model.Password);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }

        private string HashPassword(string password)
        {
            // This is a simple hash for demonstration purposes
            // In a real application, use a proper password hashing library like BCrypt
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}