using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Interface;
using ThermoMeter.Core;

namespace ThermoMeterTest.Mock
{
    /// <summary>
    /// Mock of a consumer of the thermometer
    /// whenever it receives a warning, it will add the temperature
    /// caused the warning into a List accordingly, so it's easy for validation
    /// </summary>
    public class MMeterConsumer : IMeterObserver
    {
        public MMeterConsumer()
        {
            Values = new List<IScaledValue>();
            LowerThresholdWarnings = new List<IScaledValue>();
            UpperThresholdWarnings = new List<IScaledValue>();
            
        }

        public List<IScaledValue> Values;
        public List<IScaledValue> LowerThresholdWarnings;
        public List<IScaledValue> UpperThresholdWarnings;

        public void ValueReadHandle(IScaledValue value)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("{0 : yyyy-MM-dd hh:mm} Temperature in {1} is: {2}", value.RecordedDateTime, value.ScaleDisplayName, Math.Round(value.Value, 3)));
            Values.Add(value);
        }

        public void UpperThresholdReachedHandle(IScaledValue value)
        {
            System.Diagnostics.Debug.WriteLine("Upper limit reached!");
            UpperThresholdWarnings.Add(value);
        }

        public void LowerThresholdReachedHandle(IScaledValue value)
        {
            System.Diagnostics.Debug.WriteLine("Lower limit reached!");
            LowerThresholdWarnings.Add(value);
        }

    }
}
