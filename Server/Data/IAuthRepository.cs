using System.Threading.Tasks;
using BlazorBattles.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorBattles.Server.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<bool> UserExists(string email);
    }

    class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;

        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int>(Data: 0, Message: "User already exists", false);
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var protectedUser = user with { PasswordHash = passwordHash, PasswordSalt = passwordSalt};
            await _dataContext.Users.AddAsync(protectedUser);
            await _dataContext.SaveChangesAsync();

            return new ServiceResponse<int>( Data: protectedUser.Id, Message: "Registration Successful");
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            ServiceResponse<string> response;
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                return new ServiceResponse<string>("", "User not found.", false);
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return new ServiceResponse<string>("", "Wrong password.", false);
            }

            return new ServiceResponse<string>(user.Id.ToString(), "");
        }

        public async Task<bool> UserExists(string email)
        {
            return await _dataContext.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}