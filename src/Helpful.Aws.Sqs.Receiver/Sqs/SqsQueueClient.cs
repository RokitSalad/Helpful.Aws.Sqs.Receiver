using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Helpful.Aws.Sqs.Receiver.Messages;

namespace Helpful.Aws.Sqs.Receiver.Sqs
{
    public class SqsQueueClient : IQueueClient
    {
        public async Task<IEnumerable<SqsMessage>> GetNextMessagesAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
