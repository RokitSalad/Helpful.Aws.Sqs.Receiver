using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Sqs;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit.Sqs
{
    public class ReceivedMessageBuilder_NullResponseMessages
    {
        private Exception _caughtException;
        private readonly List<ReceivedMessage> _receivedMessages = new List<ReceivedMessage>();

        [OneTimeSetUp]
        public void Setup()
        {
            IReceivedMessageBuilder builder = new ReceivedMessageBuilder();

            try
            {
                _receivedMessages.AddRange(builder.BuildMessages(null, new Mock<IAmazonSQS>().Object, "request url"));
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
        public void NoMessagesAreReturned()
        {
            Assert.AreEqual(0, _receivedMessages.Count());
        }
    }
}
