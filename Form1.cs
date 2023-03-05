using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7;
using S7.Net;
using S7.Net.Types;
using S7.Net.Protocol;


/*
 * Exceptions to be treated:
 *      - If the app don't connect to DB
 * 
 * */

namespace EngineNum_match_validator
{
    public partial class Form1 : Form
    {
        Plc PLC_M1 = new Plc(CpuType.S7300, "140.100.101.1", 0, 2);
        



        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {


            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=172.16.0.1;Initial Catalog=HMB;User ID=sa;Password=T00lsNetPwd";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand command;
            SqlDataReader reader;
            string query, Output = "";


            query = "SELECT TOP (10) [ID]" +
                ",[ENG_CODE]" +
                ",[ENG_TYPE]" +
                ",[ENG_SN]" +
                ",[BLK_NO]" +
                ",[HEAD_NO]" +
                " FROM [HMB].[SQS].[ENG_BUILD_DATA]" +
                "ORDER BY ID DESC";
            //"WHERE ENG_SN = '029563'";

            command = new SqlCommand(query, cnn);
            reader = command.ExecuteReader();

            while (reader.Read() && true)
            {
                Output += reader.GetValue(0) + " - " + reader.GetValue(1) + " - " + reader.GetValue(2) + " - " + reader.GetValue(3) + " - " + reader.GetValue(4) + " - " + reader.GetValue(5) + "\n\n";

            }

            MessageBox.Show(Output);


            reader.Close();
            command.Dispose();
            cnn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PLC_M1.Open();

            if (PLC_M1.IsConnected == true)
            {
                //MessageBox.Show("PLC CONNECTED!");
                label1.BackColor = Color.Green;
            }
            else
            {
                MessageBox.Show("FAIL!");
                label1.BackColor = Color.Red;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            PLC_M1.Close();
            if (PLC_M1.IsConnected == false)
            {
                //MessageBox.Show("Disconnected");
                label1.BackColor = Color.Red;
            }
            else
            {
                MessageBox.Show("Fail to disconnect!");
                label1.BackColor = Color.Green;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var bytes = PLC_M1.ReadBytes(DataType.DataBlock, 180, 0, 50);

            string message = "";

            for (int i = 0; i < 50; i++)
            {
                message += (Convert.ToChar(bytes[i]).ToString()) + " ";
                if (i == 9 || i == 26)
                {
                    message += "\r\n\r\n";
                }
            }

            textBox1.Text = message;
        }
    }
}