
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Collections;

namespace ProductManagement.Functions
{
    public class QueueFunction
    {
        private readonly ILogger<QueueFunction> _logger;

        public QueueFunction(ILogger<QueueFunction> logger)
        {
            _logger = logger; //This allows Azure Functions to inject a logger into our class so we can print messages in the terminal.
        }

        [Function("QueueFunction")]
        public void Run(
            [QueueTrigger("product-queue", Connection = "AzureWebJobsStorage")] string message)
        {
            _logger.LogInformation("Queue message received: {Message}", message);
        }
    }
}
//QueueTrigger	Activates the function when a queue message arrives.
//product - queue   Name of the queue being monitored.
//Connection	Tells it which storage connection to use.
//message	Receives the content of the queue message.
