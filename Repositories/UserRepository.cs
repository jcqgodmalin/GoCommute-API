using GoCommute.Data;
using Microsoft.EntityFrameworkCore;

namespace GoCommute.Repositories
{

    public interface IUserReporitory {
        Task<User?> GetUser(int? id, string? email, string? appid = null, string? secretkey = null);
        Task AddUser(User user);
        Task<string?> GetSecretKeyByAppID(string AppID);
    }

    public class UserRepository : IUserReporitory
    {
        private readonly AppDBContext _context;

        public UserRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUser(int? id = null, string? email = null, string? appid = null, string? secretkey = null){
            if(id.HasValue){
                return await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            }

            if(!string.IsNullOrEmpty(email)){
                return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            }

            if(!string.IsNullOrEmpty(appid) && !string.IsNullOrEmpty(secretkey)){
                return await _context.Users.SingleOrDefaultAsync(u => u.AppID == appid && u.SecretKey == secretkey);
            }

            return null;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string?> GetSecretKeyByAppID(string AppID){
            var user = await _context.Users.SingleOrDefaultAsync(u => u.AppID == AppID);
            if(user != null){
                return user.SecretKey;
            }
            return null;
        }
    }
}