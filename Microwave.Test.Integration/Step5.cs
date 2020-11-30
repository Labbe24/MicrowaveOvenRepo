using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step5
    {
        private UserInterface uut; // Y
        private IDoor door; // S
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

            // Stubs
            door = Substitute.For<IDoor>();
            powerButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();
            timer = Substitute.For<ITimer>();

            output = new Output();
            light = new Light(output);
            display = new Display(output);
            powerTube = new PowerTube(output);

            cookController = new CookController(timer, display, powerTube);
            uut = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cookController);
            cookController.UI = uut;
        }

        [Test]
        public void Ready_DoorOpen_LightOn()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            Assert.That(str.ToString().Contains("Light is turned on"));
        }

        [Test]
        public void DoorOpen_DoorClose_LightOff()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            Assert.That(str.ToString().Contains("Light is turned off"));
        }

        [Test]
        public void Ready_DoorOpenClose_Ready_PowerIs50()
        {
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);

            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Assert.That(str.ToString().Contains("Display shows: 50 W"));
        }

        [Test]
        public void Ready_2PowerButton_PowerIs100()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            Assert.That(str.ToString().Contains("Display shows: 100 W"));
        }


        [Test]
        public void SetPower_CancelButton_DisplayCleared()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(str.ToString().Contains("Display cleared"));
        }


        [Test]
        public void SetPower_DoorOpened_LightOn()
        {
            // Also checks if TimeButton is subscribed
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            door.Opened += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(str.ToString().Contains("Light is turned on"));
        }

        [Test]
        public void SetPower_TimeButton_TimeIs1()
        {
            // Also checks if TimeButton is subscribed
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(str.ToString().Contains("Display shows: 01:00"));
        }

        [TestCase(1)]
        [TestCase(13)]
        public void Ready_PowerAndTime_cookControllerIsCalledCorrectly(int j)
        {
            for (int i = 0; i < j; i++)
            {
                powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }

            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(str.ToString().Contains($"PowerTube works with {50*j}"));
        }

        [Test]
        public void Cooking_DoorIsOpened_cookControllerCalled()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            door.Opened += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(str.ToString().Contains("PowerTube turned off"));
        }
    }
}
