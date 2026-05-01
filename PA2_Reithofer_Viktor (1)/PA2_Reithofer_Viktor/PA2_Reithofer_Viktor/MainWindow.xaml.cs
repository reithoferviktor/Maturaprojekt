using PA2_5A_2026;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PA2_Reithofer_Viktor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            List<Token> tokens = new List<Token>();

            Regex number = new Regex(@"\d+");
            Regex drw = new Regex(@"DRAW");
            Regex fr = new Regex(@"FOR");
            Regex trn = new Regex(@"TURN");
            Regex colr = new Regex(@"COLOR");

            Regex lkl = new Regex(@"\{");
            Regex rkl = new Regex(@"\}");
            Regex brsh = new Regex(@"White|Red|Green|Blue");
            Regex direction = new Regex(@"LEFT|RIGHT");

            Regex global = new Regex(@"\d+|White|Red|Green|Blue|\}|\{|COLOR|TURN|FOR|DRAW|LEFT|RIGHT|\s|.");

            string input = txtCode.Text.ToString();
            input = input.Replace(" ", "");
            input = input.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            MatchCollection coll = global.Matches(input);
            foreach (Match match in coll)
            {
                Token t = new Token();
                if (number.Match(match.Value).Success)
                {
                    t.Type = Token.TokenType.number;
                }
                else if (drw.Match(match.Value).Success)
                {
                    t.Type = Token.TokenType.drw;
                }
                else if (fr.Match(match.Value).Success)
                {
                    t.Type = Token.TokenType.fr;
                }
                else if (trn.Match(match.Value).Success)
                {
                    t.Type = Token.TokenType.trn;
                }
                else if (lkl.Match(match.Value).Success)
                {
                    t.Type = Token.TokenType.lkl;
                }
                else if (rkl.Match(match.Value).Success)
                {
                    t.Type = Token.TokenType.rkl;
                }
                else if (brsh.Match(match.Value).Success)
                {
                    t.Type = Token.TokenType.brsh;
                }
                else if (colr.Match(match.Value).Success)
                {
                    t.Type = Token.TokenType.colr;
                }
                else if (direction.Match(match.Value).Success)
                {
                    t.Type = Token.TokenType.direction;
                }
                t.Value = match.Value.ToString();
                tokens.Add(t);
            }
            program prog = new program();
            prog.Parse(tokens);
            txtCode.Text = "durchgeparsed";
            prog.Run(this.zeicher);
        }
    }
}