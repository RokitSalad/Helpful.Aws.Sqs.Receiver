using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public class MessageReceiver : IMessageReceiver
    {
        private readonly ISqsClient _sqsClient;

        public MessageReceiver(ISqsClient sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task<SqsMessage> NextMessageAsync(CancellationToken cancellationToken)
        {
            var response = await _sqsClient.GetNextMessagesAsync(cancellationToken);
            return response.FirstOrDefault();
        }
    }
}