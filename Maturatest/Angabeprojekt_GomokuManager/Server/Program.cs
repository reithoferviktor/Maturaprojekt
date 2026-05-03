using System.Net;
using System.Net.Sockets;
using NetworkLib;

// Einfacher Relay-Server: leitet MSG von Client 1 an Client 2 weiter und umgekehrt.
const int Port = 5050;

var clients = new List<Transfer>();
var listener = new TcpListener(IPAddress.Any, Port);
listener.Start();
Console.WriteLine($"Gomoku-Server auf Port {Port}");

while (true)
{
    var tcp = listener.AcceptTcpClient();
    var relay = new ServerRelay(tcp, clients);
    clients.Add(relay.Transfer);
    relay.Transfer.Start();
    Console.WriteLine($"Client {clients.Count} verbunden");
}

class ServerRelay : Receiver
{
    public Transfer Transfer { get; }
    private List<Transfer> all;

    public ServerRelay(TcpClient c, List<Transfer> all)
    {
        this.all = all;
        Transfer = new Transfer(c, this);
    }

    public void ReceiveMessage(MSG m, Transfer t)
    {
        foreach (var other in all)
            if (other != t) other.Send(m);
    }

    public void TransferDisconnected(Transfer t)
    {
        all.Remove(t);
        Console.WriteLine("Client getrennt");
    }
}
