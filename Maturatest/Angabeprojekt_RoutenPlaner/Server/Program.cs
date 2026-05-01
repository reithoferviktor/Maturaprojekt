using System.Net;
using System.Net.Sockets;
using NetworkLib;
using Server;

const int Port = 5050;

var graph = new GraphData();
Console.WriteLine($"Geladen: {graph.Stationen.Count} Stationen, {graph.Verbindungen.Count} Verbindungen");

var listener = new TcpListener(IPAddress.Any, Port);
listener.Start();
Console.WriteLine($"Server lauscht auf Port {Port}. Strg+C zum Beenden.");

while (true)
{
    var client = listener.AcceptTcpClient();
    var receiver = new ServerReceiver(graph);
    var transfer = new Transfer(client, receiver);
    transfer.Start();
    Console.WriteLine($"Verbunden: {transfer.GetIP()}");
}
