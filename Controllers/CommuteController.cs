using Microsoft.AspNetCore.Mvc;

namespace GoCommute;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CommuteController : ControllerBase
{

    //get suggested routes based on the user's starting point and destination point

}
