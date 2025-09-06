using Microsoft.EntityFrameworkCore;

namespace Qwik.Business
{
    public class AppointmentSettingsRepository : IAppointmentSettingsRepository
    {
        private readonly AppDbContext _context;

        public AppointmentSettingsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppointmentSettings> GetAsync()
        {
            var settings = await _context.AppointmentSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new AppointmentSettings();
                _context.AppointmentSettings.Add(settings);
                await _context.SaveChangesAsync();
            }
            return settings;
        }

        public async Task UpdateAsync(AppointmentSettings settings)
        {
            _context.AppointmentSettings.Update(settings);
            await _context.SaveChangesAsync();
        }
    }
}
