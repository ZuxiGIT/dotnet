using System;
using System.Net.Sockets;
using System.Text;


namespace client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string server = "127.0.0.1";
            
            Console.WriteLine("Enter server port");

            int server_port = Int32.Parse(Console.ReadLine());

            TcpClient client = null;
            NetworkStream netStream = null;

            Console.WriteLine("Connecting to {0}:{1}", server, server_port);
            try
            {
                client = new TcpClient(server, server_port);
                netStream = client.GetStream();

                Console.WriteLine("Connected to {0}:{1}", server, server_port);
                Console.WriteLine("Enter the stirng");
                
                string message = null;
                byte[] bytes_message = new byte[32];
                
                while((message = Console.ReadLine()) != "q")
                {
                    bytes_message = Encoding.ASCII.GetBytes(message, 0, 32);
                    netStream.Write(bytes_message, 0, bytes_message.Length);

                    if(netStream.Read(bytes_message, 0, bytes_message.Length) > 0)
                    {
                        Console.WriteLine("Server Answer:{0}", Encoding.ASCII.GetString(bytes_message));
                    }
                }
                

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                netStream.Close();
                client.Close();
            }
        }

    }
}
