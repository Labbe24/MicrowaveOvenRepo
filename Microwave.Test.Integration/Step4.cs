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
    public class Step4
    {
        private CookController uut;             //T
        private Output output;                  //X
        private PowerTube powerTube;            //X
        private Display display;                //X
        private IUserInterface userInterface;   //S
        private ITimer timer;                   //S

        private StringWriter str;

        [SetUp]
        public void Setup()
        {
            output = new Output();
            powerTube = new PowerTube(output);
            display = new Display(output);
            userInterface = Substitute.For<IUserInterface>();
            timer = Substitute.For<ITimer>();

            uut = new CookController(timer, display, powerTube, userInterface);

            str = new StringWriter();
            Console.SetOut(str);
        }


        [Test]
        public void StartCooking_ValidParameters_PowerTubeStarted()
        {
            uut.StartCooking(50, 60);
            Assert.That(str.ToString().Contains("PowerTube works with 50"));
        }

        [Test]
        public void Cooking_TimerTick_DisplayCalled()
        {
            uut.StartCooking(50, 60);

            timer.TimeRemaining.Returns(115);
            timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(str.ToString().Contains("Display shows: 1:55"));
        }

        [Test]
        public void Cooking_TimerExpired_PowerTubeOff()
        {
            uut.StartCooking(50, 60);

            timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(str.ToString().Contains("PowerTube turned off"));
        }

        [Test]
        public void Cooking_Stop_PowerTubeOff()
        {
            uut.StartCooking(50, 60);
            uut.Stop();

            Assert.That(str.ToString().Contains("PowerTube turned off"));
        }


    }
}
