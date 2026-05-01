using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Xml;

namespace RechnerServer
{
    public enum Tokentype { PM, MD, H, Z, LKL, RKL, ERROR};
    public class Token
    {
        public Tokentype Type = Tokentype.ERROR;
        public string value = "ERROR";
        public int ind;
    }
    public class server : Receiver
    {
        public List<Transfer> clients = new();
        public TcpListener lsd;

        public server() 
        {
            lsd = new TcpListener(System.Net.IPAddress.Any,12345);
            lsd.Start();

                    ThreadPool.QueueUserWorkItem(q => {
                    while (true)
                    {
                        try
                        {
                            TcpClient client;
                        client = lsd.AcceptTcpClient();
                        Transfer f = new Transfer(client, this);
                                f.Start();
                        clients.Add(f);
                        }
                            catch { }
                        }
                    });

        }
        public void AddDebugInfo(Transfer t, string m, bool sent)
        {
        }

        public void ReceiveMessage(MSG m, Transfer t)
        {
            Console.WriteLine(m.Type);
            if (m.Type == Type.Request)
            {
                List<Token> Tokenlist = tokenize(m.expression);
                var curry = Tokenlist.Where(x => x.Type == Tokentype.ERROR);
                if (curry.ToList().Count > 0)
                {
                    MSG message = new MSG();
                    message.Type = Type.Error;
                    message.Errors = new List<Err>();
                    foreach (var item in curry)
                    {
                        Err fehler = new Err
                        {
                            error = item.value,
                            index = item.ind
                        };
                        message.Errors.Add(fehler);
                    }
                    t.Send(message);
                    return;

                }
                else
                {
                    PLUSMINUS pm = new PLUSMINUS();
                    pm.Parse(Tokenlist);
                    Console.WriteLine("durchgeparsed");
                    double erg = pm.Run();
                    Console.WriteLine(erg);

                    MSG Message = new MSG
                    {
                        ergebnis = erg,
                        Type = Type.Answer
                    };
                    t.Send(Message);
                    return;
                }
            }
        }
        public List<Token> tokenize(string expr)
        {         
            Regex Number = new Regex(@"\d+(,\d)?(\d+)?");
            Regex Lkl = new Regex(@"\(");
            Regex rkl = new Regex(@"\)");
            Regex PM = new Regex(@"\+|-");
            Regex MD = new Regex(@"\/|\*");
            Regex H = new Regex(@"\^");
            Regex globalregex = new Regex(@".");
            expr = expr.Replace(" ", "");
            MatchCollection mc =  globalregex.Matches(expr);
            List<Token> tokens = new List<Token>();
            int i = 0;
            foreach (Match match in mc)
            {
                if (match.Success)
                {
                    Token token = new Token();
                    if (Number.IsMatch(match.Value))
                    {
                        token.value = match.Value;
                        token.Type = Tokentype.Z;
                    }
                    else if (Lkl.IsMatch(match.Value))
                    {
                        token.value = match.Value;
                        token.Type = Tokentype.LKL;
                    }
                    else if (rkl.IsMatch(match.Value))
                    {
                        token.value = match.Value;
                        token.Type = Tokentype.RKL;
                    }
                    else if (H.IsMatch(match.Value))
                    {
                        token.value = match.Value;
                        token.Type = Tokentype.H;
                    }
                    else if (PM.IsMatch(match.Value))
                    {
                        token.value = match.Value;
                        token.Type = Tokentype.PM;
                    }
                    else if (MD.IsMatch(match.Value))
                    {
                        token.value = match.Value;
                        token.Type = Tokentype.MD;
                    }
                    else
                    {
                        token.ind = i;
                        token.value = match.Value;
                    }
                    i += match.Value.Length;
                    tokens.Add(token);
                }
            }


            return tokens;


        }
        public void TransferDisconnected(Transfer t)
        {
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            server sv = new server();
            Console.ReadLine();
        }
    }
}
