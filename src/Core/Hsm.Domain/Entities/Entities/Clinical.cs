using Hsm.Domain.Entities.Base;
using System.Xml.Linq;

namespace Hsm.Domain.Entities.Entities
{
    public class Clinical : BaseEntity
    {
        public string Name { get; private set; }

        public Guid HospitalId { get; private set; }
        public Hospital Hospital { get; private set; }

        public Clinical()
        {

        }
        public Clinical(string name)
        {

        }

        public Clinical(Guid id, string name)
        {

        }

        public Clinical(Guid id, string name, Guid hospitalId)
        {

        }

        public static Clinical Create(string name)
            => new(name);

        public static Clinical Create(Guid id, string name)
            => new(id, name);
        public static Clinical Create(Guid id, string name, Guid hospitalId)
          => new(id, name, hospitalId);

        public Clinical SetName(string name) { Name = name; return this; }

        public Clinical SetHopspitalId(Guid hospitalId) { HospitalId = hospitalId; return this; }
    }
}
