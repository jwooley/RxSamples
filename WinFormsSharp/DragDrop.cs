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
    public partial class DragDrop : Form
    {
        public DragDrop()
        {
            InitializeComponent();
            WireRx();
        }
        public void WireRx()
        {
            var mouseDown = from start in Observable.FromEventPattern<MouseEventArgs>(logo, "MouseDown")
                           select start.EventArgs.Location;

            var mouseUp = Observable.FromEventPattern<MouseEventArgs>(this, "MouseUp");
            var mouseMove = from move in Observable.FromEventPattern<MouseEventArgs>(this, "MouseMove")
                            select move.EventArgs.Location;

            var q = from startLocation in mouseDown
                    from endLocation in mouseMove.TakeUntil(mouseUp)
                    select new
                    {
                        X = endLocation.X - startLocation.X,
                        Y = endLocation.Y - startLocation.Y
                    };

            q.Subscribe(value =>
            {
                logo.Left = value.X;
                logo.Top = value.Y;    
            });
        }
    }
}
