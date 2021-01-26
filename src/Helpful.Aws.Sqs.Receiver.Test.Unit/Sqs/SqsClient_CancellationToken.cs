using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Sqs;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit.Sqs
{
    public class SqsClient_CancellationToken
    {
        private Mock<IAmazonSQS> _mockAmazonClient;
        private Exception _caughtException;
        private List<ReceivedMessage> _returnedMessages;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _returnedMessages = new List<ReceivedMessage>();
            var cancellationTokeSource = new CancellationTokenSource();
            _mockAmazonClient = new Mock<IAmazonSQS>();
            _mockAmazonClient.Setup(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReceiveMessageResponse
                {
                    Messages = new List<Message>
                    {
                        new Message()
                    }
                });

            var sqsQueueClient = new SqsQueueClient(_mockAmazonClient.Object, cancellationTokeSource.Token);

            try
            {
                _returnedMessages.AddRange(await sqsQueueClient.GetNextMessagesAsync(new ReceiveMessageRequest()));
                cancellationTokeSource.Cancel();
                _returnedMessages.AddRange(await sqsQueueClient.GetNextMessagesAsync(new ReceiveMessageRequest()));
            }
            catch (Exception e)
            {
                _caughtException = e;
            }
        }

        [Test]
        public void NoExceptionsAreThrown()
        {
            Assert.IsNull(_caughtException);
        }

        [Test]
        public void OnlyOneMessageIsReturned()
        {
            Assert.AreEqual(1, _returnedMessages.Count);
        }

        [Test]
        public void QueueClientIsCalledOnlyOce()
        {
            _mockAmazonClient.Verify(x => x.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
