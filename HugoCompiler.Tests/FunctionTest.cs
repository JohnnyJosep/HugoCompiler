using Amazon.Lambda.TestUtilities;
using Moq;
using Xunit;

namespace HugoCompiler.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void FunctionHandlerTest()
        {
            var environmentVariablesMock = new Mock<IEnvironmentVariables>();
            environmentVariablesMock.Setup(ev => ev.Get("contentBucket")).Returns("hugo-vetamuebles");
            environmentVariablesMock.Setup(ev => ev.Get("publicBucket")).Returns("hugo-compiled-vetamuebles");
            environmentVariablesMock.Setup(ev => ev.Get("region")).Returns("us-east-1");
            environmentVariablesMock.Setup(ev => ev.Get("apiKey")).Returns("AKIAYJGEZ2VNPILJYO55");
            environmentVariablesMock.Setup(ev => ev.Get("apiSecret")).Returns("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");

            // Invoke the lambda function and confirm the string was upper cased.
            var context = new TestLambdaContext();
            Function.Environment = environmentVariablesMock.Object;
            var given = Function.FunctionHandler("hello world", context);

            Assert.True(!string.IsNullOrEmpty(given));
        }
    }
}
