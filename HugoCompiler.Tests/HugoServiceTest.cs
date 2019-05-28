using System;
using System.IO;
using Xunit;

namespace HugoCompiler.Tests
{
    public class HugoServiceTest
    {

        [Fact]
        public void CompileTest()
        {
            var hugo = new HugoService();
            var result = hugo.Compile(@"C:\Users\johnn\source\repos\FilmHype.Hugo\Hugo\filmhype");

            Assert.True(!string.IsNullOrEmpty(result));
        }

    }
}
