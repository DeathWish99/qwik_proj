namespace Qwik.Business
{
    public class AppointmentSettings
    {
        public Guid Id { get; set; }
        public int? MaxAppointmentsPerDay { get; set; } = 10;
        public List<DateTime> OffDays { get; set; } = new List<DateTime>();
    }
}
