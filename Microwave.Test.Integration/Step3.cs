using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Castle.Core.Internal;
using Microwave.Classes.Boundary;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Step3
    {
        private Light uut;
        private StringWriter str;
        private Output output;

        [SetUp]
        public void Setup()
        {
            output = new Output();
            uut = new Light(output);
            str = new StringWriter();
        }

        [Test]
        public void TurnOn_WasOff_CorrectOutput()
        {
            uut.TurnOn();
            Assert.That(str.ToString().Contains("Light is turned on"));
        }

        [Test]
        public void TurnOff_WasOn_CorrectOutput()
        {
            uut.TurnOn();
            uut.TurnOff();
            Assert.That(str.ToString().Contains("Light is turned off"));
        }

        [Test]
        public void TurnOn_WasOn_CorrectOutput()
        {
            uut.TurnOn();
            uut.TurnOn();
            Assert.That(str.ToString().IsNullOrEmpty());
        }

        [Test]
        public void TurnOff_WasOff_CorrectOutput()
        {
            uut.TurnOff();
            Assert.That(str.ToString().IsNullOrEmpty());
        }
    }
}
