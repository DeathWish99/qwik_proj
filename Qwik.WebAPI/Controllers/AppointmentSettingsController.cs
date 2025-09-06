using Microsoft.AspNetCore.Mvc;
using Qwik.Business;

namespace Qwik.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentSettingsController : ControllerBase
    {
        private readonly IAppointmentSettingsService _service;

        public AppointmentSettingsController(IAppointmentSettingsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<AppointmentSettings>> Get() => await _service.GetSettingsAsync();

        [HttpPut("max")]
        public async Task<ActionResult> UpdateMax(int max)
        {
            await _service.UpdateMaxAppointmentsAsync(max);
            return Ok();
        }

        [HttpPost("offday")]
        public async Task<ActionResult> AddOffDay(DateTime offDay)
        {
            await _service.AddOffDayAsync(offDay);
            return Ok();
        }
    }
}
