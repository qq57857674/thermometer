using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Interface;

namespace ThermoMeter.TemperatureScale
{
    public class ScaledValueFactory
    {
        public static T createScaledValue<T>() where T : IScaledValue
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
