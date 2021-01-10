using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;

namespace Helpful.Aws.Sqs.Receiver.Sqs
{
    public interface IReceiveMessageRequestBuilder
    {
        ReceiveMessageRequest Build(MessageReceiverConfig config);
    }
}