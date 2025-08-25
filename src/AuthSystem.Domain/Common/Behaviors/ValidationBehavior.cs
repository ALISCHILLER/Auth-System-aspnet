using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results; // این خط را اضافه کنید
using MediatR;
using AuthSystem.Domain.Common.Exceptions;

namespace AuthSystem.Domain.Common.Behaviors;

/// <summary>
/// رفتار اعتبارسنجی برای پیپ‌لاین MediatR
/// این رفتار قبل از اجرای درخواست، اعتبارسنجی‌های لازم را انجام می‌دهد
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// سازنده با اعتبارسنجی‌ها
    /// </summary>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// اجرای اعتبارسنجی‌ها قبل از اجرای درخواست اصلی
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var failures = new List<ValidationFailure>();

        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
                failures.AddRange(result.Errors);
        }

        if (failures.Any())
            throw new ValidationException(failures);

        return await next();
    }
}