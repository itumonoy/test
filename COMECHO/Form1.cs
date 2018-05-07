using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMECHO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void frmLoad(object sender, EventArgs e)
        {
            cbxCom.Items.AddRange(SerialPort.GetPortNames());
            cbxCom.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                cbxCom.Enabled = true;
                (sender as Button).Text = "Open";
                return;
            }
            try
            {

                serialPort1.PortName = cbxCom.Text;
                serialPort1.Open();
            }catch(Exception ex)
            {
                YVY.Dialogs.Exception(ex);
            }

            if (serialPort1.IsOpen)
            {
                (sender as Button).Text = "Close";

            }
            cbxCom.Enabled = !serialPort1.IsOpen;
        }

        byte[] rcvBff = new byte[256];

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                while (true)
                {
                    int siz = (sender as SerialPort).Read(rcvBff, 0, rcvBff.Length);
                    if (siz <= 0) break;
                    (sender as SerialPort).Write(rcvBff, 0, siz);

                    string str = serialPort1.Encoding.GetString(rcvBff, 0, siz);

                    Append(str);

                    //if (siz < rcvBff.Length) break;
                }
            }
            catch { }
        }

        bool LogStop;

        void Append(string str)
        {
            if (LogStop) return;

            this.Invoke((Action)delegate ()
            {
                richTextBox1.AppendText(str);
                YVY.EDIT.edBottomScroll(richTextBox1);
            });

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tag = (sender as ToolStripMenuItem).Tag as string;

            switch (tag)
            {
                case "1":       // ログクリア
                    richTextBox1.Clear();
                    break;
                case "2":       // STOP
                    LogStop = !LogStop;
                    (sender as ToolStripMenuItem).Checked = LogStop;
                    break;
            }
        }

        private void cbxDropDown(object sender, EventArgs e)
        {
            ComboBox cbx = (sender as ComboBox);
            string s = cbx.Text;
            cbx.Items.Clear();
            cbx.Items.AddRange(SerialPort.GetPortNames());
            cbx.Text = s;
        }
    }
}
