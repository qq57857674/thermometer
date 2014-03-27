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
    public class CelsiusMeterTest
    {
        private MMeterConsumer mockConsumer;
        private MInputReader<Celsius> mockInputReader;
        private ThermoMeter<Celsius> mockMeter;
        
        [SetUp]
        public void Init()
        {
            mockConsumer = new MMeterConsumer();
            mockMeter = new ThermoMeter<Celsius>(mockConsumer);
            mockMeter.LowerThreshold = new Celsius(0);
            mockMeter.UpperThreshold = new Celsius(100);
            mockMeter.Fluctuation = new Celsius(1);
            mockMeter.ReadInterval = 500;
            mockInputReader = new MInputReader<Celsius>();
        }

        [Test]
        public void ValueReadEventTest()
        {
            List<double> values = new List<double>
            {
                1.1, 1.1, 1.1, 1.1, 1.1, 1.1, 1.1
            };
            foreach (double value in values)
            {
                mockInputReader.ValuesToRead.Enqueue(new Celsius(value));
            }
            mockMeter.InputReader = mockInputReader;
            StartMeterHelper();
            Assert.AreEqual(7, mockConsumer.Values.Count);
        }

        [Test]
        public void LowerThresholdExceededWarningTest()
        {
            mockInputReader.ValuesToRead.Enqueue(new Celsius(0));
            mockMeter.InputReader = mockInputReader;
            StartMeterHelper();
            Assert.AreEqual(1, mockConsumer.LowerThresholdWarnings.Count);
        }

        [Test]
        public void UpperThresholdExceededWarningTest()
        {
            mockInputReader.ValuesToRead.Enqueue(new Celsius(100));
            mockMeter.InputReader = mockInputReader;
            StartMeterHelper();
            Assert.AreEqual(1, mockConsumer.UpperThresholdWarnings.Count);
        }

        [Test]
        public void MixedSimpleEventTest()
        {
            mockInputReader.ValuesToRead.Enqueue(new Celsius(-2));
            mockInputReader.ValuesToRead.Enqueue(new Celsius(102));
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
            List<double> values = new List<double>
            {
                2.0,1.0,1.0,0.0,-1,0.5,1,0
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

        [Test]
        public void MixedComplexEventTest()
        {
            List<double> values = new List<double>
            {
                4.7,1.2,-0.5,-0.3,0.4,0.9,0.5,0,2,100,101,99.5,99.3,100.4,102,103,99,103,-0.3
            };
            foreach (double value in values)
            {
                mockInputReader.ValuesToRead.Enqueue(new Celsius(value));
            }
            mockMeter.InputReader = mockInputReader;
            StartMeterHelper();

            Assert.AreEqual(19, mockConsumer.Values.Count);
            Assert.AreEqual(2, mockConsumer.LowerThresholdWarnings.Count);
            Assert.AreEqual(2, mockConsumer.UpperThresholdWarnings.Count);
        }
    }
}

