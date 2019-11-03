using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using amBXLib;

namespace AmbxAddon.Driver
{
    public class AmbxDriver
    {
        private amBX amBX;

        #region Singleton

        private static AmbxDriver _instance;
        public static AmbxDriver Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AmbxDriver();
                }
                return _instance;
            }
        }
        private AmbxDriver()
        {
            Setup();
        }

        #endregion

        public void Setup()
        {
            try
            {
                amBX = new amBX(1, 0, "AmbxAddon", "2.0.0");
                Fans = new FanDriver(amBX);
                Rumble = new RumbleDriver(amBX);
            }
            catch (Exception e)
            {
                MessageBox.Show("AmBX konnte nicht initialisiert werden: " + e.Message);
            }
        }

        public FanDriver Fans { get; private set; }

        public RumbleDriver Rumble { get; private set; }

    }
}
