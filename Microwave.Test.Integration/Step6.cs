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
        private Button uutStartCancelButton; // T
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
            uutStartCancelButton = new Button();

            cookController = new CookController(timer, display, powerTube);
            userInterface = new UserInterface(uutPowerButton, uutTimerButton, uutStartCancelButton, door, display, light, cookController);
            cookController.UI = userInterface;

            str = new StringWriter();
            Console.SetOut(str);
        }

        [Test]
        public void PowerButtonPress_AtStart_PowerTubeStarted()
        {
            uutPowerButton.Press();
            Assert.That(str.ToString().Contains("Display shows: 50 W"));
        }

        


        //[Test]
        //public void TimerButtonPress_AtStart_ShowTime()
        //{
        //    uutTimerButton.Press();
        //    Assert.That(str.ToString().Contains("Display shows: 01:00"));
        //}

        [Test]
        public void TimerButtonPress_DuringSetup_ShowTime()
        {
            uutPowerButton.Press();
            uutTimerButton.Press();
            Assert.That(str.ToString().Contains("Display shows: 01:00"));
        }


        [Test]
        public void StartCancelButtonPress_SetTime_LightIsTurnOn()
        {
            uutPowerButton.Press();
            uutTimerButton.Press();
            uutStartCancelButton.Press();

            Assert.That(str.ToString().Contains("Light is turned on"));
        }

        [Test]
        public void StartCancelButtonPress_SetTime_TurnOnPowertube()
        {
            uutPowerButton.Press();
            uutTimerButton.Press();
            uutStartCancelButton.Press();

            Assert.That(str.ToString().Contains("PowerTube works with 50"));
        }

        [Test]
        public void PowerButtonPress3Times_SetTime_PowerTubeStarted()
        {
            uutPowerButton.Press();
            uutPowerButton.Press();
            uutPowerButton.Press();

            uutTimerButton.Press();
            uutStartCancelButton.Press();
            Assert.That(str.ToString().Contains("PowerTube works with 150"));
        }

        //Ext 1
        [Test]
        public void CancelButtonPress_DuringSetup_PowerTubeOff()
        {
            uutPowerButton.Press();
            uutStartCancelButton.Press();
            Assert.That(str.ToString().Contains("Display cleared"));
        }

        //Ext2
        [Test]
        public void CancelButtonPress_UnderCooking_ClearDisplay()
        {
            uutPowerButton.Press();
            uutTimerButton.Press();
            uutStartCancelButton.Press();

            uutStartCancelButton.Press();
            Assert.That(str.ToString().Contains("Display cleared"));
        }
    }
}
