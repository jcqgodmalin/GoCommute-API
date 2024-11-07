using GoCommute.DTOs;
using GoCommute.Helpers;

namespace GoCommute.Services
{
    public class CommuteService
    {

        private readonly RouteService _routeService;

        public CommuteService(RouteService routeService)
        {
            _routeService = routeService;
        }

        public async Task<List<RouteDto>> CommuteRecommendation(CommuterDto commuterDto){
            var recommendedRoutes = new List<RouteDto>();
            var routes = await _routeService.GetRoutes();

            if(routes.Count < 1){
                throw new Exception("There is no available routes in the database");
            }
            
            //loop through all the routes
            foreach(var route in routes){

                bool thisRoutePassesTheCommuterStartingPoint = false;
                bool thisRoutePassesTheCommuterDestinationPoint = false;

                //loop through all the route's routepoints
                foreach(var routepoint in route.RoutePoints){

                    //calculate the distance of the current route point to the starting point
                    var sp_distance = HaversineCalc.CalculateDistance(commuterDto.StartingLat,commuterDto.StartingLon,routepoint.Latitude,routepoint.Longitude);

                    if(sp_distance <= 2){
                        thisRoutePassesTheCommuterStartingPoint = true;
                    }

                    var ep_distance = HaversineCalc.CalculateDistance(commuterDto.DestinationLat,commuterDto.DestinationLon,routepoint.Latitude,routepoint.Longitude);

                    if(ep_distance <= 2){
                        thisRoutePassesTheCommuterDestinationPoint = true;
                    }
                }

                if(thisRoutePassesTheCommuterStartingPoint && thisRoutePassesTheCommuterDestinationPoint) {
                    recommendedRoutes.Add(route);
                }

            }

            return recommendedRoutes;
        }
    }
}