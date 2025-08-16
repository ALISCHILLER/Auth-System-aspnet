using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Persistence;
using AuthSystem.Application.Common.Primitives;
using MediatR;

namespace AuthSystem.Application.Common.Behaviors
{
    /// <summary>
    /// رفتار مدیریت تراکنش برای Commandهایی که نیازمند اجرای اتمیک هستند.
    /// </summary>
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ITransactionalCommand<TResponse>
        where TResponse : Result
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var response = await next();

                if (response.IsSuccess)
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);
                else
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                return response;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
