using GoCommute.DTOs;
using GoCommute.Helpers;
using GoCommute.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoCommute.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("generate-token")]
    public async Task<IActionResult> GenerateToken([FromBody] UserDto userDto){

        //get user based on the app id and client secret
        if(userDto.AppID == null && userDto.SecretKey == null){
            return BadRequest("AppID and SecretKey cannot be empty");
        }

        var user = await _userService.GetUser(null,null,userDto.AppID,userDto.SecretKey);

        if(user == null){
            return NotFound("Client with the provided AppID and SecretKey cannot be found");
        }

        userDto.Role = user.Role;

        //if client exists, generate token
        Console.WriteLine($"UserDto AppID: {userDto.AppID}");
        Console.WriteLine($"UserDto Role: {userDto.Role}");
        var token = SecurityHelper.GenerateToken(userDto);

        return Ok(new {token = token});
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] UserDto userDto){

        if(String.IsNullOrEmpty(userDto.Email) || String.IsNullOrEmpty(userDto.Password)) {
            return BadRequest();
        }

        //validate if email exist
        var userFromDB = await _userService.GetUser(null,userDto.Email);
        if(userFromDB != null){
            return BadRequest("Email already exists");
        }
        
        //assign role as User
        userDto.Role = "User";

        //password hashing
        userDto.Password = SecurityHelper.PasswordHash(userDto.Password);

        //generate AppID
        userDto.AppID = SecurityHelper.GenerateAppID();

        //generate SecretKey
        userDto.SecretKey = SecurityHelper.GenerateSecretKey();

        //assign created at
        userDto.Created_At = DateTime.Now;

        //Create User
        var createdUser = await _userService.AddUser(userDto);

        return CreatedAtAction(nameof(GetUser),new {id = createdUser.Id}, createdUser);

    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUser(int id){
        var user = await _userService.GetUser(id,null);
        if(user == null){
            return NotFound();
        }

        return Ok(user);
    }
}
