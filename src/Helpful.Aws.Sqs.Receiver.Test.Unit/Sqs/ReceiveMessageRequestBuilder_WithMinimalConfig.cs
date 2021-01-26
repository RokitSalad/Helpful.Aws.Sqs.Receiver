using System;
using System.Linq;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Sqs;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit.Sqs
{
    public class ReceiveMessageRequestBuilder_WithMinimalConfig
    {
        private Exception _caughtException;
        private ReceiveMessageRequest _builtRequest;
        private MessageReceiverConfig _config;

        [OneTimeSetUp]
        public void Setup()
        {
            var builder = new ReceiveMessageRequestBuilder();
            _config = new MessageReceiverConfig
            {
                QueueUrl = "some url"
            };

            try
            {
                _builtRequest = builder.Build(_config);
            }
            catch (Exception e)
            {
                _caughtException = e;
            }
        }

        [Test]
        public void NoExceptionIsThrown()
        {
            Assert.IsNull(_caughtException);
        }

        [Test]
        public void SomethingGetsBuild()
        {
            Assert.IsNotNull(_builtRequest);
        }

        [Test]
        public void AllAttributeNamesAreRequested()
        {
            Assert.AreEqual("All", _builtRequest.AttributeNames.First());
            Assert.AreEqual(1, _builtRequest.AttributeNames.Count);
        }

        [Test]
        public void AllMessageAttributesAreRequested()
        {
            Assert.AreEqual("All", _builtRequest.MessageAttributeNames.First());
            Assert.AreEqual(1, _builtRequest.MessageAttributeNames.Count);
        }

        [Test]
        public void MaxNumberOfMessagesSetToOne()
        {
            Assert.AreEqual(1, _builtRequest.MaxNumberOfMessages);
        }

        [Test]
        public void LongPollingIsEnabled()
        {
            Assert.AreEqual(10, _builtRequest.WaitTimeSeconds);
        }
    }
}
