using Kakuro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;

namespace Matura_2023_A2
{
    
    public partial class MainWindow : Window
    {
        public static int cols;
        public static int rows;
        public MainWindow()
        {
            InitializeComponent();
            KakuroControl a = new KakuroControl();
        }
        private void New_Game(object sender, RoutedEventArgs e)
        {
            bool getniggered = false;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML-Dateien|*.xml";
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                using var file = dialog.OpenFile();
                using XmlReader reader = XmlReader.Create(file);
                // Beispiel: ..\Kapitel 14\UsingXmlData
                    List<Cell> liste = new List<Cell>();

                    while (reader.Read())
                    {

                        // Prüfen, ob es sich aktuell um ein Element handelt

                        if (reader.NodeType == XmlNodeType.Element)
                        {

                            // Alle relevanten Elemente untersuchen

                            switch (reader.Name)
                            {
                                case "Field":

                                    // Neue Person erzeugen und in Liste eintragen

                                    Cell cell = new Cell();
                                    
                                    if (reader.HasAttributes)
                                    {

                                        // Attributsliste durchlaufen

                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name == "X")
                                                cell.x = int.Parse(reader.Value);
                                            else if (reader.Name == "Y")
                                                cell.y = int.Parse(reader.Value);
                                        }
                                        cell.type = celltype.gesperrrrt;
                                    }
                                    liste.Add(cell);
                                    break;
                                case "Sum":
                                     cell = new Cell();
                                     KakuroControl control = new KakuroControl();

                                    if (reader.HasAttributes)
                                    {

                                        // Attributsliste durchlaufen

                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name == "X")
                                               cell.x  = int.Parse(reader.Value);
                                            else if (reader.Name == "Y")
                                                cell.y = int.Parse(reader.Value);
                                            else if (reader.Name == "Vertical")
                                                control.Vertical = int.Parse(reader.Value);
                                            else if (reader.Name == "Horizontal")
                                                control.Horizontal = int.Parse(reader.Value);
                                        }
                                    cell.type = celltype.kaka;
                                         cell.kak = control;
                                         liste.Add(cell);
                                    }
     
                                    break;
                                case "Kakuro":
                                    getniggered = true;
                                    if (reader.HasAttributes)
                                    {

                                        // Attributsliste durchlaufen

                                        while (reader.MoveToNextAttribute())
                                        {
                                            if (reader.Name == "Rows")
                                                rows = int.Parse(reader.Value);
                                            else if (reader.Name == "Columns")
                                                cols = int.Parse(reader.Value);
                                        }
                                    }
                                    break;

 
                            }
                        }
                    }

                    reader.Close();
                if (!getniggered)
                {
                    MessageBox.Show("kein kakuro gefunden ");
                }
                nigggggggggggggggggggger.Children.Clear();
                nigggggggggggggggggggger.Columns = cols;
                nigggggggggggggggggggger.Rows = rows;
                for(int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        bool isniggered = false;
                        foreach(var zelle in liste)
                        {
                            if (zelle.x == j && zelle.y == i)
                            {
                                switch (zelle.type)
                                {
                                    case celltype.gesperrrrt: 
                                        isniggered = true;
                                        nigggggggggggggggggggger.Children.Add(zelle.TextBlock);
                                        break;
                                    case celltype.kaka:
                                        isniggered = true;
                                        nigggggggggggggggggggger.Children.Add(zelle.kak);
                                        break;
                                }

                            }
                        }
                        if (!isniggered)
                        {
                            Cell cell = new Cell();
                            cell.type = celltype.normal;
                            cell.x = j;
                            cell.y = i;
                            liste.Add(cell);
                            cell.TextBox.TextChanged += (sender, e) => {
                                TextBox tb  = sender as TextBox;
                                
                                int n;
                                if (int.TryParse(tb.Text, out n) )
                                {
                                    if (n <= 0 || n >= 10)
                                    {
                                        tb.Text = tb.Tag.ToString();

                                    }
                                    else
                                    {
                                        tb.Tag = tb.Text;
                                        Checkertobi(liste);
                                    }
                                }
                                else
                                {
                                    if (tb.Text != "")
                                    {
                                        tb.Text = tb.Tag.ToString(); 
                                    }
                                }
                            };
                            nigggggggggggggggggggger.Children.Add(cell.TextBox);

                        }

                    }

                }
                
                }
               
            }
        private void Checkertobi(List<Cell> liste)
        {
            var query = from zelle  in liste
                        where zelle.type == celltype.kaka
                        select zelle;
            foreach (var item in query)
            {
                if (item.kak.Horizontal != 0)
                {
                    var fields = from zelle in liste
                              where zelle.x > item.x && zelle.y == item.y 
                              orderby zelle.x ascending
                              select zelle;
                    int summe = 0;
                    List<int> summenliste = new List<int>();
                    foreach (var item1 in fields)
                    {

                        if (item1.type == celltype.normal)
                        {
                            item1.TextBox.Background = Brushes.White;
                            item1.TextBox.Foreground = Brushes.Black;
                            int n = 0;
                            int.TryParse(item1.TextBox.Text, out n);
                            if (summenliste.Contains(n))
                            {
                                item1.TextBox.Foreground = Brushes.Red;
                            }
                            else
                            {
               
                                if (n == 0 )
                                {
                                    summe = 0;
                                    break;
                                }
                                summenliste.Add(n);
                                summe += n;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (summe != item.kak.Horizontal && summe != 0)
                    {
                        foreach (var item1 in fields)
                        {
                            if (item1.type == celltype.normal)
                            {
                                item1.TextBox.Background = Brushes.DarkRed;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                    if (item.kak.Vertical != 0)
                    {
                        var fields = from zelle in liste
                                     where zelle.x == item.x && zelle.y  > item.y
                                     orderby zelle.y ascending
                                     select zelle;
                        int summe = 0;
                        List<int> summenliste = new List<int>();
                        foreach (var item1 in fields)
                        {

                            if (item1.type == celltype.normal)
                            {
                                item1.TextBox.Background = Brushes.White;
                                item1.TextBox.Foreground = Brushes.Black;
                                int n = 0;
                                int.TryParse(item1.TextBox.Text, out n);
                                if (summenliste.Contains(n))
                                {
                                    item1.TextBox.Foreground = Brushes.Red;
                                }
                                else
                                {

                                    if (n == 0)
                                    {
                                        summe = 0;
                                        break;
                                    }
                                    summenliste.Add(n);
                                    summe += n;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (summe != item.kak.Vertical && summe != 0)
                        {
                            foreach (var item1 in fields)
                            {
                                if (item1.type == celltype.normal)
                                {
                                    item1.TextBox.Background = Brushes.DarkRed;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                
            }
        }


    }

    }

