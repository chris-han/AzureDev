using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EventHubHelper;
using System.Diagnostics;

namespace Test_EventHub
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EventHubHelper.MetricEvent info = new MetricEvent() {
                DeviceId = 1,
                MakeTime = DateTime.Now,
                Purity = 100,
                Shortage = 98
            };
            var task = EventHubHelper.EventHubProxy.WriteDataAsync(info);
            //task.ConfigureAwait(false);
            task.ContinueWith(t => { Debug.Write(t.Exception.Message); },
        TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
