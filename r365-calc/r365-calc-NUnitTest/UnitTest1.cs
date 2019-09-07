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
            // Changed requirement. No longer limited to a max of two values.
            calc.Calculate("1,1,1");
            Assert.That(calc.Output() == 3);
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

        [Test]
        public void MultipleValues()
        {
            var calc = new r365_calc.Calc();
            calc.Calculate("1,2,3,4,5,6,7,8,9,10,11,12");
            Assert.That(calc.Output() == 78);
        }

        [Test]
        public void Newline()
        {
            var calc = new r365_calc.Calc();
            calc.Calculate("1\n2,3");
            Assert.That(calc.Output() == 6);
        }

        [Test]
        public void NoNegatives()
        {
            var calc = new r365_calc.Calc();
            Assert.Throws<System.ApplicationException>(() => calc.Calculate("1,-2,3,-4"))
                .Message.Equals("Negatives are not allowed. You had: -2,-4");
        }
    }
}