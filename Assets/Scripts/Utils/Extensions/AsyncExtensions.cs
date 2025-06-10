using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;

namespace Utils
{
    public static class AsyncExtensions
    {
        public static UniTask AsyncWaitForKill(this Tween tween, System.Threading.CancellationToken cancellationToken)
        {
            if (tween == null || !tween.active)
            {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(tween);
                return UniTask.CompletedTask;
            }

            var tcs = new UniTaskCompletionSource();

            CancellationTokenRegistration registration = default;
            if (cancellationToken.CanBeCanceled)
            {
                registration = cancellationToken.Register(() =>
                {
                    if (tween.IsActive())
                    {
                        tween.Kill();
                    }

                    tcs.TrySetCanceled(cancellationToken);
                });
            }

            tween.OnKill(() =>
            {
                registration.Dispose();
                tcs.TrySetResult();
            });

            return tcs.Task;
        }
    }
}