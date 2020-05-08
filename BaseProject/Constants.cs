using System.IO;

namespace BaseProject
{
    public class Constants
    {
        //public static X509Certificate2 RootCert1 { get; set; } = new X509Certificate2(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_1\certs\ca.cert.pem");
        //public static X509Certificate2 ServerCert1 { get; set; } = new X509Certificate2(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_1\intermediate\certs\localhost-1.cert.pem");

        // certificate_hierarchy (1 and 2)
        public static string PemRootCertBoth { get; }
        public static string PemIntermediateCertBoth { get; }

        // certificate_hierarchy_1
        public static string PemRootCert1 { get; }
        public static string PemIntermediateCert1 { get; }
        public static string PemServerCert1 { get; }
        public static string PemClientCert1 { get; }
        public static string PemServerKey1 { get; }
        public static string PemClientKey1 { get; }

        // certificate_hierarchy_2
        public static string PemRootCert2 { get; }
        public static string PemIntermediateCert2 { get; }
        public static string PemServerCert2 { get; }
        public static string PemClientCert2 { get; }
        public static string PemServerKey2 { get; }
        public static string PemClientKey2 { get; }

        // my configs
        public static bool IsServerSecure { get; } = true;
        public static bool UseConfigFetcher { get; } = true;

        // my certs
        public static string PemMyRoot { get; }
        public static string PemMyKey { get; }

        static Constants()
        {
            // certificate_hierarchy (1 and 2)
            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\both_ca_root_certs.pem"))
                PemRootCertBoth = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\both_intermediate_certs.pem"))
                PemIntermediateCertBoth = reader.ReadToEnd();

            // certificate_hierarchy_1
            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_1\certs\ca.cert.pem"))
                PemRootCert1 = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_1\intermediate\certs\intermediate.cert.pem"))
                PemIntermediateCert1 = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_1\intermediate\certs\localhost-1.cert.pem"))
                PemServerCert1 = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_1\intermediate\certs\client.cert.pem"))
                PemClientCert1 = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_1\intermediate\private\localhost-1.key.pem"))
                PemServerKey1 = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_1\intermediate\private\client.key.pem"))
                PemClientKey1 = reader.ReadToEnd();

            // certificate_hierarchy_2
            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_2\certs\ca.cert.pem"))
                PemRootCert2 = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_2\intermediate\certs\intermediate.cert.pem"))
                PemIntermediateCert2 = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_2\intermediate\certs\localhost-1.cert.pem"))
                PemServerCert2 = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_2\intermediate\certs\client.cert.pem"))
                PemClientCert2 = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_2\intermediate\private\localhost-1.key.pem"))
                PemServerKey2 = reader.ReadToEnd();

            using (var reader = File.OpenText(@"D:\source\TestGrpc\Base\credentials\certificate_hierarchy_2\intermediate\private\client.key.pem"))
                PemClientKey2 = reader.ReadToEnd();

            // my certs
            using (var reader = File.OpenText(@"C:\Users\Aleksa\Desktop\GeneratedCerts\certificate.pem"))
                PemMyRoot = reader.ReadToEnd();

            using (var reader = File.OpenText(@"C:\Users\Aleksa\Desktop\GeneratedCerts\key.pem"))
                PemMyKey = reader.ReadToEnd();
        }
    }
}
