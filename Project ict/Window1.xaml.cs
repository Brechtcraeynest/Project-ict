using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO.Ports;
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
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;


namespace Project_ict
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        SerialPort serialPort = new SerialPort();
        public Window1()
        {
            InitializeComponent();
            foreach (string s in SerialPort.GetPortNames())
                cbxPortName.Items.Add(s);
        }

        private void cbxPortName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serialPort != null)
            {
                if (serialPort.IsOpen)
                    serialPort.Close();

                if (cbxPortName.SelectedItem.ToString() != "None")
                {
                    serialPort.PortName = cbxPortName.SelectedItem.ToString();
                    serialPort.Open();
                }
                else
                {
                    MessageBox.Show("Kies een COM-poort.", "Fout",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort != null)  !serialPort.IsOpen))
            {
                serialPort.WriteLine("Welkom");
                score.Null();
                letter = randomLetters.GetLetter();
                lblletter.Content = $"Letter: {letter}";
                serialPort.WriteLine($"{letter}");
                i = false;
                spelBezig = true;
                tijdTimer = 400;
                aTimer.Enabled = true;
                lblletter.Content = $"Letter: {letter}";
                lblScore.Content = "Score:";
            }
        }
    }
}
