namespace AuthSystem.Domain.Common.Specifications;

/// <summary>
/// مشخصات ترکیبی
/// </summary>
public abstract class CompositeSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// مشخصات سمت چپ
    /// </summary>
    public ISpecification<T> Left { get; }

    /// <summary>
    /// مشخصات سمت راست
    /// </summary>
    public ISpecification<T> Right { get; }

    /// <summary>
    /// سازنده با دو مشخصات
    /// </summary>
    protected CompositeSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Left = left;
        Right = right;
    }
}