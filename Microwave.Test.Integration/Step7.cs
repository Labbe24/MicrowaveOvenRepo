using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step7
    {
        private UserInterface ui; // Y
        private Door uut; // S
        private IButton powerButton; // S
        private IButton timeButton; // S
        private IButton startCancelButton; // S
        private Light light; // X
        private Display display; // X
        private CookController cookController; // S
        private PowerTube powerTube; // X
        private ITimer timer; // S
        private Output output; // X
        private StringWriter str;
        [SetUp]
        public void Setup()
        {
            str = new StringWriter();
            Console.SetOut(str);

            uut = new Door();

            powerButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();
            timer = Substitute.For<ITimer>();

            output = new Output();
            light = new Light(output);
            display = new Display(output);
            powerTube = new PowerTube(output);

            cookController = new CookController(timer, display, powerTube);
            ui = new UserInterface(powerButton, timeButton, startCancelButton, uut, display, light, cookController);
            cookController.UI = ui;
        }

        [Test]
        public void Open_1subscriber_LightTurnedOn()
        {
            uut.Open();
            Assert.That(str.ToString().Contains("Light is turned on"));
        }

        [Test]
        public void Close_1subscriber_LightTurnedOff()
        {
            uut.Open();
            uut.Close();
            Assert.That(str.ToString().Contains("Light is turned off"));
        }
    }
}
