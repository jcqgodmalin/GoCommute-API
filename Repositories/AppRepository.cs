using System.Linq.Expressions;
using GoCommute.Data;
using GoCommute.Models;
using Microsoft.EntityFrameworkCore;

namespace GoCommute.Repositories
{

    public interface IAppRepository {
        Task<List<App>> GetAllApps();
        Task<List<App>> GetApp(Expression<Func<App,bool>> filter);
        Task AddApp(App app);
        Task<bool> UpdateApp(App app);
        Task<bool> DeleteApp(int id);
    }

    public class AppRepository : IAppRepository {

        private readonly AppDBContext _context;

        public AppRepository(AppDBContext context) {
            _context = context;
        }

        public async Task<List<App>> GetAllApps()
        {
            try{
                return await _context.Apps
                    .AsNoTracking()
                    .ToListAsync();
            }catch(Exception){
                return new List<App>();
            }
        }

        public Task<List<App>> GetApp(Expression<Func<App,bool>> filter){
            try{
                return _context.Apps.Where(filter).ToListAsync();
            }catch(Exception){
                return Task.FromResult(new List<App>());
            }
        }

        public async Task AddApp(App app)
        {
            try{
                await _context.Apps.AddAsync(app);
                await _context.SaveChangesAsync();
            }catch(Exception){
                
            }
        }

        public async Task<bool> DeleteApp(int id)
        {
            try{
                var app = await _context.Apps.FindAsync(id);
                if(app == null){
                    return false;
                }
                _context.Apps.Remove(app);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception){
                return false;
            }
        }

        public async Task<bool> UpdateApp(App app)
        {
            var existingApp = await _context.Apps.FindAsync(app.Id);
            if(existingApp == null){
                return false;
            }

            try{

                existingApp.Name = app.Name;
                existingApp.Role = app.Role;
                existingApp.SecretKey = app.SecretKey;
                existingApp.RefreshToken = app.RefreshToken;
                existingApp.Updated_At = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;

            }catch(Exception){
                return false;
            }
        }
    }
}