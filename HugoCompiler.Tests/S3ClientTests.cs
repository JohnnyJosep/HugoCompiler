using System.Threading.Tasks;
using Xunit;

namespace HugoCompiler.Tests
{
    public class S3ClientTests
    {
        [Fact]
        public async Task S3GetObjectsTest()
        {
            using (var client = new S3Client("hugo-vetamuebles", "us-east-1", "AKIAYJGEZ2VNPILJYO55", "qK0w+yPJo61emWNIb/Cg1uXJXzT+lHhxMKdTJq1x"))
            {
                var objects = await client.GetObjects();

                Assert.True(objects.Count > 0);
            }
        }


        [Fact]
        public async Task S3GetObjectTest()
        {
            if (System.IO.File.Exists(@"C:\Temp\Test\config.yml"))
                System.IO.File.Delete(@"C:\Temp\Test\config.yml");

            using (var client = new S3Client("hugo-vetamuebles", "us-east-1", "AKIAYJGEZ2VNPILJYO55", "qK0w+yPJo61emWNIb/Cg1uXJXzT+lHhxMKdTJq1x"))
            {
                await client.DownloadObject("config.yml", @"C:\Temp\Test");
            }

            Assert.True(System.IO.File.Exists(@"C:\Temp\Test\config.yml"));
        }
    }
}
