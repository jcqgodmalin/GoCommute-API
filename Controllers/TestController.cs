using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoCommute;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{

    [HttpGet("GetUnsecureTest")]
    public async Task<IActionResult> GetUnsecureTest() {
        return await Task.FromResult(Ok(new { Hello = "Hello World"}));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetSecureTest")]
    public async Task<IActionResult> GetSecureTest() {
        return await Task.FromResult(Ok(new { Hello = "This is a secure result"}));
    }

}
