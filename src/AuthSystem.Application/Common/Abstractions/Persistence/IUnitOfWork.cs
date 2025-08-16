using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Common.Abstractions.Persistence
{
    /// <summary>
    /// واحد کاری برای مدیریت عملیات تراکنش در طول یک درخواست
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// شروع تراکنش جدید
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// تایید تغییرات انجام شده و ثبت در دیتابیس
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// بازگردانی تغییرات انجام شده در تراکنش جاری
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
