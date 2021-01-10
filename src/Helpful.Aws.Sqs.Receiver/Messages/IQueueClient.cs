using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public interface IQueueClient
    {
        Task<IEnumerable<ReceivedMessage>> GetNextMessagesAsync(ReceiveMessageRequest request);
    }
}