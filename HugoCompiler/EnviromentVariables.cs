using System;

namespace HugoCompiler
{
    public class EnvironmentVariables : IEnvironmentVariables
    {
        public string Get(string variableName) => Environment.GetEnvironmentVariable(variableName);
    }
}
