using System.Net;
using System.Net.Sockets;
using NetworkLib;
using Server;

const int Port = 5050;

var graph = new GraphData();
graph.Load("data/pois.csv", "data/verbindungen.csv");
Console.WriteLine($"Geladen: {graph.Pois.Count} POIs, {graph.Edges.Count} Verbindungen");

var listener = new TcpListener(IPAddress.Any, Port);
listener.Start();
Console.WriteLine($"Server lauscht auf Port {Port}. Strg+C zum Beenden.");

while (true)
{
    var client = listener.AcceptTcpClient();
    var receiver = new ServerReceiver(graph);
    var transfer = new Transfer(client, receiver);
    transfer.Start();
    Console.WriteLine($"Neuer Client verbunden: {transfer.GetIP()}");
}
