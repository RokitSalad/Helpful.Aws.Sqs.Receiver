using System.Threading.Tasks;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public interface IMessageReceiver
    {
        Task<ReceivedMessage> NextMessageAsync();
    }
}