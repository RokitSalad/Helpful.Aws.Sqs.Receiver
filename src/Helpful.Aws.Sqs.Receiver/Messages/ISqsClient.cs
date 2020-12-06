using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public interface ISqsClient
    {
        Task<IEnumerable<SqsMessage>> GetNextMessagesAsync(CancellationToken cancellationToken);
    }
}