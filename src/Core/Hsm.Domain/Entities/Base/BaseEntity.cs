using System.ComponentModel.DataAnnotations;

namespace Hsm.Domain.Entities.Base
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public virtual Guid Id { get; private set; }
        public virtual DateTime CreatedDateUTC { get; private set; } = DateTime.UtcNow;
        public virtual DateTime? UpdatedDateUTC { get; private set; }
        public virtual bool IsActive { get; private set; } = true;

        [Timestamp]
        public virtual byte[] RowVersion { get; set; } = null!;


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

        public void CreateId() { Id = Guid.NewGuid(); }
        public BaseEntity SetId(Guid id) { Id = id; return this; }
        public BaseEntity SetIsActive(bool isActive) { IsActive = isActive; return this; }
        public BaseEntity SetCreatedDateUTC(DateTime createdDateUTC) { CreatedDateUTC = createdDateUTC; return this; }
        public BaseEntity SetUpdatedDateUTC(DateTime updatedDateUTC) { UpdatedDateUTC = updatedDateUTC; return this; }

    }
}
