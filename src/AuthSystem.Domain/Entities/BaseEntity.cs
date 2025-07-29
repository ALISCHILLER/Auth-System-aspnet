using System;

namespace AuthSystem.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }

        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;

        public void SetId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            Id = id;
        }

        public void SetCreatedAt(DateTime dateTime)
        {
            CreatedAt = dateTime;
        }

        public void SetUpdatedAt(DateTime dateTime)
        {
            UpdatedAt = dateTime;
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetIsDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsRestored()
        {
            IsDeleted = false;
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
