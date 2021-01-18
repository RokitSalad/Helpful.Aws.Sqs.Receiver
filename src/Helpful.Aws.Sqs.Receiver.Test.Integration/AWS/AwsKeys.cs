using System;

namespace Helpful.Aws.Sqs.Receiver.Test.Integration.AWS
{
    internal static class AwsKeys
    {
        public static string AccessKeyId => Environment.GetEnvironmentVariable("PETE_AUTOMATION_AWS_ACCESS_KEY_ID");
        public static string SecretKey => Environment.GetEnvironmentVariable("PETE_AUTOMATION_AWS_SECRET_KEY");
    }
}
