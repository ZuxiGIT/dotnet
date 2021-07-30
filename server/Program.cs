using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net.Sockets;
using System.Net;

namespace web_example
{
    public class Program
    {
        public static string[] m_args;
        public static int m_port;
        public static void Main(string[] args)
        {
            m_args = args;
            CreateHostBuilder(args).Build().Run();
        }
        public static void startEchoServer()
        {
            if(m_args.Length > 1)
                throw new ArgumentException("Parameters: <Port>");

            m_port = (m_args.Length == 1) ? Int32.Parse(m_args[0]) : 43444;

            TcpListener listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), m_port);
                listener.Start();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ErrorCode + ":" + e.Message);
                Environment.Exit(e.ErrorCode);
            }

            byte[] rcvBuff = new byte[32];
            int byteRcvd;

            while(true)
            {
                TcpClient client = null;
                NetworkStream netStream = null;

                try
                {
                    client = listener.AcceptTcpClient();
                    netStream = client.GetStream();

                    Console.WriteLine("Client connected");

                    while((byteRcvd = netStream.Read(rcvBuff, 0, rcvBuff.Length)) > 0)
                    {
                        netStream.Write(rcvBuff, 0, byteRcvd);
                    }
                    
                    netStream.Close();
                    client.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    netStream.Close();
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
