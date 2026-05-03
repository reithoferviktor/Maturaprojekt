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

    public Transfer(TcpClient c, Receiver r)
    {
        client = c; receiver = r;
        stream = client.GetStream();
        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);
    }

    public void Start()
    {
        var t = new Thread(Run) { IsBackground = true };
        t.Start();
    }

    public void Stop()
    {
        running = false;
        try { reader.Close(); writer.Close(); client.Close(); } catch { }
    }

    private void Run()
    {
        string buf = "";
        while (running && client.Connected)
        {
            try
            {
                string? line = reader.ReadLine();
                if (line == null) break;
                buf += line + "\n";
                if (buf.Contains("</MSG>"))
                {
                    MSG? m = (MSG?)xml.Deserialize(new StringReader(buf));
                    if (m != null) receiver.ReceiveMessage(m, this);
                    buf = "";
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
            writer.WriteLine(sw.ToString());
            writer.Flush();
        }
        catch { }
    }
}
