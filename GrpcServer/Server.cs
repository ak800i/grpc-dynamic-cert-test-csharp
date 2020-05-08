using BaseProject;
using Grpc.Core;
using Helloworld;
using Helloworld.Helloworld;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcServer
{
    class GreeterImpl : Greeter.GreeterBase
    {
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }
    }

    class ProgramServer
    {
        const int Port = 50051;

        static ServerCertificateConfig ServerCertificateConfig { get; set; } = null;

        public static void Main(string[] args)
        {
            string pemRootCerts;
            string certificateChain;
            string privateKey;

            pemRootCerts = Constants.PemIntermediateCert2;
            certificateChain = Constants.PemServerCert1;
            privateKey = Constants.PemServerKey1;

            var keyCertificatePair = new KeyCertificatePair(certificateChain, privateKey);
            var keyCertificatePairs = new List<KeyCertificatePair> { keyCertificatePair };

            SslServerCredentials sslCreds;
            if (Constants.UseConfigFetcher)
            {
                ServerCertificateConfig = new ServerCertificateConfig(keyCertificatePairs, pemRootCerts);
                ServerCertificateConfigCallback serverCertificateConfigCallback = new ServerCertificateConfigCallback(MyServerCertificateConfigCallback);
                sslCreds = new SslServerCredentials(
                    ServerCertificateConfig,
                    SslClientCertificateRequestType.RequestAndRequireAndVerify,
                    serverCertificateConfigCallback);
            }
            else
            {
                sslCreds = new SslServerCredentials(
                    keyCertificatePairs,
                    pemRootCerts,
                    SslClientCertificateRequestType.RequestAndRequireAndVerify);
            }

            Server server = new Server
            {
                Services = { Greeter.BindService(new GreeterImpl()) },
                Ports = { new ServerPort("localhost", Port, Constants.IsServerSecure ? sslCreds : ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to change root certs to both...");
            Console.ReadKey();
            pemRootCerts = Constants.PemIntermediateCertBoth;
            ServerCertificateConfig = new ServerCertificateConfig(keyCertificatePairs, pemRootCerts);
            Console.WriteLine("certs changed.");
            Console.WriteLine("Press any key to change signature certs to 2...");
            Console.ReadKey();
            certificateChain = Constants.PemServerCert2;
            privateKey = Constants.PemServerKey2;
            keyCertificatePair = new KeyCertificatePair(certificateChain, privateKey);
            keyCertificatePairs = new List<KeyCertificatePair> { keyCertificatePair };
            ServerCertificateConfig = new ServerCertificateConfig(keyCertificatePairs, pemRootCerts);
            Console.WriteLine("certs changed.");
            Console.WriteLine("Press any key to change root certs to 1...");
            Console.ReadKey();
            pemRootCerts = Constants.PemIntermediateCert1;
            ServerCertificateConfig = new ServerCertificateConfig(keyCertificatePairs, pemRootCerts);
            Console.WriteLine("certs changed.");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }

        public static ServerCertificateConfig MyServerCertificateConfigCallback()
        {
            return ServerCertificateConfig;
        }
    }
}
