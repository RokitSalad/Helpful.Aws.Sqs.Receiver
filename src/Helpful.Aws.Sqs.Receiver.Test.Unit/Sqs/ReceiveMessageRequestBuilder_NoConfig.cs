using System;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Exceptions;
using Helpful.Aws.Sqs.Receiver.Messages;
using Helpful.Aws.Sqs.Receiver.Sqs;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit.Sqs
{
    public class ReceiveMessageRequestBuilder_NoConfig
    {
        private Exception _caughtException;
        private ReceiveMessageRequest _builtRequest;

        [OneTimeSetUp]
        public void Setup()
        {
            var builder = new ReceiveMessageRequestBuilder();

            try
            {
                _builtRequest = builder.Build(new MessageReceiverConfig());
            }
            catch (Exception e)
            {
                _caughtException = e;
            }
        }

        [Test]
        public void AnExceptionIsThrown()
        {
            Assert.NotNull(_caughtException);
        }

        [Test]
        public void NothingGetsBuild()
        {
            Assert.IsNull(_builtRequest);
        }

        [Test]
        public void TheMissingQueueUrlIsTheProblem()
        {
            Assert.IsInstanceOf<MissingQueueUrlException>(_caughtException);
        }
    }
}
