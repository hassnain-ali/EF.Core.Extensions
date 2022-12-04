using System.ComponentModel.DataAnnotations;

namespace EF.Core.Extensions;

#if NET6_0_OR_GREATER

public abstract record BaseEntity
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    [Key]
    public virtual Guid Id { get; set; }
}

#endif