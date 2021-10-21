using Microsoft.AspNetCore.Mvc;
using GeneratedTypes;

namespace SampleApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class PartsController : ControllerBase
    {

        [HttpPost]
        public IActionResult CreatePart([FromBody] PartDto part)
        {
            //TODO: Add the part 
            return Ok("Part added");
        }
    }
}
