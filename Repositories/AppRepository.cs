using System.Linq.Expressions;
using GoCommute.Data;
using GoCommute.Models;
using Microsoft.EntityFrameworkCore;

namespace GoCommute.Repositories
{

    public interface IAppRepository {
        Task<List<App>> GetAllApps();
        Task<List<App>> GetAllAppsByUserId(int id);
        Task<App?> GetAppByUserId(int UserId, string appName);
        Task<App?> GetAppByID(int id);
        Task<App?> GetAppByAppID(string AppID);
        IQueryable<App>? GetApp(Expression<Func<App,bool>> filter);
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

        public async Task<List<App>> GetAllAppsByUserId(int id)
        {
            if(id < 1){
                return new List<App>();
            }

            try{
                return await _context.Apps
                    .AsNoTracking()
                    .Where(a => a.UserId == id)
                    .ToListAsync();
            }catch(Exception){
                return new List<App>();
            }
        }

        public async Task<App?> GetAppByUserId(int UserId, string appName){
            if(UserId < 1 || string.IsNullOrEmpty(appName)){
                return null;
            }

            try{
                var app = await _context.Apps
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.UserId == UserId && a.Name == appName);
                return app;
            }catch(Exception){
                return null;
            }
        }

        public async Task<App?> GetAppByAppID(string AppID)
        {
            if(string.IsNullOrEmpty(AppID)){
                return null;
            }
            
            try{
                var app = await _context.Apps
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.AppID == AppID);
                return app;
            }catch(Exception){
                return null;
            }
        }

        public async Task<App?> GetAppByID(int id)
        {
            if(id < 1){
                return null;
            }
            
            var app = await _context.Apps
                .FindAsync(id);

            return app;
        }

        public IQueryable<App>? GetApp(Expression<Func<App,bool>> filter){
            try{
                var app = _context.Apps.Where(filter);
                return app;
            }catch(Exception){
                return null;
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