namespace Qwik.Business
{
    public interface IAppointmentSettingsService
    {
        Task<AppointmentSettings> GetSettingsAsync();
        Task UpdateMaxAppointmentsAsync(int max);
        Task AddOffDayAsync(DateTime offDay);
    }
}
