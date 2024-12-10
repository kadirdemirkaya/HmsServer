using Hsm.Domain.Entities.Base;

namespace Hsm.Domain.Entities.Entities
{
    public class BedManagement : BaseEntity
    {
        public string Room { get; set; }
        public bool Status { get; set; }

        public Guid AssignedPatientId { get; set; }
        public Patient AssignedPatient { get; set; }
    }
}
