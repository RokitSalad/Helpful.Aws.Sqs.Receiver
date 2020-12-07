using System;
using System.Threading;
using System.Threading.Tasks;
using Helpful.Aws.Sqs.Receiver.Exceptions;
using Helpful.Aws.Sqs.Receiver.Messages;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit
{
    public class GetNextMessages_Exception
    {
        const string exceptionMessage = "Some exception";

        private Mock<ISqsClient> _mockAwsClient;
        private Exception _caughtException;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _mockAwsClient = new Mock<ISqsClient>();
            _mockAwsClient.Setup(x =>
                    x.GetNextMessagesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception(exceptionMessage));
            var messageReceiver = new MessageReceiver(_mockAwsClient.Object);

            try
            {
                await messageReceiver.NextMessageAsync(CancellationToken.None);
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