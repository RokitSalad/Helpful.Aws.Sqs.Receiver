using Amazon.SQS.Model;

namespace Helpful.Aws.Sqs.Receiver.Sqs
{
    public interface IReceiveMessageRequestBuilder
    {
        ReceiveMessageRequest Build(MessageReceiverConfig config);
    }
}