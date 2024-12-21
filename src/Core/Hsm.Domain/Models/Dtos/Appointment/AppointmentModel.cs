namespace Hsm.Domain.Models.Dtos.Appointment
{
    public class AppointmentModel
    {
        public Guid Id { get; set; }
        public DateTime AppointmentTime { get; set; }
        public byte[] RowVersion { get; set; } = null!;
        public bool IsActive { get; set; }

        public Guid UserId { get; set; }
    }
}
