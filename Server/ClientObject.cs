using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ClientObject
    {
        private TcpClient client;

        public ClientObject(TcpClient client)
        {
            this.client = client;
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64];
                while (true)
                {
                    StringBuilder messageBuilder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        messageBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);
                    string message = messageBuilder.ToString();
                    Console.WriteLine(message);
                    message = message.Substring(message.IndexOf(':') + 1).Trim().ToUpper();
                    data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (client != null)
                {
                    client.Close();
                }
            }
        }
    }
}
