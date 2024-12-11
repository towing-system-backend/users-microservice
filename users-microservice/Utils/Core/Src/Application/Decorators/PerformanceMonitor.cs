namespace Application.Core
{
    using System.Diagnostics;
    public class PerfomanceMonitor<T, U>(IService<T, U> service, Logger logger, IPerformanceLogsRepository performanceLogsRepository, string action) : IService<T, U>
    {
        private readonly IService<T, U> _service = service;
        private readonly Logger _logger = logger;
        private readonly IPerformanceLogsRepository _performanceLogsRepository = performanceLogsRepository;
        async public Task<Result<U>> Execute(T data)
        {
            var sw = Stopwatch.StartNew();
            var result = await _service.Execute(data);
            sw.Stop();
            _logger.Log($"Execution time: {sw.ElapsedMilliseconds}ms");
            if(result.IsSuccess)
                Task.Run(() => _performanceLogsRepository.LogStats(action, sw.ElapsedMilliseconds));

            return result;
        }
    }
}