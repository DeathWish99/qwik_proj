namespace Qwik.Business
{
    public interface IAppointmentRepository
    {
        Task AddAsync(Appointment appointment);
        Task<List<Appointment>> GetByDateAsync(DateTime date);
        Task<int> GetNextTokenAsync(DateTime date);
    }
}
