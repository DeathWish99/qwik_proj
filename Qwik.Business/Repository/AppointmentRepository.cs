using Microsoft.EntityFrameworkCore;

namespace Qwik.Business
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)  // DIP: Dependency injection
        {
            _context = context;
        }

        public async Task AddAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetByDateAsync(DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.AppointmentDate.Date == date.Date) 
                .OrderBy(a => a.Token)
                .ToListAsync();
        }

        public async Task<int> GetNextTokenAsync(DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.AppointmentDate.Date == date.Date)
                .CountAsync() + 1;
        }
    }
}
