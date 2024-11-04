using Microsoft.EntityFrameworkCore;

namespace GoCommute;


public interface IRouteRepository {
    Task<List<Route>> GetRoutes();
    Task<Route?> GetRoute(int? id = null, string? busname = null, string? routenumber = null);
    Task AddRoute(Route route);
    Task UpdateRoute(Route route);
    Task DeleteRoute(int id);
    Task VerifyRoute(int id);
}

public class RouteRepository : IRouteRepository
{

    private readonly AppDBContext _context;

    public RouteRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task<List<Route>> GetRoutes() {
        return await _context.Routes
            .AsNoTracking()
            .Include(r => r.Markers)
            .Include(r => r.RoutePoints)
            .ToListAsync();
    }

    public async Task<Route?> GetRoute(int? id = null, string? busname = null, string? routenumber = null){
        
        if(id.HasValue){
            return await _context.Routes
            .AsNoTracking()
            .Include(r => r.Markers)
            .Include(r => r.RoutePoints)
            .FirstOrDefaultAsync(r => r.Id == id);
        }

        if(!string.IsNullOrEmpty(busname)){
            return await _context.Routes
            .AsNoTracking()
            .Include(r => r.Markers)
            .Include(r => r.RoutePoints)
            .FirstOrDefaultAsync(r => r.BusName == busname);
        }

        if(!string.IsNullOrEmpty(routenumber)){
            return await _context.Routes
            .AsNoTracking()
            .Include(r => r.Markers)
            .Include(r => r.RoutePoints)
            .FirstOrDefaultAsync(r => r.RouteNumber == routenumber);
        }
        
        return null;

    }

    public async Task AddRoute(Route route){
        await _context.Routes.AddAsync(route);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRoute(Route route){
        var existingRoute = await _context.Routes
            .Include(r => r.Markers)
            .Include(r => r.RoutePoints)
            .FirstOrDefaultAsync(r => r.Id == route.Id);

        if(existingRoute == null){
            throw new KeyNotFoundException($"Route with ID {route.Id} is not found");
        }

        existingRoute.VehicleType = route.VehicleType;
        existingRoute.BusName = route.BusName;
        existingRoute.RouteNumber = route.RouteNumber;
        existingRoute.Markers = route.Markers;
        existingRoute.RoutePoints = route.RoutePoints;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteRoute(int id){
        var route = await _context.Routes
            .Include(r => r.Markers)
            .Include(r => r.RoutePoints)
            .FirstOrDefaultAsync(r => r.Id == id);

        if(route == null){
            throw new KeyNotFoundException($"Route with ID {id} is not found");
        }

        _context.Remove(route);
        await _context.SaveChangesAsync();

    }

    public async Task VerifyRoute(int id){

        var existingRoute = await _context.Routes.FirstOrDefaultAsync(r => r.Id == id);

        if(existingRoute == null){
            throw new KeyNotFoundException($"Route with ID {id} is not found");
        }

        existingRoute.Verified = true;

        await _context.SaveChangesAsync();

    }

}
