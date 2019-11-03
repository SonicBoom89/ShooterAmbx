using AmbxAddon.Driver;
using NUnit.Framework;

namespace AmbxAddonTests
{
    public class RumbleDriverTests
    {
        [SetUp]
        public void Setup()
        {
            AmbxDriver.Instance.Rumble.StopRumble();
        }

        [Test]
        public void RumbleOnce()
        {
            AmbxDriver.Instance.Rumble.RumbleOnce();
            Assert.True(true);
        }
    }
}