namespace EF.Core.Extensions;

public interface IBaseEntity<T> where T : IEquatable<T>
{
    [Key]
    T Id { get; set; }
}