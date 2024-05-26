using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server
{
    private TcpListener listener;
    private TcpClient client;
    private NetworkStream stream;

    public event Action<string> OnMessageReceived;

    public void Start(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        AcceptClient();
    }

    private async void AcceptClient()
    {
        client = await listener.AcceptTcpClientAsync();
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

    public void Stop()
    {
        stream?.Close();
        client?.Close();
        listener?.Stop();
    }
}
