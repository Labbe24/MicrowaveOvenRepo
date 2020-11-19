using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microwave.Classes.Boundary;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step2
    {
        private Display uut;
        private Output output;
        private StringWriter str;

        [SetUp]
        public void Setup()
        {
            output = new Output();
            uut = new Display(output);
            str = new StringWriter();
            Console.SetOut(str);
        }

        [Test]
        public void ShowTime_ZeroMinuteZeroSeconds_CorrectOutput()
        {
            uut.ShowTime(0, 0);
            Assert.That(str.ToString().Contains("Display shows: 00:00"));
        }

        [Test]
        public void ShowTime_ZeroMinuteSomeSecond_CorrectOutput()
        {
            uut.ShowTime(0, 5);
            Assert.That(str.ToString().Contains("Display shows: 00:05"));
        }

        [Test]
        public void ShowTime_SomeMinuteZeroSecond_CorrectOutput()
        {
            uut.ShowTime(5, 0);
            Assert.That(str.ToString().Contains("Display shows: 05:00"));
        }

        [Test]
        public void ShowTime_SomeMinuteSomeSecond_CorrectOutput()
        {
            uut.ShowTime(10, 15);
            Assert.That(str.ToString().Contains("Display shows: 10:15"));
        }

        [Test]
        public void ShowPower_Zero_CorrectOutput()
        {
            uut.ShowPower(0);
            Assert.That(str.ToString().Contains("Display shows: 0 W"));
        }

        [Test]
        public void ShowPower_NotZero_CorrectOutput()
        {
            uut.ShowPower(150);
            Assert.That(str.ToString().Contains("Display shows: 150 W"));
        }

        [Test]
        public void Clear_CorrectOutput()
        {
            uut.Clear();
            Assert.That(str.ToString().Contains("Display cleared"));
        }
    }
}
