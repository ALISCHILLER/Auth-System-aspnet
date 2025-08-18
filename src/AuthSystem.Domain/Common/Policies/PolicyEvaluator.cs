// File: AuthSystem.Domain/Common/Policies/PolicyEvaluator.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Policies
{
    /// <summary>
    /// ارزیاب سیاست‌ها (Policy Evaluator)
    /// - چند Policy را اجرا و نتیجهٔ تجمیعی بازمی‌گرداند
    /// - پشتیبانی از گزارش چندین خطا در صورت نقض
    /// </summary>
    public static class PolicyEvaluator
    {
        /// <summary>
        /// اجرای مجموعهٔ Policy همگام و ادغام نتایج (AND منطقی)
        /// - گزارش تمام خطاها در صورت نقض
        /// </summary>
        public static PolicyResult EvaluateAll<TContext>(
            TContext ctx,
            IEnumerable<IPolicy<TContext>> policies)
        {
            if (policies == null)
                throw new ArgumentNullException(nameof(policies));

            var errors = new List<PolicyError>();
            foreach (var p in policies)
            {
                var r = p.Evaluate(ctx);
                if (!r.IsAllowed)
                    errors.AddRange(r.Errors);
            }

            return errors.Count > 0
                ? PolicyResult.Deny(errors.ToArray())
                : PolicyResult.Allow();
        }

        /// <summary>
        /// اجرای مجموعهٔ Policy ناهمگام و ادغام نتایج (AND منطقی)
        /// - گزارش تمام خطاها در صورت نقض
        /// </summary>
        public static async Task<PolicyResult> EvaluateAllAsync<TContext>(
            TContext ctx,
            IEnumerable<IAsyncPolicy<TContext>> policies,
            CancellationToken cancellationToken = default)
        {
            if (policies == null)
                throw new ArgumentNullException(nameof(policies));

            var errors = new List<PolicyError>();
            foreach (var p in policies)
            {
                var r = await p.EvaluateAsync(ctx, cancellationToken).ConfigureAwait(false);
                if (!r.IsAllowed)
                    errors.AddRange(r.Errors);
            }

            return errors.Count > 0
                ? PolicyResult.Deny(errors.ToArray())
                : PolicyResult.Allow();
        }
    }
}