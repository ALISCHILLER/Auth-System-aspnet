using System.Linq.Expressions;

namespace AuthSystem.Domain.Common.Abstractions;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
}
