using System.ComponentModel.DataAnnotations;

namespace Hsm.Domain.Entities.Base
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDateUTC { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDateUTC { get; set; }
        public bool IsActive { get; set; } = true;


        public BaseEntity()
        {
            IsActive = false;
            CreatedDateUTC = DateTime.UtcNow;
            UpdatedDateUTC = null;
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
            IsActive = false;
            CreatedDateUTC = DateTime.UtcNow;
            UpdatedDateUTC = null;
        }

        public BaseEntity SetId(Guid id) { Id = id; return this; }
        public BaseEntity SetIsActive(bool isActive) { IsActive = isActive; return this; }
        public BaseEntity SetCreatedDateUTC(DateTime createdDateUTC) { CreatedDateUTC = createdDateUTC; return this; }
        public BaseEntity SetUpdatedDateUTC(DateTime updatedDateUTC) { UpdatedDateUTC = updatedDateUTC; return this; }

    }
}
