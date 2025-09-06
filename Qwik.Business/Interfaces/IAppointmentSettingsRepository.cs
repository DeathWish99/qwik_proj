namespace Qwik.Business
{
    public interface IAppointmentSettingsRepository
    {
        Task<AppointmentSettings> GetAsync();
        Task UpdateAsync(AppointmentSettings settings);
    }
}
