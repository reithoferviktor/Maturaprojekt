using Microsoft.Win32;
using System;
using System.Reflection.PortableExecutable;
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
using System.Xml;

namespace ROBOTIKERN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public enum tokentype
    {
        ERROR, UNTIL, ISA, WENN, REPEAT, COLLECT, RKL, LKL, LETTER, HINDERNIS, DIRECTION, NUMBER, MOVE
    }
    public class Token
    {
        public tokentype tokentype = tokentype.ERROR;
        public string value = "ERROR";
        
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tb.AcceptsReturn = true;
            tb.TextWrapping = TextWrapping.Wrap;
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML-Dateien|*.xml";
               bool? result = dialog.ShowDialog();

            if (result == true)
            {
                robotfield.LoadField(dialog.FileName);
                /*XmlReader reader =  XmlReader.Create(dialog.OpenFile());
                while (reader.Read())
                {

                    // Prüfen, ob es sich aktuell um ein Element handelt

                    if (reader.NodeType == XmlNodeType.Element)
                    {

                        // Alle relevanten Elemente untersuchen

                        switch (reader.Name)
                        {
                            case "Person":

                                // Neue Person erzeugen und in Liste eintragen

                                person = new Person();
                                liste.Add(person);
                                break;
                            case "Vorname":
                                person.Vorname = reader.ReadString();
                                break;
                            case "Zuname":
                                person.Zuname = reader.ReadString();
                                break;
                            case "Alter":
                                person.Alter = reader.ReadElementContentAsInt();
                                break;
                            case "Adresse":

                                // Neue Adresse erzeugen und der Person zuordnen

                                adresse = new Adresse();
                                person.Adresse = adresse;
                                if (reader.HasAttributes)
                                {

                                    // Attributsliste durchlaufen

                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name == "Ort")
                                            adresse.Ort = reader.Value;
                                        else if (reader.Name == "Strasse")
                                            adresse.Strasse = reader.Value;
                                    }
                                }
                                break;
                        }
                    }
                }*/

            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Regex Number = new Regex(@"[1-9]\d*");
            Regex collect = new Regex(@"COLLECT");
            Regex Direction = new Regex(@"LEFT|RIGHT|UP|DOWN");
            Regex WENN = new Regex(@"IF");
            Regex ISTEIN = new Regex(@"IS-A");
            Regex HINDERNIS = new Regex(@"OBSTACLE");
            Regex Repeat = new Regex(@"REPEAT");
            Regex MOVE = new Regex(@"MOVE");
            Regex LETTER = new Regex(@"[A-Z]");
            Regex Until = new Regex(@"UNTIL");
            Regex Move = new Regex(@"MOVE");

            Regex LKL = new Regex(@"\{");
            Regex RKL = new Regex(@"\}");
            Regex globalregex = new Regex(@"[1-9]\d*|COLLECT|MOVE|REPEAT|LEFT|RIGHT|UP|DOWN|IF|IS-A|UNTIL|\{|\}|OBSTACLE|[A-Z]|.");

            MatchCollection mc = globalregex.Matches(tb.Text.Replace(" ", "").Replace("\r", ""));
            List<Token> Tokenlist = new List<Token>();

            foreach (Match m in mc)
            {

                Token token = new Token();

                if (m.Success)
                {
                    if (Number.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.NUMBER;
                    }
                    else if (collect.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.COLLECT;
                    }
                    else if (Direction.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.DIRECTION;
                    }
                    else if (WENN.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.WENN;
                    }
                    else if (ISTEIN.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.ISA;
                    }
                    else if (HINDERNIS.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.HINDERNIS;
                    }

                    else if (Until.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.UNTIL;
                    }
                    else if (LKL.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.LKL;
                    }
                    else if (RKL.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.RKL;
                    }
                    else if (Repeat.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.REPEAT;
                    }
                    else if (Move.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.MOVE;
                    }
                    else if (LETTER.IsMatch(m.Value))
                    {
                        token.value = m.Value;
                        token.tokentype = tokentype.LETTER;
                    }

                    Tokenlist.Add(token);
                    
                }




            }
            Programm orgi = new Programm();
            orgi.parse(Tokenlist);
            orgi.Run(robotfield);
        }
    }
}