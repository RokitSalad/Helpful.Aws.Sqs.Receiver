using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public class MessageReceiver : IMessageReceiver
    {
        private readonly ISqsClient _sqsClient;

        private readonly Queue<SqsMessage> _receivedCache;

        public MessageReceiver(ISqsClient sqsClient)
        {
            _sqsClient = sqsClient;
            _receivedCache = new Queue<SqsMessage>();
        }

        public async Task<SqsMessage> NextMessageAsync(CancellationToken cancellationToken)
        {
            if (!_receivedCache.Any())
            {
                IEnumerable<SqsMessage> messages = await _sqsClient.GetNextMessagesAsync(cancellationToken);
                foreach (var message in messages)
                {
                    _receivedCache.Enqueue(message);
                }
            }

            return _receivedCache.Any() ? _receivedCache.Dequeue() : null;
        }
    }
}