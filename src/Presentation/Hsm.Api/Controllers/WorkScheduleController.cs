using Microsoft.AspNetCore.Mvc;

namespace Hsm.Api.Controllers
{
    public class WorkScheduleController : BaseController
    {

        [HttpPost]
        [Route("create-workschedule")]
        public async Task<IActionResult> CreateWorkSchedule()
        {
            return Ok();
        }
    }
}
