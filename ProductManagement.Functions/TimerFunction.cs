using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProductManagement.Functions
{
    public class TimerFunction
    {
        private readonly ILogger<TimerFunction> _logger;

        public TimerFunction(ILogger<TimerFunction> logger)
        {
            _logger = logger;
        }

        [Function("TimerFunction")]
        public void Run(
            [TimerTrigger("0 */1 * * * *")] TimerInfo timer)
        {
            _logger.LogInformation(
                "Timer Function executed at: " + DateTime.Now);
        }
    }
}