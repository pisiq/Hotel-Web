using Hotel.Data;
using Hotel.ViewsModels;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Hotel.Models;
using Microsoft.AspNetCore.Identity;

namespace Hotel.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<User> _passwordHasher;



        public UserService(
           IRepository<User> userRepository,
           RoleManager<IdentityRole> roleManager,
           IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
        }


        public async Task<bool> RegisterUserAsync(SignModel model)
        {
            if (await EmailExistsAsync(model.Email))
                return false; // Email already in use

            var user = new User
            {
                Email = model.Email,
                Name = model.Name,
                Phone = model.Phone,
                ProfilePicturePath = "/images/profiles/default-avatar.png"
            };

            // Hash the password
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            // Ensure role exists
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));

            // Save user
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AuthenticateUserAsync(LoginModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null)
                return false;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

    }
}