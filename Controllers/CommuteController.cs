using GoCommute.DTOs;
using GoCommute.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoCommute.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CommuteController : ControllerBase
{

    private readonly CommuteService _commuteService;

    public CommuteController(CommuteService commuteService)
    {
        _commuteService = commuteService;
    }

    //get suggested routes based on the user's starting point and destination point
    [Authorize(Roles = "Commute")]
    [HttpGet]
    public async Task<IActionResult> RecommendRoutes(CommuterDto commuterDto){
        if(commuterDto.StartingLat < 1 && commuterDto.StartingLon < 1 && commuterDto.DestinationLat < 1 && commuterDto.DestinationLon < 1){
            return BadRequest();
        }

        try{
            var recommendedRoutes = await _commuteService.CommuteRecommendation(commuterDto);
            return Ok(recommendedRoutes);
        }catch(Exception e){
            return StatusCode(500,e.Message);
        }
    }
}
