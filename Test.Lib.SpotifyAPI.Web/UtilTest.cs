using System;
using Lib.SpotifyAPI.Web;
using NUnit.Framework;

namespace Test.Lib.SpotifyAPI.Web
{
    [TestFixture]
    public class UtilTest
    {
        [Test]
        public void TimestampShouldBeNoFloatingPoint()
        {
            string timestamp = DateTime.Now.ToUnixTimeMillisecondsPoly().ToString();

            StringAssert.DoesNotContain(".", timestamp);
            StringAssert.DoesNotContain(",", timestamp);
        }
    }
}