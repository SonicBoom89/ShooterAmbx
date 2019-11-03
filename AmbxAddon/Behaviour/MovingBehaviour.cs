using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AmbxAddon.Driver;
using log4net;
using MouseKeyboardActivityMonitor;

namespace AmbxAddon.Behaviour
{
    public class MovingBehaviour : Behaviour
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsRunning { get; private set; }
        public bool RunBoostEnabled { get; private set; }

        private static readonly float RUN_BOOST_INTENSITY = 0.2f;

        private void run()
        {
            if (!IsRunning)
            {

            log.Debug("Start Run");
            IsRunning = true;
            AmbxDriver.Instance.Fans.StartFans();
            Task.Factory.StartNew(() =>
            {
                while (IsRunning)
                {
                    float intensity = 0.3f;
                   
                    if (RunBoostEnabled)
                    {
                        intensity += RUN_BOOST_INTENSITY;
                    } else if (intensity - RUN_BOOST_INTENSITY >= 0 )
                    {
                        intensity -= RUN_BOOST_INTENSITY;
                    }
                    AmbxDriver.Instance.Fans.SetFanIntensity(intensity);

                    Thread.Sleep(200);
                }
            });
            } else
            {
                log.Debug("Already Running!");
            }
        }
        private void runboost()
        {
            if (IsRunning) { RunBoostEnabled = true; }
        }

        private void jump()
        {
            log.Debug("Start Jump");
            //rumble.RumbleSetting = createRumbleSetting(RumbleType.Hit, 0.2f, 0.2f);
        }

        private void stopRun()
        {
            log.Debug("Stop Running");
            IsRunning = false;
            AmbxDriver.Instance.Fans.StopFans();
        }
        private void stopRunBoost()
        {
            RunBoostEnabled = false;
        }

        private void stopJump()
        {
            //
        }

        internal override void OnMouseDown(object sender, MouseEventExtArgs e)
        {
            //
        }

        internal override void OnMouseUp(object sender, MouseEventArgs e)
        {
            //
        }

        internal override void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Space:
                    jump();
                    break;
                case System.Windows.Forms.Keys.W:
                    run();
                    break;
                case System.Windows.Forms.Keys.LShiftKey:
                    runboost();
                    break;
            }
        }

        internal override void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Space:
                    stopJump();
                    break;
                case System.Windows.Forms.Keys.W:
                    stopRun();
                    break;
                case Keys.LShiftKey:
                    stopRunBoost();
                    break;
            }
        }

    }
}
