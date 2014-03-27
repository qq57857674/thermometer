using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ThermoMeterTest.Mock;
using ThermoMeter.TemperatureScale;
using ThermoMeter.Core;
using ThermoMeter.Interface;
namespace ThermoMeterTest.UnitTests
{
    [TestFixture]
    public class TemperatureScaleTest
    {
        
        [Test]
        public void CreateCelsiusWithCelsiusValue()
        {
            Random rand = new Random();
            double randDbl = rand.NextDouble() * 100;
            IScaledValue temp = new Celsius();                              
            temp.CelsiusValue = randDbl;                                   //Create a temperature in Celcius and implicitly assign a value in celsius
            Assert.AreEqual(Math.Round(randDbl, 3), Math.Round(temp.CelsiusValue, 3));
        }

        [Test]
        public void ReadCelsiusAsCelsius()
        {
            Random rand = new Random();
            double randDbl = rand.NextDouble() * 100;
            IScaledValue temp = new Celsius(randDbl);                      //Set temperature with value in celcius
            Assert.AreEqual(Math.Round(randDbl, 3), Math.Round(temp.CelsiusValue, 3));
        }

        [Test]
        public void CreateFahrenheitWithCelsiusValue()
        {
            Random rand = new Random();
            double randDbl = rand.NextDouble() * 100;
            IScaledValue temp = new Fahrenheit();
            temp.CelsiusValue = randDbl;                                 //Create a temperature in fahrenheit and implicitly assign a value in celsius
            Assert.AreEqual(Math.Round(randDbl, 3), Math.Round(temp.CelsiusValue, 3));                 //The temperature's CelsiusValue should match the value we passed in
            Assert.AreEqual(Math.Round(randDbl * 9.0 / 5.0 + 32, 3), Math.Round(temp.Value, 3));           //The temperature's actual value in Fahrenheit should be correctly converted from celsius
        }


        [Test]
        public void ReadFahrenheitAsCelsius()
        {
            Random rand = new Random();
            double randDbl = rand.NextDouble() * 100;
            IScaledValue temp = new Fahrenheit(randDbl);                  //Set temperature with value in fahrenheit
            Assert.AreEqual(Math.Round((randDbl - 32) * 5.0 / 9.0, 3), Math.Round(temp.CelsiusValue, 3));
        }

        [Test]
        public void CompareBetweenCelsiusTemps()
        {
            Random rand = new Random();
            double randDbl1 = rand.NextDouble() * 100;
            double randDbl2 = rand.NextDouble() * 100;
            IScaledValue temp1 = new Celsius(randDbl1);
            IScaledValue temp2 = new Celsius(randDbl2);

            if (randDbl1 > randDbl2)
            {
                Assert.AreEqual(1, temp1.Compare(temp2));
            }
            else if (randDbl1 == randDbl2)
            {
                Assert.AreEqual(0, temp1.Compare(temp2));
            }
            else
            {
                Assert.AreEqual(-1, temp1.Compare(temp2));
            }
        }

        [Test]
        public void CompareBetweenFahrenheitTemps()
        {
            Random rand = new Random();
            double randDbl1 = rand.NextDouble() * 100;
            double randDbl2 = rand.NextDouble() * 100;
            IScaledValue temp1 = new Fahrenheit(randDbl1);
            IScaledValue temp2 = new Fahrenheit(randDbl2);

            if (randDbl1 > randDbl2)
            {
                Assert.AreEqual(1, temp1.Compare(temp2));
            }
            else if (randDbl1 == randDbl2)
            {
                Assert.AreEqual(0, temp1.Compare(temp2));
            }
            else
            {
                Assert.AreEqual(-1, temp1.Compare(temp2));
            }
        }

        [Test]
        public void CompareBetweenCelsiusAndFahrenheitTemps()
        {
            Random rand = new Random();
            double randDbl1 = rand.NextDouble() * 100;
            double randDbl2 = rand.NextDouble() * 100;
            IScaledValue temp1 = new Celsius(randDbl1);
            //Let's assume both the randomed numbers are in celsius
            //So for temp2 as a Fahrenheit temp, we pass in randDbl2 to CelsiusValue
            IScaledValue temp2 = new Fahrenheit();
            temp2.CelsiusValue = randDbl2;                  

            if (randDbl1 > randDbl2)
            {
                Assert.AreEqual(1, temp1.Compare(temp2));
            }
            else if (randDbl1 == randDbl2)
            {
                Assert.AreEqual(0, temp1.Compare(temp2));
            }
            else
            {
                Assert.AreEqual(-1, temp1.Compare(temp2));
            }
        }

        [Test]
        public void ConvertCelsiusToFahrenheit()
        {
            Random rand = new Random();
            double randDbl = rand.NextDouble() * 100;
            //Create a celsius and a fahrenheit temp with the same value
            //So it's easier for assertion later on
            
            IScaledValue temp1 = new Celsius(randDbl);
            IScaledValue temp2 = new Fahrenheit();
            temp2.CelsiusValue = randDbl;

            Assert.AreEqual(0, temp1.Convert<Fahrenheit>().Compare(temp2));
        }
    }
}

