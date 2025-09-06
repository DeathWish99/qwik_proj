namespace Qwik.Business
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IAppointmentSettingsRepository _settingsRepo;

        public AppointmentService(IAppointmentRepository appointmentRepo, IAppointmentSettingsRepository settingsRepo)  // DIP
        {
            _appointmentRepo = appointmentRepo;
            _settingsRepo = settingsRepo;
        }

        public async Task<Appointment> BookAppointmentAsync(string customerName, DateTime requestedDate)
        {
            var settings = await _settingsRepo.GetAsync();
            var date = requestedDate;

            // Handle off days and max appointments
            while (settings.OffDays.Any(d => d.Date == date.Date) ||  // LINQ
                   (await _appointmentRepo.GetByDateAsync(date)).Count >= settings.MaxAppointmentsPerDay)
            {
                date = date.AddDays(1);  // Overflow to next day
            }

            var token = await _appointmentRepo.GetNextTokenAsync(date);
            var appointment = new Appointment { CustomerName = customerName, AppointmentDate = date, Token = token };
            await _appointmentRepo.AddAsync(appointment);
            return appointment;
        }

        public async Task<List<Appointment>> GetDailyQueueAsync(DateTime date)
        {
            return await _appointmentRepo.GetByDateAsync(date);
        }
    }
}
