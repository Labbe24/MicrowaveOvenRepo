using System;
using System.IO;
using Castle.Core.Internal;
using Microwave.Classes.Boundary;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step1
    {
        private PowerTube uut;
        private Output output;
        private StringWriter str;

        [SetUp]
        public void Setup()
        {
            str = new StringWriter();
            Console.SetOut(str);
            output = new Output();
            uut = new PowerTube(output);
        }

        [TestCase(1)]
        [TestCase(50)]
        [TestCase(100)]
        public void TurnOn_WasOffCorrectPower_CorrectOutput(int power)
        {
            uut.TurnOn(power);
            Assert.That(str.ToString().Contains($"PowerTube works with {power}"));
        }

        [Test]
        public void TurnOff_WasOn_CorrectOutput()
        {
            uut.TurnOn(50);
            uut.TurnOff();
            Assert.That(str.ToString().Contains($"PowerTube turned off"));
        }

        [Test]
        public void TurnOff_WasOff_NoOutput()
        {
            uut.TurnOff();
            Assert.That(str.ToString().IsNullOrEmpty());
        }

    }
}
