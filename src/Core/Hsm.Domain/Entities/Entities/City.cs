using Hsm.Domain.Entities.Base;

namespace Hsm.Domain.Entities.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Hospital> Hospitals { get; set; } = new List<Hospital>();

        public City()
        {

        }

        public City(string name)
        {
            CreateId();
            Name = name;
        }

        public City(Guid id, string name) : base(id)
        {
            SetId(id);
            Name = name;
        }

        public static City Create(string name)
            => new(name);

        public static City Create(Guid id, string name)
         => new(id, name);


    }
}
