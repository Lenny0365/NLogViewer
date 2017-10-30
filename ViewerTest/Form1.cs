using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewerTest
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                Logger console = NLog.LogManager.GetLogger("console");
                Logger logControl = NLog.LogManager.GetLogger("Standard");
                Logger MylogControl = NLog.LogManager.GetLogger("MyLog");

                LogLevel level = LogLevel.Trace;
                console.Log(level, "Test message to log");
                console.Error("test errror");

               
                MylogControl.Log(level, "My Test message to log");
                MylogControl.Error("My test errror");

                logControl.Log(level, "Test message to logControl");
                logControl.Error("test errror");
                Exception ex = new Exception("this is long exception message to disply on the screen");
                logControl.Error(ex, "This is error: ");
            }
            catch (Exception ex)
            {

                string error = ex.Message;
            }
        }
    }
}
