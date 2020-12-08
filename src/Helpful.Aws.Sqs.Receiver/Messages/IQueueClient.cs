using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public interface IQueueClient
    {
        Task<IEnumerable<SqsMessage>> GetNextMessagesAsync();
    }
}