using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class r365CalcTest
    {
        [Test]
        public void OneValue()
        {
            var calc = new r365_calc.Calc();
            calc.Calculate("20");
            Assert.That(calc.Output() == 20);
        }

        [Test]
        public void TwoValue()
        {
            var calc = new r365_calc.Calc();
            calc.Calculate("1,5000");
            Assert.That(calc.Output() == 5001);
        }

        [Test]
        public void ThreeValue()
        {
            var calc = new r365_calc.Calc();
            Assert.Throws<System.ArgumentException>(() => calc.Calculate("1,1,1"));
        }

        [Test]
        public void NoValue()
        {
            var calc = new r365_calc.Calc();
            calc.Calculate("");
            Assert.That(calc.Output() == 0);
        }

        [Test]
        public void JunkValue()
        {
            var calc = new r365_calc.Calc();
            calc.Calculate("5,tytyt");
            Assert.That(calc.Output() == 5);
        }
    }
}