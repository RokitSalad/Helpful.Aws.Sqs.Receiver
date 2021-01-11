using System.Collections.Generic;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;

namespace Helpful.Aws.Sqs.Receiver.Sqs
{
    public interface IReceivedMessageBuilder
    {
        IEnumerable<ReceivedMessage> BuildMessages(IEnumerable<Message> responseMessages);
    }
}