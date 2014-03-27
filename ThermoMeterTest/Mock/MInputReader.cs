using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Interface;
using ThermoMeter.TemperatureScale;

namespace ThermoMeterTest.Mock
{
    /// <summary>
    /// A Mock of input source
    /// It will read from a pre-defined queue of temperatures
    /// </summary>
    /// <typeparam name="TScaledValue"></typeparam>
    public class MInputReader<TScaledValue> : IInputReader<IScaledValue> 
        where TScaledValue : IScaledValue
    {
        public MInputReader()
        {
            ValuesToRead = new Queue<TScaledValue>();
        }

        public Queue<TScaledValue> ValuesToRead;

        public IScaledValue Read()
        {
            if (ValuesToRead.Count <= 0)
            {
                return null;
            }
            return ValuesToRead.Dequeue();
        }
        public bool IsReady
        {
            get{
                return ValuesToRead.Count > 0;
            }
            set{
            }
        }

    }
}
