using System.Linq.Expressions;

namespace MangaBaseAPI.CrossCuttingConcerns.BackgroundJob.HangfireScheduler
{
    public interface IHangfireBackgroundJobService
    {
        string Enqueue(Expression<Func<Task>> methodCall);
        string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay);
        void AddOrUpdateRecurringJob(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression);
        void RemoveRecurringJob(string recurringJobId);
        string Enqueue<T>(Expression<Action<T>> methodCall);
        string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);
    }
}
