using BaseProject;
using Grpc.Core;
using Helloworld;
using Helloworld.Helloworld;
using System;

namespace GrpcClient
{
    class ProgramClient
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Make sure the server is ready...");
            Console.ReadKey();
            Console.WriteLine("");

            string pemRootCerts;
            string certificateChain;
            string privateKey;

            pemRootCerts = Constants.PemIntermediateCert1;
            certificateChain = Constants.PemClientCert2;
            privateKey = Constants.PemClientKey2;

            /*
            SslCredentials creds = new SslCredentials(pemRootCerts, new KeyCertificatePair(certificateChain, privateKey));
            Channel channel = new Channel("localhost:50051", Constants.IsServerSecure ? creds : ChannelCredentials.Insecure);

            var client = new Greeter.GreeterClient(channel);
            String user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("Greeting: " + reply.Message);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            */

            DoOneShotClientRpc(true, Constants.PemIntermediateCert1, Constants.PemClientKey2, Constants.PemClientCert2);
            DoOneShotClientRpc(true, Constants.PemIntermediateCert1, Constants.PemClientKey2, Constants.PemClientCert2);
            DoOneShotClientRpc(true, Constants.PemIntermediateCert1, Constants.PemClientKey2, Constants.PemClientCert2);

            //fail1 - server side
            try
            {
                DoOneShotClientRpc(false, Constants.PemIntermediateCert1, Constants.PemClientKey1, Constants.PemClientCert1);
            }
            catch (RpcException)
            {
                Console.WriteLine("fail1 - server side");
            }

            DoOneShotClientRpc(true, Constants.PemIntermediateCert1, Constants.PemClientKey2, Constants.PemClientCert2);

            SslCredentials creds = new SslCredentials(Constants.PemIntermediateCert1, new KeyCertificatePair(Constants.PemClientCert2, Constants.PemClientKey2));

            string messageChannelA = "channelA: " + CreateMessage(true, Constants.PemIntermediateCert1, Constants.PemClientKey2, Constants.PemClientCert2);
            Channel channelA = new Channel("localhost:50051", creds);
            var clientA = new Greeter.GreeterClient(channelA);
            PerformRpc(clientA, messageChannelA);

            string messageChannelB = "channelB: " + CreateMessage(true, Constants.PemIntermediateCert1, Constants.PemClientKey2, Constants.PemClientCert2);
            Channel channelB = new Channel("localhost:50051", creds);
            var clientB = new Greeter.GreeterClient(channelB);
            PerformRpc(clientB, messageChannelB);

            Console.WriteLine("sleeping... please change certs before continuing");
            Console.ReadKey();

            DoOneShotClientRpc(true, Constants.PemIntermediateCertBoth, Constants.PemClientKey1, Constants.PemClientCert1);
            DoOneShotClientRpc(true, Constants.PemIntermediateCertBoth, Constants.PemClientKey2, Constants.PemClientCert2);

            //fail2 - client side
            try
            {
                DoOneShotClientRpc(false, Constants.PemIntermediateCert2, Constants.PemClientKey1, Constants.PemClientCert1);
            }
            catch (RpcException)
            {
                Console.WriteLine("fail2 - client side");
            }

            DoOneShotClientRpc(true, Constants.PemIntermediateCertBoth, Constants.PemClientKey1, Constants.PemClientCert1);

            PerformRpc(clientA, messageChannelA);
            PerformRpc(clientB, messageChannelB);

            Console.WriteLine("sleeping... please change certs before continuing");
            Console.ReadKey();

            DoOneShotClientRpc(true, Constants.PemIntermediateCertBoth, Constants.PemClientKey1, Constants.PemClientCert1);
            DoOneShotClientRpc(true, Constants.PemIntermediateCert2, Constants.PemClientKey1, Constants.PemClientCert1);

            //fail3 - client side
            try
            {
                DoOneShotClientRpc(false, Constants.PemIntermediateCert1, Constants.PemClientKey1, Constants.PemClientCert1);
            }
            catch (RpcException)
            {
                Console.WriteLine("fail3 - client side");
            }

            DoOneShotClientRpc(true, Constants.PemIntermediateCertBoth, Constants.PemClientKey1, Constants.PemClientCert1);
            DoOneShotClientRpc(true, Constants.PemIntermediateCert2, Constants.PemClientKey1, Constants.PemClientCert1);

            PerformRpc(clientA, messageChannelA);
            PerformRpc(clientB, messageChannelB);

            Console.WriteLine("sleeping... please change certs before continuing");
            Console.ReadKey();

            DoOneShotClientRpc(true, Constants.PemIntermediateCert2, Constants.PemClientKey1, Constants.PemClientCert1);

            //fali 4 - server side
            try
            {
                DoOneShotClientRpc(false, Constants.PemIntermediateCert2, Constants.PemClientKey2, Constants.PemClientCert2);
            }
            catch (RpcException)
            {
                Console.WriteLine("fail4 - server side");
            }

            DoOneShotClientRpc(true, Constants.PemIntermediateCert2, Constants.PemClientKey1, Constants.PemClientCert1);

            PerformRpc(clientA, messageChannelA);
            PerformRpc(clientB, messageChannelB);

            channelA.ShutdownAsync().Wait();
            channelB.ShutdownAsync().Wait();

            Console.WriteLine("reached the end of main()");
            Console.ReadKey();
        }

        public static void DoOneShotClientRpc(bool expectSuccess, string pemRootCerts, string privateKey, string certificateChain)
        {
            string message = CreateMessage(expectSuccess, pemRootCerts, privateKey, certificateChain);
            SslCredentials creds = new SslCredentials(pemRootCerts, new KeyCertificatePair(certificateChain, privateKey));
            Channel channel = new Channel("localhost:50051", creds);
            var client = new Greeter.GreeterClient(channel);
            PerformRpc(client, message);
            channel.ShutdownAsync().Wait();
        }

        public static void PerformRpc(Greeter.GreeterClient client, string message)
        {
            var reply = client.SayHello(new HelloRequest { Name = message }, new CallOptions().WithDeadline(DateTime.UtcNow.AddSeconds(1)));
            Console.WriteLine("Greeter client received: " + reply.Message);
        }

        public static string CreateMessage(bool expectSuccess, string pemRootCerts, string privateKey, string certificateChain)
        {
            string message = "";
            if (!expectSuccess)
            {
                message += "UNEXPECTED SUCCESS: ";
            }
            message += "[";

            if (pemRootCerts == Constants.PemIntermediateCert1)
            {
                message += "CA_1, ";
            }
            else if (pemRootCerts == Constants.PemIntermediateCert2)
            {
                message += "CA_2, ";
            }
            else if (pemRootCerts == Constants.PemIntermediateCertBoth)
            {
                message += "CA_BOTH, ";
            }
            else
            {
                message += "unknown, ";
            }

            if (privateKey == Constants.PemClientKey1)
            {

                message += "CLIENT_KEY_1, ";
            }
            else if (privateKey == Constants.PemClientKey2)
            {
                message += "CLIENT_KEY_2, ";
            }
            else
            {
                message += "unknown, ";
            }


            if (certificateChain == Constants.PemClientCert1)
            {
                message += "CLIENT_CERT_1";
            }
            else if(certificateChain == Constants.PemClientCert2)
            {
                message += "CLIENT_CERT_2";
            }
            else
            {
                message += "unknown";
            }

            return message + "]";
        }
    }
}
