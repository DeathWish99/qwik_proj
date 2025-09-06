namespace Qwik.Business
{
    public class AppointmentSettingsService : IAppointmentSettingsService
    {
        private readonly IAppointmentSettingsRepository _repo;

        public AppointmentSettingsService(IAppointmentSettingsRepository repo)
        {
            _repo = repo;
        }

        public async Task<AppointmentSettings> GetSettingsAsync() => await _repo.GetAsync();

        public async Task UpdateMaxAppointmentsAsync(int max)
        {
            var settings = await _repo.GetAsync();
            settings.MaxAppointmentsPerDay = max;
            await _repo.UpdateAsync(settings);
        }

        public async Task AddOffDayAsync(DateTime offDay)
        {
            var settings = await _repo.GetAsync();
            if (!settings.OffDays.Any(d => d.Date == offDay.Date))  // LINQ
            {
                settings.OffDays.Add(offDay);
                await _repo.UpdateAsync(settings);
            }
        }
    }
}
