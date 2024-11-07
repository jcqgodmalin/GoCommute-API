using GoCommute.DTOs;
using GoCommute.Helpers;
using GoCommute.Models;
using GoCommute.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoCommute.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AppsController : ControllerBase
    {

        private readonly AppService _appService;

        public AppsController(AppService appService)
        {
            _appService = appService;
        }

        [HttpGet("get-all-apps")]
        public async Task<IActionResult> GetAllApps() {
            var apps = await _appService.GetAllApps();
            return Ok(apps);
        }

        [HttpGet("get-all-apps-by-user")]
        public async Task<IActionResult> GetAllAppsByUser(int UserId){
            var apps = await _appService.GetAppsByUserId(UserId);
            return Ok(apps);
        }

        [HttpGet("get-app-by-id")]
        public async Task<IActionResult> GetAppByID(int id) {
            var app = await _appService.GetAppByID(id);
            return Ok(app);
        }

        [HttpGet("get-app-by-appid")]
        public async Task<IActionResult> GetAppByAppID(string AppID){
            var app = await _appService.GetAppByAppID(AppID);
            return Ok(app);
        }

        [HttpPost("add-app")]
        public async Task<IActionResult> AddApp([FromBody] NewAppDto newAppDto){
            
            if(newAppDto.UserId < 1){
                return BadRequest("User ID is invalid");
            }
            if(string.IsNullOrEmpty(newAppDto.Name) || newAppDto.Role.Count < 1){
                return BadRequest("Name or Role is invalid");
            }

            try{
                var app = await _appService.AddApp(newAppDto);
                return CreatedAtAction(nameof(GetAppByID), new {Id = app.Id}, app);
            }catch(Exception e){
                return StatusCode(500,e.Message);
            }
            
        }

        [HttpPut("update-app")]
        public async Task<IActionResult> UpdateApp([FromBody] AppDto appDto){
            var updatedapp = await _appService.UpdateApp(appDto);
            if(updatedapp){
                return NoContent();
            }else{
                return StatusCode(500,"Unable to update the App");
            }
        }

        [HttpDelete("delete-app")]
        public async Task<IActionResult> DeleteApp(int id){
            var isAppDeleted = await _appService.DeleteApp(id);
            if(isAppDeleted){
                return Ok($"App with ID: {id} has been deleted");
            }else{
                return StatusCode(500,"Unable to delete the App");
            }
        }

        [HttpPost("token/generate")]
        public async Task<IActionResult> GenerateToken([FromBody] AppGenerateTokenDto appGenerateTokenDto){
            try{
                var atrt = await _appService.GenerateToken(appGenerateTokenDto);
                return Ok(atrt);
            }catch(Exception e){
                return StatusCode(500,e.Message);
            }
        }

        [HttpPost("token/refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] AppRefreshTokenDto appRefreshTokenDto){
            try{
                var atrt = await _appService.RefreshToken(appRefreshTokenDto);
                return Ok(atrt);
            }catch(Exception e){
                return StatusCode(500,e.Message);
            }
        }
        
    }
}