using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Sqs;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit.Sqs
{
    public class ReceivedMessageBuilder_CheckAllProps
    {
        private Exception _caughtException;
        private readonly List<ReceivedMessage> _receivedMessages = new List<ReceivedMessage>();
        private List<Message> _messages;
        private Mock<IAmazonSQS> _mockAmazonSqs;
        private const string QUEUE_URL = "request url";


        [OneTimeSetUp]
        public async Task Setup()
        {
            _mockAmazonSqs = new Mock<IAmazonSQS>();
            IReceivedMessageBuilder builder = new ReceivedMessageBuilder();
            _messages = new List<Message>
            {
                new Message
                {
                    Attributes = new Dictionary<string, string>
                    {
                        {"key1", "val1"},
                        {"key2", "val2"}
                    },
                    Body = "Some body",
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        {"key1", new MessageAttributeValue
                        {
                            StringValue = "val1"
                        }}
                    },
                    MessageId = "message id",
                    ReceiptHandle = "receipt handle"
                }
            };

            try
            {
                _receivedMessages.AddRange(builder.BuildMessages(_messages, _mockAmazonSqs.Object, QUEUE_URL));
                await _receivedMessages.First().RemoveFromQueueAsync();
            }
            catch (Exception e)
            {
                _caughtException = e;
            }
        }

        [Test]
        public void PopulatesTheMessageBody()
        {
            Assert.AreEqual(_messages.First().Body, _receivedMessages.First().Body);
        }

        [Test]
        public void PopulatesTheMessageId()
        {
            Assert.AreEqual(_messages.First().MessageId, _receivedMessages.First().MessageId);
        }

        [Test]
        public void PopulatesTheAttributes()
        {
            Assert.AreEqual(_messages.First().Attributes, _receivedMessages.First().Attributes);
        }

        [Test]
        public void PopulatesTheMessageAttributes()
        {
            Assert.AreEqual(_messages.First().MessageAttributes, _receivedMessages.First().MessageAttributes);
        }

        [Test]
        public void DeleteFromQueueIsSentToAws()
        {
            _mockAmazonSqs.Verify(x => x.DeleteMessageAsync(QUEUE_URL, _messages.First().ReceiptHandle, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
