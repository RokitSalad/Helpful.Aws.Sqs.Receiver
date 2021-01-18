using Amazon;
using Amazon.Runtime;
using Amazon.SQS;

namespace Helpful.Aws.Sqs.Receiver.Test.Integration.AWS
{
    internal static class SqsClient
    {
        public static AmazonSQSClient GetAmazonSqsClient()
        {
            AWSCredentials creds = new BasicAWSCredentials(AwsKeys.AccessKeyId, AwsKeys.SecretKey);
            var sqsClient = new AmazonSQSClient(creds, RegionEndpoint.APSoutheast2);
            return sqsClient;
        }
    }
}
