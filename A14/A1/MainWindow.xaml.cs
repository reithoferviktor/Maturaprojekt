using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace A1
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AbstractExpression parser = null; //mit konkreter Klasse anlegen

            //Variablenliste zurücksetzen
            AbstractExpression.variables.Clear();

            //Formel parsen
            parser.ParseFormel(new List<char>(textBox.Text.ToCharArray()));

            //Auswerten für Feld 0001
            bool result = parser.Interpret(getBooleanFromInt(AbstractExpression.variables, 1));


        }

        public Dictionary<char, bool> getBooleanFromInt(List<char> variables, int value)
        {
            string binary = Convert.ToString(value, 2).PadLeft(variables.Count, '0');
            if (variables.Count != binary.Length)
                throw new ArgumentException("Not enough variables");
            Dictionary<char, bool> res = new Dictionary<char, bool>();
            for(int i = 0; i < variables.Count; i++)
            {
                res.Add(variables[i], binary[i] == '0' ? false : true);
            }
            return res;
        }
    }
}
