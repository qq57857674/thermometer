using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Interface;

namespace ThermoMeter.TemperatureScale
{
    /// <summary>
    /// Fahrenheit type
    /// Conversion from Farenheit to celsius is handled from here
    /// </summary>
    public class Fahrenheit : ScaledValueBase
    {
        public Fahrenheit() : base()
        {
        }

        public Fahrenheit(double value)
            : base(value)
        {
        }

        public override double CelsiusValue
        {
            get
            {
                return Math.Round((Value - 32) * 5.0 / 9.0, 3);
            }
            set
            {
                Value = Math.Round(value * 9.0 / 5.0 + 32, 3);
            }
        }

        public override string ScaleDisplayName
        {
            get
            {
                return "Fahrenheit";
            }
        }
    }
}
