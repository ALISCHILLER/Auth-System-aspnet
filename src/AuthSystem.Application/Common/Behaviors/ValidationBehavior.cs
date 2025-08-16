using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Primitives;
using FluentValidation;
using MediatR;

namespace AuthSystem.Application.Common.Behaviors
{
    /// <summary>
    /// رفتار اعتبارسنجی ورودی‌ها با FluentValidation قبل از اجرای Handler.
    /// در صورت وجود خطا، اجرای Handler متوقف و Failure (Result یا Result&lt;T&gt;) تولید می‌شود.
    /// </summary>
    /// <typeparam name="TRequest">نوع درخواست</typeparam>
    /// <typeparam name="TResponse">نوع پاسخ (Result یا Result&lt;T&gt;)</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators
                .Select(v => v.ValidateAsync(context, cancellationToken)));

            var errors = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .Select(f => Error.Validation(f?.PropertyName ?? string.Empty, f?.ErrorMessage ?? string.Empty))
                .ToList();

            if (errors.Any())
                return CreateValidationResult(errors);

            return await next();
        }

        /// <summary>
        /// ساخت نمونهٔ Failure از Result یا Result&lt;T&gt; به کمک Reflection
        /// </summary>
        private static TResponse CreateValidationResult(IReadOnlyCollection<Error> errors)
        {
            // حالت Result
            if (typeof(TResponse) == typeof(Result))
                return (Result.Failure(errors) as TResponse)!;

            // حالت Result<T>
            var genericArgs = typeof(TResponse).GetGenericArguments();
            if (genericArgs.Length != 1)
                throw new InvalidOperationException($"نوع پاسخ {typeof(TResponse).FullName} باید Result یا Result<T> باشد.");

            var resultGenericType = typeof(Result<>).MakeGenericType(genericArgs[0]);
            var failureMethod = resultGenericType
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m => m.Name == "Failure" && m.GetParameters().Length == 1);

            if (failureMethod is null)
                throw new InvalidOperationException($"متد Failure در {resultGenericType.FullName} یافت نشد.");

            var result = failureMethod.Invoke(null, new object[] { errors });
            return (TResponse)result!;
        }
    }
}
