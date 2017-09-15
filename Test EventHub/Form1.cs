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

        private void btn100Msg_Click(object sender, EventArgs e)
        {
            var task = EventHubHelper.EventHubProxy.Test100MessagesToEventHub(1000);
            //task.ConfigureAwait(false);
            task.ContinueWith(t => { Debug.Write(t.Exception.Message); },
        TaskContinuationOptions.OnlyOnFaulted);
        }

        private void btnPBI_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            MetricEvent info = new MetricEvent()
            {
                DeviceId = 1,
                        MakeTime = DateTime.Now,
                        Purity = random.Next(100),
                        Shortage = random.Next(100)
                    };
            var task = EventHubHelper.EventHubProxy.SendMessagesToPBI(info);
            //task.ConfigureAwait(false);
            task.ContinueWith(t => { Debug.Write(t.Exception.Message); },
        TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
