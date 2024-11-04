using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GoCommute;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password){

            if(username != "admin" || password != "admin"){
                return Unauthorized();
            }

            var TokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourSuperSecureAndLongEnoughKey_1234567890!@#$");
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = TokenHandler.CreateToken(tokenDescriptor);
            var tokenString = TokenHandler.WriteToken(token);

            return await Task.FromResult(Ok( new { Token = tokenString } ));
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] string email){
            if(String.IsNullOrEmpty(email)) {
                return BadRequest();
            }

            //validate if email exist

            //generate AppID

            //generate SecretKey

            //Create User

            return await Task.FromResult(Ok( email ));

        }

}
