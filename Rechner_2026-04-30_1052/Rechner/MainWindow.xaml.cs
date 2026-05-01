using System;
using System.Globalization;
using System.Windows;
using System.Net.Sockets;

namespace Rechner
{
    public partial class MainWindow : Window, Receiver
    {
        Transfer t;
        public MainWindow()
        {
            t = new Transfer( new System.Net.Sockets.TcpClient("localhost", 12345), this);
            t.Start();
            InitializeComponent();
        }

        private void Append(string s)
        {
            tb_input.Text += s;
        }

        private void btn_0_Click(object sender, RoutedEventArgs e) => Append("0");
        private void btn_1_Click(object sender, RoutedEventArgs e) => Append("1");
        private void btn_2_Click(object sender, RoutedEventArgs e) => Append("2");
        private void btn_3_Click(object sender, RoutedEventArgs e) => Append("3");
        private void btn_4_Click(object sender, RoutedEventArgs e) => Append("4");
        private void btn_5_Click(object sender, RoutedEventArgs e) => Append("5");
        private void btn_6_Click(object sender, RoutedEventArgs e) => Append("6");
        private void btn_7_Click(object sender, RoutedEventArgs e) => Append("7");
        private void btn_8_Click(object sender, RoutedEventArgs e) => Append("8");
        private void btn_9_Click(object sender, RoutedEventArgs e) => Append("9");

        private void btn_add_Click(object sender, RoutedEventArgs e) => Append("+");
        private void btn_sub_Click(object sender, RoutedEventArgs e) => Append("-");
        private void btn_mul_Click(object sender, RoutedEventArgs e) => Append("*");
        private void btn_div_Click(object sender, RoutedEventArgs e) => Append("/");
        private void btn_pow_Click(object sender, RoutedEventArgs e) => Append("^");

        private void btn_paren_open_Click(object sender, RoutedEventArgs e) => Append("(");
        private void btn_paren_close_Click(object sender, RoutedEventArgs e) => Append(")");

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            tb_input.Text = "";
            tbl_output.Text = "";
        }

        private void btn_backspace_Click(object sender, RoutedEventArgs e)
        {
            if (tb_input.Text.Length > 0)
                tb_input.Text = tb_input.Text.Substring(0, tb_input.Text.Length - 1);
        }

        private void btn_equals_Click(object sender, RoutedEventArgs e)
        {
            t.Send(new MSG { expression = tb_input.Text, Type = Type.Request});
        }

        public void ReceiveMessage(MSG m, Transfer t)
        {
            if (m.Type == Type.Error)
            {
                Dispatcher.Invoke(() => { yurrlb.ItemsSource = m.Errors; });
            }
            else if (m.Type == Type.Answer)
            {
                Dispatcher.Invoke(() => { tbl_output.Text = m.ergebnis.ToString(); });

            }
        }

        public void TransferDisconnected(Transfer t)
        {
        }

        public void AddDebugInfo(Transfer t, string m, bool sent)
        {
        }
    }
}
