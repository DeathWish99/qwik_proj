using Moq;
using Qwik.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwik.Tests
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepo = new();
        private readonly Mock<IAppointmentSettingsRepository> _settingsRepo = new();
        private readonly IAppointmentService _service;

        public AppointmentServiceTests()
        {
            _service = new AppointmentService(_appointmentRepo.Object, _settingsRepo.Object);
        }

        [Fact]
        public async Task BookAppointment_NormalDay_SucceedsWithToken1()
        {
            var settings = new AppointmentSettings { MaxAppointmentsPerDay = 10 };
            _settingsRepo.Setup(r => r.GetAsync()).ReturnsAsync(settings);
            _appointmentRepo.Setup(r => r.GetByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(new List<Appointment>());
            _appointmentRepo.Setup(r => r.GetNextTokenAsync(It.IsAny<DateTime>())).ReturnsAsync(1);
            _appointmentRepo.Setup(r => r.AddAsync(It.IsAny<Appointment>())).Returns(Task.CompletedTask);

            var result = await _service.BookAppointmentAsync("asdf", DateTime.Today);

            Assert.Equal(DateTime.Today.Date, result.AppointmentDate.Date);
            Assert.Equal(1, result.Token);
            _appointmentRepo.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public async Task BookAppointment_MaxReached_OverflowsToNextDay()
        {
            var settings = new AppointmentSettings { MaxAppointmentsPerDay = 1 };
            _settingsRepo.Setup(r => r.GetAsync()).ReturnsAsync(settings);
            _appointmentRepo.Setup(r => r.GetByDateAsync(DateTime.Today)).ReturnsAsync(new List<Appointment> { new Appointment() });
            _appointmentRepo.Setup(r => r.GetByDateAsync(DateTime.Today.AddDays(1))).ReturnsAsync(new List<Appointment>());
            _appointmentRepo.Setup(r => r.GetNextTokenAsync(DateTime.Today.AddDays(1))).ReturnsAsync(1);

            var result = await _service.BookAppointmentAsync("ggg", DateTime.Today);

            Assert.Equal(DateTime.Today.AddDays(1).Date, result.AppointmentDate.Date);
        }

        [Fact]
        public async Task BookAppointment_OnOffDay_SkipsToNextDay()
        {
            var settings = new AppointmentSettings { OffDays = new List<DateTime> { DateTime.Today } };
            _settingsRepo.Setup(r => r.GetAsync()).ReturnsAsync(settings);
            _appointmentRepo.Setup(r => r.GetByDateAsync(It.IsAny<DateTime>())).ReturnsAsync(new List<Appointment>());
            _appointmentRepo.Setup(r => r.GetNextTokenAsync(DateTime.Today.AddDays(1))).ReturnsAsync(1);

            var result = await _service.BookAppointmentAsync("APPOINTMENT 1", DateTime.Today);

            Assert.Equal(DateTime.Today.AddDays(1).Date, result.AppointmentDate.Date);
        }

        [Fact]
        public async Task BookAppointment_MultipleOffDaysAndMax_ChainsOverflow()
        {
            var settings = new AppointmentSettings
            {
                MaxAppointmentsPerDay = 1,
                OffDays = new List<DateTime> { DateTime.Today.AddDays(1) }
            };
            _settingsRepo.Setup(r => r.GetAsync()).ReturnsAsync(settings);
            _appointmentRepo.Setup(r => r.GetByDateAsync(DateTime.Today)).ReturnsAsync(new List<Appointment> { new() });
            _appointmentRepo.Setup(r => r.GetByDateAsync(DateTime.Today.AddDays(1))).ReturnsAsync(new List<Appointment>()); // Off day
            _appointmentRepo.Setup(r => r.GetByDateAsync(DateTime.Today.AddDays(2))).ReturnsAsync(new List<Appointment>());
            _appointmentRepo.Setup(r => r.GetNextTokenAsync(DateTime.Today.AddDays(2))).ReturnsAsync(1);

            var result = await _service.BookAppointmentAsync("TEST", DateTime.Today);

            Assert.Equal(DateTime.Today.AddDays(2).Date, result.AppointmentDate.Date);
        }

        [Fact]
        public async Task GetDailyQueueAsync_HasAppointments_ReturnsList()
        {
            var appointments = new List<Appointment> { new Appointment { Token = 1 } };
            _appointmentRepo.Setup(r => r.GetByDateAsync(DateTime.Today)).ReturnsAsync(appointments);

            var result = await _service.GetDailyQueueAsync(DateTime.Today);

            Assert.Single(result);
            Assert.Equal(1, result[0].Token);
        }

        [Fact]
        public async Task GetDailyQueueAsync_NoAppointments_ReturnsEmptyList()
        {
            _appointmentRepo.Setup(r => r.GetByDateAsync(DateTime.Today)).ReturnsAsync(new List<Appointment>());

            var result = await _service.GetDailyQueueAsync(DateTime.Today);

            Assert.Empty(result);
        }
    }
}
