namespace Qwik.Business
{
    public interface IAppointmentService
    {
        Task<Appointment> BookAppointmentAsync(string customerName, DateTime requestedDate);
        Task<List<Appointment>> GetDailyQueueAsync(DateTime date);
    }
}
