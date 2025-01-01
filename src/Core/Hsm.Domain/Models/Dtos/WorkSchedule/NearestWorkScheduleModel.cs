namespace Hsm.Domain.Models.Dtos.WorkSchedule
{
    public class NearestWorkScheduleModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid RowVersion { get; set; }
        public bool IsActive { get; set; }
    }
}
