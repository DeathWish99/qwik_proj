using Moq;
using Qwik.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qwik.Tests
{
    public class AppointmentSettingServiceTests
    {
        private readonly Mock<IAppointmentSettingsRepository> _repo = new();
        private readonly IAppointmentSettingsService _service;

        public AppointmentSettingServiceTests()
        {
            _service = new AppointmentSettingsService(_repo.Object);
        }

        [Fact]
        public async Task GetSettingsAsync_ReturnsDefaultSettings()
        {
            // Arrange
            var settings = new AppointmentSettings();
            _repo.Setup(r => r.GetAsync()).ReturnsAsync(settings);

            var result = await _service.GetSettingsAsync();

            // Assert
            Assert.Equal(10, result.MaxAppointmentsPerDay);
            Assert.Empty(result.OffDays);
        }

        [Fact]
        public async Task UpdateMaxAppointmentsAsync_UpdatesAndSaves()
        {
            var settings = new AppointmentSettings();
            _repo.Setup(r => r.GetAsync()).ReturnsAsync(settings);
            _repo.Setup(r => r.UpdateAsync(It.IsAny<AppointmentSettings>())).Returns(Task.CompletedTask);

            await _service.UpdateMaxAppointmentsAsync(20);

            Assert.Equal(20, settings.MaxAppointmentsPerDay);
            _repo.Verify(r => r.UpdateAsync(settings), Times.Once);
        }

        [Fact]
        public async Task AddOffDayAsync_AddsNewOffDayAndSaves()
        {
            var settings = new AppointmentSettings();
            _repo.Setup(r => r.GetAsync()).ReturnsAsync(settings);
            _repo.Setup(r => r.UpdateAsync(It.IsAny<AppointmentSettings>())).Returns(Task.CompletedTask);

            await _service.AddOffDayAsync(DateTime.Today);

            Assert.Single(settings.OffDays);
            Assert.Equal(DateTime.Today.Date, settings.OffDays[0].Date);
            _repo.Verify(r => r.UpdateAsync(settings), Times.Once);
        }

        [Fact]
        public async Task AddOffDayAsync_DuplicateOffDay_DoesNotAdd()
        {
            var settings = new AppointmentSettings { OffDays = new List<DateTime> { DateTime.Today } };
            _repo.Setup(r => r.GetAsync()).ReturnsAsync(settings);
            _repo.Setup(r => r.UpdateAsync(It.IsAny<AppointmentSettings>())).Returns(Task.CompletedTask);

            await _service.AddOffDayAsync(DateTime.Today);

            Assert.Single(settings.OffDays);
            _repo.Verify(r => r.UpdateAsync(It.IsAny<AppointmentSettings>()), Times.Never);
        }
    }
}
