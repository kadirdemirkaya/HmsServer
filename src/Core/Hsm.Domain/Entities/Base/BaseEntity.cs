using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hsm.Domain.Entities.Base
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public virtual Guid Id { get; private set; }
        public virtual DateTime CreatedDateUTC { get; private set; } = DateTime.UtcNow;
        public virtual DateTime? UpdatedDateUTC { get; private set; }
        public virtual bool IsActive { get; private set; } = true;

        [Column(TypeName = "uuid")]
        public virtual Guid RowVersion { get; private set; }


        public BaseEntity()
        {
            CreateId();
            IsActive = false;
            CreatedDateUTC = DateTime.UtcNow;
            UpdatedDateUTC = null;
            RowVersion = Guid.NewGuid();
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
            IsActive = false;
            CreatedDateUTC = DateTime.UtcNow;
            UpdatedDateUTC = null;
            RowVersion = Guid.NewGuid();
        }

        public Guid GetId() { return Id; }
        public bool GetIsActive() { return IsActive; }
        public void CreateId()
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
        }

        public BaseEntity SetRowVersion(Guid rowVersion) { RowVersion = rowVersion; return this; }
        public BaseEntity SetId(Guid id) { Id = id; return this; }
        public BaseEntity SetIsActive(bool isActive) { IsActive = isActive; return this; }
        public BaseEntity SetCreatedDateUTC(DateTime createdDateUTC) { CreatedDateUTC = createdDateUTC; return this; }
        public BaseEntity SetUpdatedDateUTC(DateTime updatedDateUTC) { UpdatedDateUTC = updatedDateUTC; return this; }

    }
}
