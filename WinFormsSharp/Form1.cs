using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<IDisposable> subscriptions = new List<IDisposable>();

        protected override void OnLoad(EventArgs e)
        {
            ConfigureTaps();
        }
        private void ConfigureTaps()
        {
            var taps = Observable.FromEventPattern(button1, "Click");
            var intervals = taps.TimeInterval().Select(t => t.Interval);

            subscriptions.Add(
                intervals
                .Buffer(TimeSpan.FromSeconds(5))
                .Select(bufferedIntervals => bufferedIntervals.Any() ? bufferedIntervals.Average(ts => ts.TotalMilliseconds) : 0)
                .Select(avg => avg == 0 ? 0 : (int)(60000 / avg))
                .ObserveOn(button1)
                .Subscribe(bpm => button1.Text = bpm > 0 ?  $"BPM : {bpm}" : "Start tapping again"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new DragDrop().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new FileWatcher().ShowDialog();
        }

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                subscriptions.ForEach(s => s?.Dispose());
            }
            base.Dispose(disposing);
        }

        private void btnTranslator_Click(object sender, EventArgs e)
        {
            new Translator().ShowDialog();
        }
    }
}
