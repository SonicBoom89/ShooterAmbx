using amBXLib;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;
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
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.ComponentModel.IContainer components;

        public AmbxAddon()
        {
            Start();
            CreateNotifyicon();
        }

        // First, a MouseHookListener object must exist in the class
        private MouseHookListener m_mouseListener;
        private KeyboardHookListener m_keyboardListener;
        amBXRumble rumble;
        amBXRumbleSetting setting;
        amBXFan fan1;
        amBXFan fan2;
        amBX amBX;
        bool isMouseClicking;
        bool isRunning;
        float runBoost;

        // Subroutine for activating the hook
        public void Activate()
        {
            // Note: for an application hook, use the AppHooker class instead
            m_mouseListener = new MouseHookListener(new GlobalHooker());

            // The listener is not enabled by default
            m_mouseListener.Enabled = true;

            // Set the event handler
            // recommended to use the Extended handlers, which allow input suppression among other additional information
            m_mouseListener.MouseDownExt += MouseListener_MouseDownExt;
            m_mouseListener.MouseUp += MouseListener_MouseUpExt;

            m_keyboardListener = new KeyboardHookListener(new GlobalHooker());
            m_keyboardListener.Enabled = true;
            m_keyboardListener.KeyDown += KeyboardListener_KeyDownExt;
            m_keyboardListener.KeyUp += KeyboardListener_KeyUpExt;
        }

        private void KeyboardListener_KeyUpExt(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Space:
                    stopRumble();
                    break;
                case System.Windows.Forms.Keys.W:
                    isRunning = false;
                    stopFans();
                    break;
                case System.Windows.Forms.Keys.R:
                    stopRumble();
                    break;
                case System.Windows.Forms.Keys.LShiftKey:
                    runBoost = 0f;
                    break;
            }
        }

        private void KeyboardListener_KeyDownExt(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Space:
                    jump();
                    break;
                case System.Windows.Forms.Keys.W:
                    run();
                    break;
                case System.Windows.Forms.Keys.R:
                    reload();
                    break;
                case System.Windows.Forms.Keys.LShiftKey:
                    boost();
                    break;
            }
        }

        private void MouseListener_MouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.IsMouseKeyDown)
            {
                shoot();
            }
        }


        private void MouseListener_MouseUpExt(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                isMouseClicking = false;
                stopFans();
                stopRumble();
            }
        }

        private void boost()
        {
            if (isRunning) { runBoost = 0.20f; }
        }

        private void reload()
        {
            rumble.RumbleSetting = createRumbleSetting(RumbleType.Crash, 0.1f, 0.1f);
        }

        private void run()
        {
            Console.WriteLine("Start Run");
            isRunning = true;
            float intensity = 0f;
            Task.Factory.StartNew(() =>
            {
                while (isRunning)
                {
                    Console.WriteLine("Running...");
                    fan1.Enabled = true;
                    intensity = 0.05f + runBoost;
                    fan1.Intensity = intensity;
                    Thread.Sleep(100);
                }
                Console.WriteLine("Stop Running");
            });
        }

        private void jump()
        {
              Console.WriteLine("Start Jump");
            rumble.RumbleSetting = createRumbleSetting(RumbleType.Hit, 0.2f, 0.2f);
        }
         
        private void shoot()
        {
            Console.WriteLine("Start Shoot");
            isMouseClicking = true;
            float fanintensity = 0f;

            rumble.RumbleSetting = createRumbleSetting(RumbleType.Thud, 0.4f, 0.4f);

            Task.Factory.StartNew(() =>
            {
                //increase Intensity
                while (isMouseClicking)
                {
                    Console.WriteLine("Shooting...");
                    fan1.Enabled = true;
                    fanintensity += 0.05f;
                    fan1.Intensity = fanintensity;

                    setting.Speed += 0.05f;
                    setting.Intensity += 0.05f;
                    rumble.RumbleSetting = setting;

                    Thread.Sleep(100);
                }
                Console.WriteLine("Stop Shooting");
            });
        }

        private void stopRumble()
        {
            Console.WriteLine("Stop Rumble");
            rumble.Dispose(); 
            Task.Factory.StartNew(() =>
            {
                amBX = new amBXLib.amBX(1, 0, "AmbxAddon", "1.0.0");
                rumble = amBX.CreateRumble(CompassDirection.Everywhere, RelativeHeight.AnyHeight);
            });
          
        }

        private void stopFans()
        {
            Console.WriteLine("Stop Fans");
            fan1.Intensity = 0;
        }


        private amBXRumbleSetting createRumbleSetting(RumbleType rumbleType, float speed, float intensity)
        {
            Console.WriteLine("Create Settings " + rumbleType + " " + speed + " "  + intensity);
            setting.Type = rumbleType;
            setting.Speed = speed;
            setting.Intensity = intensity;
            return setting;
        }

        public void Start()
        {
            Activate();
            try
            {
                amBX = new amBXLib.amBX(1, 0, "AmbxAddon", "1.0.0");
                fan1 = amBX.CreateFan(CompassDirection.Everywhere, RelativeHeight.AnyHeight);
                rumble = amBX.CreateRumble(CompassDirection.Everywhere, RelativeHeight.AnyHeight);
                setting = new amBXRumbleSetting();
            } catch (Exception)
            {
                MessageBox.Show("AmBX konnte nicht initialisiert werden.");
            }
        }


        private void CreateNotifyicon()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem1 });

            // Create the NotifyIcon.
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = new Icon("gun3.ico");

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = this.contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "amBX Addon";
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            //notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            //notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);

        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            isMouseClicking = false;
            isRunning = false;
            Application.Exit();
        }
    }

}
