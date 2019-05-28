using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HugoCompiler
{
    public class Function
    {
        private static async Task Main(string[] args)
        {
            Func<string, ILambdaContext, string> func = FunctionHandler;
            using(var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new JsonSerializer()))
            using(var bootstrap = new LambdaBootstrap(handlerWrapper))
            {
                await bootstrap.RunAsync();
            }
        }

        private static IEnvironmentVariables _environment;
        public static IEnvironmentVariables Environment
        {
            get
            {
                if (_environment == null)
                {
                    _environment = new EnvironmentVariables();
                }
                return _environment;
            }
            set { _environment = value; }
        }


        private static string ContentFolder = $@"{Directory.GetCurrentDirectory()}\hugoContent";
        
        public static string FunctionHandler(string input, ILambdaContext context)
        {
            var contentBucket = Environment.Get("contentBucket");
            var publicBucket = Environment.Get("publicBucket");
            var region = Environment.Get("region");
            var apiKey = Environment.Get("apiKey");
            var apiSecret = Environment.Get("apiSecret");


            if (Directory.Exists(ContentFolder))
            {
                Directory.Delete(ContentFolder, true);
            }
            Directory.CreateDirectory(ContentFolder);

            using (var s3 = new S3Client(contentBucket, region, apiKey, apiSecret))
            {
                var files = s3.GetObjects().Result;
                foreach (var file in files)
                {
                    if (!file.Key.EndsWith("/"))
                    {
                        s3.DownloadObject(file.Key, ContentFolder).Wait();
                    }
                    else
                    {
                        Directory.CreateDirectory($@"{ContentFolder}\{file.Key}");
                    }
                }
            }

            var hugo = new HugoService();
            var compile = hugo.Compile(ContentFolder);

            using (var s3 = new S3Client(publicBucket, region, apiKey, apiSecret))
            {
                s3.UploadObjects($@"{ContentFolder}\public").Wait();
            }

            //TODO: falta comprobar si hay que borrar archivos del s3 de destino

            return compile;
        }

    }
}
