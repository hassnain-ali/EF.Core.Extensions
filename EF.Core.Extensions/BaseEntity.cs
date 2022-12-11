namespace EF.Core.Extensions;

public abstract record BaseEntity
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    [Key]
    public virtual Guid Id { get; set; }
}
