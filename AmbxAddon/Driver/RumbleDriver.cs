using amBXLib;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmbxAddon.Driver
{
    public class RumbleDriver
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly float MAX_RUMBLE_INTENSITY = 1.0f;
        private static readonly float MAX_RUMBLE_SPEED = 10.0f;

        private amBX amBX;
        private amBXRumble rumble;
        public amBXRumbleSetting RumbleSetting { get; private set; }

        public RumbleDriver(amBX amBX)
        {
            this.amBX = amBX;
            InitRumble();
        }

        [HandleProcessCorruptedStateExceptions]
        private void InitRumble()
        {
            try {
                log.Debug("Init Rumble");
                rumble = amBX.CreateRumble(CompassDirection.Everywhere, RelativeHeight.AnyHeight);
                RumbleSetting = CreateRumbleSetting(RumbleType.Boing, 0, 0);
                rumble.RumbleSetting = RumbleSetting;
            } catch (AccessViolationException e)
            {
                Console.Error.WriteLine("error" + e);
            }
        }
        
        public void StopRumble()
        {
            log.Debug("Stop Rumble");
            rumble.Enabled = false;
            RumbleSetting = CreateRumbleSetting(RumbleType.Thunder, 0, 0);
            rumble.RumbleSetting = RumbleSetting;
        }

        public void SetRumbleIntensity(float speed, float intensity)
        {
            if (speed >= MAX_RUMBLE_SPEED)
                speed = MAX_RUMBLE_SPEED;
            if (intensity >= MAX_RUMBLE_INTENSITY)
                intensity = MAX_RUMBLE_INTENSITY;

            log.Debug($"Set Rumble Intensity {speed}, {intensity}");
            RumbleSetting = CreateRumbleSetting(RumbleType.Boing, speed, intensity);
            rumble.RumbleSetting = RumbleSetting;
            rumble.Enabled = true;
        }

        [HandleProcessCorruptedStateExceptions]
        public void IncreaseRumbleIntensity(float speed, float intensity)
        {
            float newSpeed = RumbleSetting.Speed + speed;
            float newIntensity = RumbleSetting.Intensity + intensity;

            if (newSpeed >= MAX_RUMBLE_SPEED)
                newSpeed = MAX_RUMBLE_SPEED;
            if (newIntensity >= MAX_RUMBLE_INTENSITY)
                newIntensity = MAX_RUMBLE_INTENSITY;

            log.Debug($"Increase Rumble Intensity {newSpeed}, {newIntensity}");
            RumbleSetting = CreateRumbleSetting(RumbleType.Boing, newSpeed, newIntensity);
            rumble.RumbleSetting = RumbleSetting;
            rumble.Enabled = true;
        }

        public void RumbleOnce()
        {
            log.Info("Rumble Once");
            rumble.Enabled = true;
            RumbleSetting = CreateRumbleSetting(RumbleType.Explosion, 10, 1f);
            rumble.RumbleSetting = RumbleSetting;
        }

        private amBXRumbleSetting CreateRumbleSetting(RumbleType rumbleType, float speed, float intensity)
        {
            log.Debug("Create Settings " + rumbleType + " " + speed + " " + intensity);
            var setting = new amBXRumbleSetting();
            setting.Type = rumbleType;
            setting.Speed = speed;
            setting.Intensity = intensity;
            return setting;
        }


    }
}
