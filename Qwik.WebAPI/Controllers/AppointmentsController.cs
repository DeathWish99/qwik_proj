using Microsoft.AspNetCore.Mvc;
using Qwik.Business;

namespace Qwik.WebAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)  // DI
        {
            _service = service;
        }

        [HttpPost("book")]
        public async Task<ActionResult<Appointment>> Book([FromBody] Request request)
        {
            var appointment = await _service.BookAppointmentAsync(request.CustomerName, request.RequestedDate);
            return Ok(appointment);
        }

        [HttpGet("queue/{date}")]
        public async Task<ActionResult<List<Appointment>>> GetQueue(DateTime date)
        {
            return await _service.GetDailyQueueAsync(date);
        }
    }
}
