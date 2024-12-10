using Hsm.Domain.Entities.Base;

namespace Hsm.Domain.Entities.Entities
{
    public class ElectronicHealthRecord : BaseEntity
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public string Notes { get; set; }
    }
}
