using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ThermoMeterTest.Mock;
using ThermoMeter.TemperatureScale;
using ThermoMeter.Core;
using ThermoMeter.Interface;
using System.Threading;

namespace ThermoMeterTest.UnitTests
{
    [TestFixture]
    public class FahrenheitMeterTest
    {
        private MMeterConsumer mockConsumer;
        private MInputReader<Celsius> mockInputReader;
        private ThermoMeter<Fahrenheit> mockMeter;

        [SetUp]
        public void Init()
        {
            mockConsumer = new MMeterConsumer();
            mockMeter = new ThermoMeter<Fahrenheit>(mockConsumer, new Fahrenheit(32), new Celsius(100), new Celsius(1), 200);
        }

       
        [Test]
        public void MixedSimpleEventTest()
        {
            mockInputReader = new MInputReader<Celsius>();
            mockInputReader.ValuesToRead.Enqueue(new Celsius(0));
            mockInputReader.ValuesToRead.Enqueue(new Celsius(100));
            mockMeter.InputReader = mockInputReader;
            StartMeterHelper();

            Assert.AreEqual(2, mockConsumer.Values.Count);
            Assert.AreEqual(1, mockConsumer.LowerThresholdWarnings.Count);
            Assert.AreEqual(1, mockConsumer.UpperThresholdWarnings.Count);
        }

        [Test]
        public void UpperWarningWithFluctuation()
        {
            List<double> values = new List<double>
            {
                98.1,100,99.2,100,101,100,102,100,99,100,101
            };
            foreach (double value in values)
            {
                mockInputReader.ValuesToRead.Enqueue(new Celsius(value));
            }
            mockMeter.InputReader = mockInputReader;
            StartMeterHelper();

            Assert.AreEqual(2, mockConsumer.UpperThresholdWarnings.Count);
        }

        [Test]
        public void LowerWarningWithFluctuation()
        {
            mockInputReader = new MInputReader<Celsius>();
            List<double> values = new List<double>
            {
                2.0,1.0,1.0,0.0,-1,0.5,1.0,0
            };
            foreach (double value in values)
            {
                mockInputReader.ValuesToRead.Enqueue(new Celsius(value));
            }

            mockMeter.InputReader = mockInputReader;
            StartMeterHelper();

            Assert.AreEqual(1, mockConsumer.LowerThresholdWarnings.Count);
        }

        [Test]
        public void LowerWarningWithFluctuation_FromRequirement()
        {
            List<double> values = new List<double>
            {
                1.5,1.0,0.5,0.0,-0.5,0.0,-0.5,0.0,0.5,0.0
            };
            foreach (double value in values)
            {
                mockInputReader.ValuesToRead.Enqueue(new Celsius(value));
            }

            mockMeter.Fluctuation = new Celsius(0.5);
            mockMeter.InputReader = mockInputReader;
            StartMeterHelper();

            Assert.AreEqual(1, mockConsumer.LowerThresholdWarnings.Count);
        }

        private void StartMeterHelper()
        {
            mockMeter.StartMeter();
            while (mockMeter.isStarted)
            {
                if (!mockMeter.InputReader.IsReady)
                {
                    mockMeter.StopMeter();
                }
            }
            Thread.Sleep(mockMeter.ReadInterval * mockInputReader.ValuesToRead.Count);    //Wait a little bit for the observer to finish
        }
    }
}

