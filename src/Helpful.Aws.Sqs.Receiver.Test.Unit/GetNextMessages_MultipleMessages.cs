using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit
{
    public class GetNextMessages_MultipleMessages
    {
        private IMessageReceiver _messageReceiver;
        private Mock<IQueueClient> _mockAwsClient;
        private List<ReceivedMessage> _messages;
        private Exception _caughtException;
        private ReceivedMessage _returnedMessage1;
        private ReceivedMessage _returnedMessage2;
        private ReceivedMessage _returnedMessage3;
        private int _unprocessedMessageCount1;
        private IEnumerable<ReceivedMessage> _unprocessedMessages1;
        private int _unprocessedMessageCount2;
        private IEnumerable<ReceivedMessage> _unprocessedMessages2;
        private int _unprocessedMessageCount3;
        private IEnumerable<ReceivedMessage> _unprocessedMessages3;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _messages = new List<ReceivedMessage>
            {
                new ReceivedMessage(),
                new ReceivedMessage(),
                new ReceivedMessage()
            };
            _mockAwsClient = new Mock<IQueueClient>();
            _mockAwsClient.Setup(x =>
                    x.GetNextMessagesAsync(It.IsAny<ReceiveMessageRequest>()))
                .ReturnsAsync(_messages);
            _messageReceiver = new MessageReceiver(new MessageReceiverConfig
            {
                QueueUrl = "some url"
            }, _mockAwsClient.Object);

            try
            {
                _unprocessedMessageCount1 = _messageReceiver.UnprocessedMessageCount;
                _unprocessedMessages1 = _messageReceiver.UnprocessedMessages.ToList();
                _returnedMessage1 = await _messageReceiver.NextMessageAsync();

                _unprocessedMessageCount2 = _messageReceiver.UnprocessedMessageCount;
                _unprocessedMessages2 = _messageReceiver.UnprocessedMessages.ToList();
                _returnedMessage2 = await _messageReceiver.NextMessageAsync();

                _unprocessedMessageCount3 = _messageReceiver.UnprocessedMessageCount;
                _unprocessedMessages3 = _messageReceiver.UnprocessedMessages.ToList();
                _returnedMessage3 = await _messageReceiver.NextMessageAsync();
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
        public void TheMessagesAreReturnedInReceivedOrder()
        {
            Assert.AreEqual(_messages.First(), _returnedMessage1);
            Assert.AreEqual(_messages.Skip(1).First(), _returnedMessage2);
            Assert.AreEqual(_messages.Last(), _returnedMessage3);
        }

        [Test]
        public void TheUnprocessedMessageCountGivesExpectedValues()
        {
            Assert.AreEqual(0, _unprocessedMessageCount1);
            Assert.AreEqual(2, _unprocessedMessageCount2);
            Assert.AreEqual(1, _unprocessedMessageCount3);
        }

        [Test]
        public void TheUnprocessedMessagesAreAvailable()
        {
            Assert.AreEqual(0, _unprocessedMessages1.Count());
            Assert.AreEqual(_messages.Skip(1), _unprocessedMessages2);
            Assert.AreEqual(_messages.Skip(2), _unprocessedMessages3);
        }
    }
}