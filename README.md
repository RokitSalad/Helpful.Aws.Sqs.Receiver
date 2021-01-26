# Helpful.Aws.Sqs.Receiver

DotNet Standard project for receiving messages from AWS SQS.

## About

This is a simple package which allows a consumer to receive messages from SQS with a DotNet application. The default behaviours are already pretty decent, but if you want to override them then you have all the control which you would do using the AWS SDK directly.

## Usage

The first thing you need as a method for authenticating with AWS. The [ReceiverFactory](/src/Helpful.Aws.Sqs.Receiver/ReceiverFactory.cs) provides several different methods which should make it pretty flexible, probably the easiest way to get started is to create an IAM User and provide the access key and secret key, along with the region, the URL of the queue you want to receive messages from, and a cancellation token.

```csharp
IMessageReceiver messageReceiver =
                ReceiverFactory.GetReceiver(AwsKeys.AccessKeyId, AwsKeys.SecretKey, "ap-southeast-2", _testQueueUrl, CancellationToken.None);
```

Once you have a message receiver configured (as above) then you can just ask it for the next message on the queue.

```csharp
ReceivedMessage message = await messageReceiver.NextMessageAsync();
```

The body of the original SQS message is returned in the [ReceivedMessage](/src/Helpful.Aws.Sqs.Receiver/Messages/ReceivedMessage.cs) as a string. There is also an async method available on the message for you to use to delete the originating SQS message upon successful processing.

```csharp
ReceivedMessage message = await messageReceiver.NextMessageAsync();
// do some processing
await message.RemoveFromQueueAsync();
```

If something goes wrong during message processing, and you with to try the message again, simply avoid removing the message from the queue. The message will be retried again after the VisibilityTimeout has elapsed.

Based on the configuration of the message receiver, the message will either come directly from SQS or from the local cache (when receiving multiple messages at once). See below for more details.

## Configuarion and Defaults

Configuration options for the receiver can be found in [MessageReceiverConfig](/src/Helpful.Aws.Sqs.Receiver/Messages/MessageReceiverConfig.cs).

```csharp
public class MessageReceiverConfig
{
    public int? MaxNumberOfMessages { get; set; }
    public string QueueUrl { get; set; }
    public int? VisibilityTimeout { get; set; }
    public int? WaitTimeSeconds { get; set; }
    public List<string> AttributeNames { get; set; }
    public List<string> MessageAttributeNames { get; set; }
}
```

**MaxNumberOfMessages** - as per the AWS SDK, you can opt to receive up to 10 messages at once. Setting this value above 1 will not guarantee you receiving that many. You can ignore this value and it will default to 1. 

If you elect to receive more than one message at a time, the message receiver will queue the messages internally, so your receiving code will not change. The entire queue is available as a property on the receiver as an IEnumerable, so if there are any fatal exceptions, received messages can be logged.

**QueueUrl** - this is a required property. The full url of the queue you want to receive from.

**VisibilityTimeout** - when a message is in-flight (having been received by the receiver) it is still on the queue in SQS but it is not visible or available to any calling application. If you don't set this value, the default is 30 seconds.

**WaitTimeSeconds** - setting WaitTimeSeconds to any value other than zero will enable long polling, which is generally a much faster option for receiving messages than the alternative. Leaving this value blank will enable the default of 30 seconds, which is the longest watit time possible.

**AttributeNames** / **MessageAttributeNames** - the default for these collections is to return all attributes and message attributes. If you want to configure specific attributes to be returned and exclude others, that can be set here.

[![Build Status](https://dev.azure.com/pete0159/Helpful.Libraries/_apis/build/status/RokitSalad.Helpful.Aws.Sqs.Receiver?branchName=main)](https://dev.azure.com/pete0159/Helpful.Libraries/_build/latest?definitionId=12&branchName=main)