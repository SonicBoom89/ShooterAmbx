using AmbxAddon.Driver;
using log4net;
using MouseKeyboardActivityMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmbxAddon.Behaviour
{
    public partial class ShootingBehaviour : Behaviour
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ShootingMode ShootingMode { get; } = ShootingMode.DYNAMIC;

        public bool IsShooting { get; private set; }
        public bool IsReloading { get; private set; }

        internal override void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.R:
                    //reload();
                    break;
            }
        }
        internal override void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.R:
                    //stopReload();
                    break;
            }
        }

        internal override void OnMouseDown(object sender, MouseEventExtArgs e)
        {
            if (e.Button == MouseButtons.Left && e.IsMouseKeyDown)
            {
                shoot();
            }
        }

        internal override void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                stopShoot();
            }
        }

        private void reload()
        {
            if (!IsReloading)
            {
                IsReloading = true;
                AmbxDriver.Instance.Rumble.RumbleOnce();
            }
            
        }
        private void stopReload()
        {
            IsReloading = false;
            AmbxDriver.Instance.Rumble.StopRumble();
        }


        private void shoot()
        {
            IsShooting = false;
            if (!IsShooting)
            {
                log.Debug("Start Shoot");
                IsShooting = true;
                AmbxDriver.Instance.Fans.StartFans();
                log.Debug($"Shooting... Mode: {ShootingMode}");
                switch (ShootingMode)
                {
                    case ShootingMode.CONSTANT:
                        ConstantShoot();
                        break;
                    case ShootingMode.DYNAMIC:
                        DynamicShoot();
                        break;
                }
               
            } else
            {
                log.Debug("Already Shooting!");
            }
        }

        private void ConstantShoot()
        {
            Task.Factory.StartNew(() =>
            {
                AmbxDriver.Instance.Fans.IncreaseFanIntensity(1f);
                AmbxDriver.Instance.Rumble.IncreaseRumbleIntensity(10f, 1f);
                log.Debug("Stop Shooting");
            });
        }

        private void DynamicShoot()
        {
            Task.Factory.StartNew(() =>
            {
                //increase Intensity
                while (IsShooting)
                {
                    AmbxDriver.Instance.Fans.IncreaseFanIntensity(0.05f);
                    AmbxDriver.Instance.Rumble.IncreaseRumbleIntensity(0.05f, 0.05f);
                    Thread.Sleep(100);
                }
            });
        }

        private void stopShoot()
        {
            log.Debug("Stop Shooting");
            IsShooting = false;
            AmbxDriver.Instance.Fans.StopFans();
            AmbxDriver.Instance.Rumble.StopRumble();
        }
    }
}
