using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using WebApi.Models;
using WebApi.Database;

namespace WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _dbContext.Users.AsNoTracking().ToListAsync();
            return users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<User?> AddUserAsync(User user)
        {
            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch (System.Exception)
            {
                throw new Exception("Falha ao cadstrar usuario");
            }
        }

        public async Task<User?> DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);

            if (user == null)
            {
                return user;
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var userQuery = await GetUserByIdAsync(id);

            if (userQuery == null)
            {
                return userQuery;
            }

            _dbContext.Entry(userQuery).CurrentValues.SetValues(user);
            await _dbContext.SaveChangesAsync();

            return userQuery;
        }
    }
}