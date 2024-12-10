using Hsm.Domain.Entities.Base;
using System.Numerics;

namespace Hsm.Domain.Entities.Entities
{
    public class Inventory : BaseEntity
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Threshold { get; set; }
    }
}
