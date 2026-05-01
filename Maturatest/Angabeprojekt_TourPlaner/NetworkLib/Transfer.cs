using System.Net.Sockets;
using System.Xml.Serialization;

namespace NetworkLib;

public class Transfer
{
    private TcpClient client;
    private Receiver receiver;
    private NetworkStream stream;
    private StreamReader reader;
    private StreamWriter writer;
    private XmlSerializer xml = new XmlSerializer(typeof(MSG));
    private bool running = true;
    private Thread? t;

    public Transfer(TcpClient c, Receiver r)
    {
        client = c;
        receiver = r;
        stream = client.GetStream();
        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);
    }

    public string? GetIP()
    {
        if (client.Connected)
            return client.Client.RemoteEndPoint?.ToString();
        return null;
    }

    public void Start()
    {
        t = new Thread(Run);
        t.IsBackground = true;
        t.Start();
    }

    public void Stop()
    {
        running = false;
        try { reader.Close(); } catch { }
        try { writer.Close(); } catch { }
        try { stream.Close(); } catch { }
        try { client.Close(); } catch { }
    }

    private void Run()
    {
        string msg = "";
        while (running && client.Connected)
        {
            try
            {
                string? line = reader.ReadLine();
                if (line == null) break;
                if (line.Length > 0)
                {
                    msg += line + "\n";
                    if (msg.Contains("</MSG>"))
                    {
                        StringReader sr = new StringReader(msg);
                        MSG? m = (MSG?)xml.Deserialize(sr);
                        if (m != null)
                            receiver.ReceiveMessage(m, this);
                        msg = "";
                    }
                }
            }
            catch { break; }
        }
        receiver.TransferDisconnected(this);
        Stop();
    }

    public void Send(MSG m)
    {
        try
        {
            StringWriter sw = new StringWriter();
            xml.Serialize(sw, m);
            string msg = sw.ToString();
            writer.WriteLine(msg);
            writer.Flush();
        }
        catch { }
    }
}
