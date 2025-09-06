namespace Qwik.Business
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public string? CustomerName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int? Token { get; set; }  // Sequential per day
    }
}
