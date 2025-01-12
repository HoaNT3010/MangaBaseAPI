using Hangfire;
using MangaBaseAPI.CrossCuttingConcerns.BackgroundJob.HangfireScheduler;
using System.Linq.Expressions;

namespace MangaBaseAPI.Infrastructure.BackgroundJob.HangfireScheduler
{
    public class HangfireBackgroundJobService : IHangfireBackgroundJobService
    {
        readonly IBackgroundJobClientV2 _jobClient;
        readonly IRecurringJobManagerV2 _jobManager;

        public HangfireBackgroundJobService(
            IBackgroundJobClientV2 jobClient,
            IRecurringJobManagerV2 jobManager)
        {
            _jobClient = jobClient;
            _jobManager = jobManager;
        }

        public void AddOrUpdateRecurringJob(
            string recurringJobId,
            Expression<Func<Task>> methodCall,
            string cronExpression)
        {
            _jobManager.AddOrUpdate(recurringJobId, methodCall, cronExpression);
        }

        public string Enqueue(Expression<Func<Task>> methodCall)
        {
            return _jobClient.Enqueue(methodCall);
        }

        public string Enqueue<T>(Expression<Action<T>> methodCall)
        {
            return _jobClient.Enqueue<T>(methodCall);
        }

        public void RemoveRecurringJob(string recurringJobId)
        {
            _jobManager.RemoveIfExists(recurringJobId);
        }

        public string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay)
        {
            return _jobClient.Schedule(methodCall, delay);
        }

        public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay)
        {
            return _jobClient.Schedule<T>(methodCall, delay);
        }
    }
}
