using AmbxAddon.Core;
using AmbxAddon.Driver;
using MouseKeyboardActivityMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmbxAddon.Behaviour
{
    public abstract class Behaviour
    {

        public Behaviour()
        {
            PeripheralHooks.Instance.OnKeyUp += OnKeyUp;
            PeripheralHooks.Instance.OnKeyDown += OnKeyDown;
            PeripheralHooks.Instance.OnMouseUp += OnMouseUp;
            PeripheralHooks.Instance.OnMouseDown += OnMouseDown;
        }


        internal abstract void OnMouseDown(object sender, MouseEventExtArgs e);
        internal abstract void OnMouseUp(object sender, MouseEventArgs e);
        internal abstract void OnKeyDown(object sender, KeyEventArgs e);
        internal abstract void OnKeyUp(object sender, KeyEventArgs e);

        public void Dispose()
        {
            PeripheralHooks.Instance.OnKeyUp -= OnKeyUp;
            PeripheralHooks.Instance.OnKeyDown -= OnKeyDown;
            PeripheralHooks.Instance.OnMouseUp -= OnMouseUp;
            PeripheralHooks.Instance.OnMouseDown -= OnMouseDown;
        }
    }
}
