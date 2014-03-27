using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Interface;

namespace ThermoMeter.TemperatureScale
{
    /// <summary>
    /// Celsius type
    /// </summary>
    public class Celsius : ScaledValueBase
    {
        public Celsius() : base()
        {
        }

        public Celsius(double value)
            : base(value)
        {
        }

        public override double CelsiusValue{
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        public override string ScaleDisplayName
        {
            get
            {
                return "Celsius";
            }
        }

    }
}
