using AmbxAddon.Behaviour;
using AmbxAddon.Driver;
using amBXLib;
using amBXLib.Net;
using amBXLib.Net.Device.Components;
using amBXLib.Net.Helpers;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
using Ninject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmbxAddon
{
    public class AmbxAddon : ApplicationContext
    {

        public AmbxAddon()
        {
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.CreateNotifyicon("amBX Addon", "gun3.ico", "E&xit");
            var movingBehaviour = new MovingBehaviour();
            var shootingBehaviour = new ShootingBehaviour();
        }
                       
    }

}
