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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto ){

        if(String.IsNullOrEmpty(userLoginDto.Email) || String.IsNullOrEmpty(userLoginDto.Password)) {
            return BadRequest();
        }

        //try to get the user
        var userFromDB = await _userService.GetUser(null, userLoginDto.Email);

        if(userFromDB == null){
            return NotFound($"User with Email {userLoginDto.Email} was not found");
        }

        //if user is found, compare password
        if(SecurityHelper.PasswordVerify(userLoginDto.Password,userFromDB.Password)){
            return Ok();
        }else{
            return BadRequest($"Password is incorrect");
        }

    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] UserSignupDto userSignupDto){

        if(String.IsNullOrEmpty(userSignupDto.Email) || String.IsNullOrEmpty(userSignupDto.Password)) {
            return BadRequest();
        }

        //validate if email exist
        var userFromDB = await _userService.GetUser(null,userSignupDto.Email);
        if(userFromDB != null){
            return BadRequest("Email already exists");
        }

        //if user is not found. Create a new user and set the appropriate values

        var user = new UserDto();

        //created now
        user.Created_At = DateTime.Now;

        //email
        user.Email = userSignupDto.Email;

        //password
        user.Password = SecurityHelper.PasswordHash(userSignupDto.Password);

        //set the initial role to User
        user.Role.Add("User");

        //save the user
        var userCreated = await _userService.AddUser(user);

        return CreatedAtAction(nameof(GetUser), new {Id = userCreated.Id}, userCreated);

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
