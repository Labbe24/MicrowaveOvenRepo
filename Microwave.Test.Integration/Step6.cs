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
    public class Step6
    {
        private Button uutPowerButton; // T
        private Button uutTimerButton; // T
        private Button uutCancelButton; // T
        private Output output; // X
        private Display display; // X
        private Light light; // X
        private PowerTube powerTube; // X
        private Timer timer; // X
        private CookController cookController; // X
        private UserInterface userInterface; // X
        private Door door; // S

        private StringWriter str;

        [SetUp]
        public void Setup()
        {
            output = new Output();
            display = new Display(output);
            light = new Light(output);
            powerTube = new PowerTube(output);
            timer = Substitute.For<Timer>();
            door = Substitute.For<Door>();

            uutPowerButton = new Button();
            uutTimerButton = new Button();
            uutCancelButton = new Button();

            cookController = new CookController(timer, display, powerTube);
            userInterface = new UserInterface(uutPowerButton, uutTimerButton, uutCancelButton, door, display, light, cookController);
            cookController.UI = userInterface;

            str = new StringWriter();
            Console.SetOut(str);
        }

        [Test]
        public void PowerButtonPress_UserInterfaceSubscribes_PowerTubeStarted()
        {
            uutPowerButton.Press();
            Assert.That(str.ToString().Contains("Display shows: 50 W"));
        }

        [Test]
        public void TimerButtonPress_UserInterfaceSubscribes_ShowTime()
        {
            uutTimerButton.Press();
            Assert.That(str.ToString().Contains("Display shows: 01:00"));
        }

        [Test]
        public void CancelButtonPress_UserInterfaceSubscribes_PowerTubeOff()
        {
            uutCancelButton.Press();
            Assert.That(str.ToString().Contains("Display cleared"));
        }

        [Test]
        public void CancelButtonPress_UnderCooking_ClearDisplay()
        {
            uutPowerButton.Press();
            uutTimerButton.Press();
            uutCancelButton.Press();

            uutCancelButton.Press();
            Assert.That(str.ToString().Contains("Display cleared"));
        }
    }
}
