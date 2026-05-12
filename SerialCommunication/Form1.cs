using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "connect";
                    labelStatus.Text = "status: Disconnected";

                }
                else
                {
                    serialPortArduino.PortName = (string)comboBoxPoort.SelectedItem;
                    serialPortArduino.BaudRate = Int32.Parse((string)comboBoxBaudrate.SelectedItem);
                    serialPortArduino.DataBits = (int)numericUpDownDatabits.Value;

                    if (radioButtonParityEven.Checked) serialPortArduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPortArduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPortArduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPortArduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPortArduino.Parity = Parity.Space;

                    if (radioButtonStopbitsNone.Checked) serialPortArduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPortArduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPortArduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked) serialPortArduino.StopBits = StopBits.Two;

                    if (radioButtonHandshakeNone.Checked) serialPortArduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPortArduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPortArduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPortArduino.Handshake = Handshake.XOnXOff;

                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPortArduino.Open();
                    string commando = "ping";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    if (antwoord == "pong")
                    {
                        radioButtonVerbonden.Checked = true;
                        buttonConnect.Text = "Disconnect";
                        labelStatus.Text = "Status: Connected";
                    }
                    else
                    {
                        serialPortArduino.Close();
                        labelStatus.Text = "Error: verkeerd antwoord";
                    }
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";
            }
        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    labelStatus.Text = "Error: No open serial connection";
                    return;
                }

                string command = checkBoxDigital2.Checked ? "set d2 high" : "set d2 low";
                serialPortArduino.WriteLine(command);
                labelStatus.Text = $"Status: Sent '{command}'";
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error: " + ex.Message;
            }
        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    labelStatus.Text = "Error: No open serial connection";
                    return;
                }

                string command = checkBoxDigital3.Checked ? "set d3 high" : "set d3 low";
                serialPortArduino.WriteLine(command);
                labelStatus.Text = $"Status: Sent '{command}'";
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error: " + ex.Message;
            }

        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    labelStatus.Text = "Error: No open serial connection";
                    return;
                }

                string command = checkBoxDigital4.Checked ? "set d4 high" : "set d4 low";
                serialPortArduino.WriteLine(command);
                labelStatus.Text = $"Status: Sent '{command}'";
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error: " + ex.Message;
            }

        }

        private void trackBarPWM9_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    labelStatus.Text = "Error: No open serial connection";
                    return;
                }

                string command = $"set pwm9 {trackBarPWM9.Value}";
                serialPortArduino.WriteLine(command);
                labelStatus.Text = $"Status: Sent '{command}'";
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error: " + ex.Message;
            }

        }

        private void trackBarPWM10_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    labelStatus.Text = "Error: No open serial connection";
                    return;
                }

                string command = $"set pwm10 {trackBarPWM10.Value}";
                serialPortArduino.WriteLine(command);
                labelStatus.Text = $"Status: Sent '{command}'";
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error: " + ex.Message;
            }
        }

        private void trackBarPWM11_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    labelStatus.Text = "Error: No open serial connection";
                    return;
                }

                string command = $"set pwm11 {trackBarPWM11.Value}";
                serialPortArduino.WriteLine(command);
                labelStatus.Text = $"Status: Sent '{command}'";
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Error: " + ex.Message;
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerOefening3.Enabled = tabControl.SelectedIndex == 3;
            timerOefening4.Enabled = tabControl.SelectedIndex == 4;
            timerOefening5.Enabled = tabControl.SelectedIndex == 5;
        }

        private void timerOefening3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.ReadExisting();
                    string comando = "get d5";
                    serialPortArduino.WriteLine(comando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital5.Checked = (antwoord == "1");

                    comando = "get d6";
                    serialPortArduino.WriteLine(comando);
                    antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital6.Checked = (antwoord == "1");

                    comando = "get d7";
                    serialPortArduino.WriteLine(comando);
                    antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital7.Checked = (antwoord == "1");


                }

            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";

            }
        }

        private void timerOefening4_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.ReadExisting();
                    string comando = "get a0";
                    serialPortArduino.WriteLine(comando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    labelAnalog0.Text = antwoord;

                    int value = int.Parse(antwoord);
                    labelAnalog0.Text = value.ToString();
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "connect";
            }
        }

        private void timerOefening5_Tick(object sender, EventArgs e)
        {

            {
                try
                {
                    if (!serialPortArduino.IsOpen)
                    {
                        labelStatus.Text = "Error: No open serial connection";
                        return;
                    }

                    // verwijder eerdere antwoorden
                    serialPortArduino.ReadExisting();

                    // Gewenste temperatuur (analoge pin 0) 0..1023 -> 5..45 °C
                    serialPortArduino.WriteLine("get a0");
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    // extraheer alleen de numerieke waarde
                    string numeric = new string(antwoord.Where(c => char.IsDigit(c) || c == '-').ToArray());
                    if (!int.TryParse(numeric, out int rawA0))
                    {
                        throw new Exception("Ongeldig antwoord voor a0: " + antwoord);
                    }

                    double slopeGewenst = (45.0 - 5.0) / 1023.0; // richtingscoëfficiënt
                    double offsetGewenst = 5.0; // offset
                    double gewenst = slopeGewenst * rawA0 + offsetGewenst;
                    labelGewensteTemp.Text = Math.Round(gewenst, 1).ToString("F1") + " °C";

                    // Huidige temperatuur (analoge pin 1) 0..1023 -> 0..500 °C
                    serialPortArduino.DiscardInBuffer();
                    serialPortArduino.WriteLine("get a1");
                    antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.Trim();
                    string[] delen = antwoord.Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    string meetWaarde = delen.Length > 0 ? delen[delen.Length - 1] : "";

                    string value = new string(meetWaarde.Where(c => char.IsDigit(c) || c == '-').ToArray());

                    if (!int.TryParse(value, out int rawA1))
                    {
                        throw new Exception("Ongeldig antwoord voor a1: " + antwoord);
                    }

                    double slopeHuidig = 500.0 / 1023.0;
                    double offsetHuidig = 0.0;
                    double huidig = slopeHuidig * rawA1 + offsetHuidig;
                    labelHuidigeTemp.Text = Math.Round(huidig, 1).ToString("F1") + " °C";

                    // Led aansturen op digitale pin 2: aan als huidig < gewenst
                    
                    string command = huidig < gewenst ? "set d2 high" : "set d2 low";
                    serialPortArduino.WriteLine(command);
                    labelStatus.Text = $"Status: Sent '{command}'";
                }
                catch (Exception exception)
                {
                    labelStatus.Text = "Error: " + exception.Message;
                    try
                    {
                        serialPortArduino.Close();
                    }
                    catch { }
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "connect";
                }
            }
        }
    }
}
