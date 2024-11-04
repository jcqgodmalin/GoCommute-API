namespace GoCommute;

public static class RouteMapper
{

    public static RouteDto ToDto(Route route) {
        return new RouteDto {

            Id = route.Id,
            VehicleType = route.VehicleType,
            BusName = route.BusName,
            RouteNumber = route.RouteNumber,
            Verified = route.Verified,
            Markers = route.Markers,
            RoutePoints = route.RoutePoints,
            Created_By = route.Created_By,
            Updated_By = route.Updated_By,
            Created_At = route.Created_At,
            Updated_At = route.Updated_At
            
        };
    }

    public static Route ToEntity(RouteDto routeDto){
        return new Route {
            Id = routeDto.Id,
            VehicleType = routeDto.VehicleType,
            BusName = routeDto.BusName,
            RouteNumber = routeDto.RouteNumber,
            Verified = routeDto.Verified,
            Markers = routeDto.Markers,
            RoutePoints = routeDto.RoutePoints,
            Created_By = routeDto.Created_By,
            Updated_By = routeDto.Updated_By,
            Created_At = routeDto.Created_At,
            Updated_At = routeDto.Updated_At
        };
    }


}
