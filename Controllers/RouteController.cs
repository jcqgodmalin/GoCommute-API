using GoCommute.DTOs;
using GoCommute.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoCommute.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class RouteController : ControllerBase
{
    private readonly RouteService _routeService;

    public RouteController(RouteService routeService)
    {
        _routeService = routeService;
    }
    //get all routes
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetRoutes() {
        var routes = await _routeService.GetRoutes();
        return Ok(routes);
    }

    //get routebyid
    [Authorize]
    [HttpGet("GetRoute")]
    public async Task<IActionResult> GetRoute(int? id, string? busname = null, string? routenumber = null){

        if((id.HasValue ? 1 : 0) + (!string.IsNullOrEmpty(busname) ? 1 : 0) + (!string.IsNullOrEmpty(routenumber) ? 1 : 0) != 1){
            return BadRequest("Please provide exactly one parameter: id, busname, or routenumber");
        }

        var route = await _routeService.GetRoute(id, busname, routenumber);

        if(route == null){
            return NotFound("Route not found");
        }

        return Ok(route);


    }

    //get routebyroutenumber

    //get routebybusname

    //add route
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddRoute([FromBody] RouteDto routeDto){

        if(routeDto == null){
            return BadRequest();
        }

        await _routeService.AddRoute(routeDto);
        return Ok(CreatedAtAction(nameof(GetRoute), new { Id = routeDto.Id }, routeDto));

    }

    //update route

    //delete route

    //verify route
}
