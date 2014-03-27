using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Interface;

namespace ThermoMeter.Core
{
    public class MeterEventArgs : EventArgs
    {
        public MeterEventArgs(IScaledValue value)
        {
            TemperatureReading = value;
        }

        public MeterEventArgs(IScaledValue value, String messsage) 
        {
            TemperatureReading = value;
            WarningMessage = messsage;
        }
        public IScaledValue TemperatureReading { get; set;}
        public String WarningMessage { get; set; }
    }
}
