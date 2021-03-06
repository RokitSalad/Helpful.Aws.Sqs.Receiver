using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Exceptions;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit
{
    public class GetNextMessages_Exception
    {
        const string exceptionMessage = "Some exception";

        private Mock<IQueueClient> _mockAwsClient;
        private Exception _caughtException;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _mockAwsClient = new Mock<IQueueClient>();
            _mockAwsClient.Setup(x =>
                    x.GetNextMessagesAsync(It.IsAny<ReceiveMessageRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));
            var messageReceiver = new MessageReceiver(new MessageReceiverConfig
            {
                QueueUrl = "some url"
            }, _mockAwsClient.Object);

            try
            {
                await messageReceiver.NextMessageAsync();
            }
            catch (Exception e)
            {
                _caughtException = e;
            }
        }

        [Test]
        public void AnExceptionIsThrown()
        {
            Assert.IsNotNull(_caughtException);
        }

        [Test]
        public void TheExceptionIsASqsMessageReceiveException()
        {
            Assert.IsInstanceOf<SqsMessageReceiveException>(_caughtException);
        }

        [Test]
        public void TheSourceExceptionIsWrapped()
        {
            Assert.AreEqual(exceptionMessage, _caughtException?.InnerException?.Message);
        }
    }
}