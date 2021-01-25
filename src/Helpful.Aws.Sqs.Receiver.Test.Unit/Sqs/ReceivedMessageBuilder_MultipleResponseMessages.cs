using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;
using Helpful.Aws.Sqs.Receiver.Sqs;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit.Sqs
{
    public class ReceivedMessageBuilder_MultipleResponseMessages
    {
        private Exception _caughtException;
        private List<Message> _messages;
        private readonly List<ReceivedMessage> _receivedMessages = new List<ReceivedMessage>();

        [OneTimeSetUp]
        public void Setup()
        {
            IReceivedMessageBuilder builder = new ReceivedMessageBuilder();
            _messages = new List<Message>
            {
                new Message
                {
                    Attributes = new Dictionary<string, string>(),
                    Body = "Some body",
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>(),
                    MessageId = "message id",
                    ReceiptHandle = "receipt handle"
                },
                new Message
                {
                    Attributes = new Dictionary<string, string>(),
                    Body = "Some other body",
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>(),
                    MessageId = "another message id",
                    ReceiptHandle = "another receipt handle"
                }
            };

            try
            {
                _receivedMessages.AddRange(builder.BuildMessages(_messages, new Mock<IAmazonSQS>().Object, "request url"));
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
        public void BothMessagesAreReturned()
        {
            Assert.AreEqual(2, _receivedMessages.Count());
        }

        [Test]
        public void TheMessagesAreBuiltCorrectly()
        {
            Assert.AreEqual(_messages[0].Body, _receivedMessages[0].Body);
            Assert.AreEqual(_messages[1].Body, _receivedMessages[1].Body);
        }
    }
}
