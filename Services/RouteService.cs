namespace GoCommute;

public class RouteService
{

    private readonly IRouteRepository _routeRepository;

    public RouteService(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;
    }

    public async Task<List<RouteDto>> GetRoutes(){
        var routes = await _routeRepository.GetRoutes();
        return routes.Select(RouteMapper.ToDto).ToList();
    }

    public async Task<RouteDto?> GetRoute(int? id = null, string? busName = null, string? routeNumber = null){

        var route = await _routeRepository.GetRoute(id, busName, routeNumber);
        if(route != null){
            return RouteMapper.ToDto(route);
        }
        return null;
        
    }

    public async Task AddRoute(RouteDto routeDto) {
        await _routeRepository.AddRoute(RouteMapper.ToEntity(routeDto));
    }

    public async Task UpdateRoute(RouteDto routeDto){
        await _routeRepository.UpdateRoute(RouteMapper.ToEntity(routeDto));
    }

    public async Task DeleteRoute(int id){
        await _routeRepository.DeleteRoute(id);
    }

    public async Task VerifyRoute(int id){
        await _routeRepository.VerifyRoute(id);
    }

}
