using System;
using System.Net.Sockets;
using System.Text;

public class Client
{
    private TcpClient client;
    private NetworkStream stream;

    public event Action<string> OnMessageReceived;

    public async void Connect(string ipAddress, int port)
    {
        client = new TcpClient();
        await client.ConnectAsync(ipAddress, port);
        stream = client.GetStream();
        ReadMessages();
    }

    private async void ReadMessages()
    {
        byte[] buffer = new byte[1024];
        while (true)
        {
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                OnMessageReceived?.Invoke(message);
            }
        }
    }

    public async void SendMessage(string message)
    {
        if (stream != null && stream.CanWrite)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }

    public void Disconnect()
    {
        stream?.Close();
        client?.Close();
    }
}
