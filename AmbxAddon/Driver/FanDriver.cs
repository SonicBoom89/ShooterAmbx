using amBXLib;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbxAddon.Driver
{
    public class FanDriver
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly float MAX_FAN_INTENSITY = 1.0f;

        private amBXFan fan1;

        public FanDriver(amBX amBX)
        {
            fan1 = amBX.CreateFan(CompassDirection.Everywhere, RelativeHeight.AnyHeight);
        }

        public void StartFans()
        {
            log.Debug("Start Fans");
            fan1.Enabled = true;
        }

        public void StopFans()
        {
            log.Debug("Stop Fans");
            SetFanIntensity(0);
        }

        public void SetFanIntensity(float intensity)
        {
            if (intensity >= MAX_FAN_INTENSITY)
                intensity = MAX_FAN_INTENSITY;
            
            log.Debug($"Set Fan Intensity {intensity}");
            fan1.Intensity = intensity;
        }

        internal void IncreaseFanIntensity(float increaseIntensity)
        {
            IncreaseFanIntensityToLimit(increaseIntensity, MAX_FAN_INTENSITY);
        }

        internal void IncreaseFanIntensityToLimit(float increaseIntensity, float max_intensity)
        {
            float newIntensity = fan1.Intensity + increaseIntensity;
            if (newIntensity >= max_intensity)
            {
                newIntensity = max_intensity;
            }
            log.Debug($"Increase Fan Intensity {newIntensity}");
            SetFanIntensity(newIntensity);
        }
    }
}
