using System;
using System.Collections.Generic;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Exceptions;
using Helpful.Aws.Sqs.Receiver.Messages;
using Helpful.Aws.Sqs.Receiver.Sqs;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit.Sqs
{
    public class ReceiveMessageRequestBuilder_WithConfig
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
                AttributeNames = new List<string> {"attr name 1", "attr name 2"},
                QueueUrl = "some url",
                MaxNumberOfMessages = 10,
                MessageAttributeNames = new List<string> {"msg attr 1"},
                VisibilityTimeout = 10,
                WaitTimeSeconds = 2
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
        public void ThePropertiesAreTransferredToTheRequest()
        {
            Assert.AreEqual(_config.QueueUrl, _builtRequest.QueueUrl);
            Assert.AreEqual(_config.MaxNumberOfMessages, _builtRequest.MaxNumberOfMessages);
            Assert.AreEqual(_config.VisibilityTimeout, _builtRequest.VisibilityTimeout);
            Assert.AreEqual(_config.WaitTimeSeconds, _builtRequest.WaitTimeSeconds);
            Assert.AreEqual(_config.AttributeNames, _builtRequest.AttributeNames);
            Assert.AreEqual(_config.MessageAttributeNames, _builtRequest.MessageAttributeNames);
        }
    }
}
