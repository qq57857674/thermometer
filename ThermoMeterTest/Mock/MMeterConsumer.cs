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

        public void ValueReadHandle(object sender, MeterEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("{0 : yyyy-MM-dd hh:mm} Temperature in {1} is: {2}", args.TemperatureReading.RecordedDateTime, args.TemperatureReading.ScaleDisplayName, Math.Round(args.TemperatureReading.Value, 3)));
            Values.Add(args.TemperatureReading);
        }

        public void UpperThresholdReachedHandle(object sender, MeterEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine(args.WarningMessage);
            UpperThresholdWarnings.Add(args.TemperatureReading);
        }

        public void LowerThresholdReachedHandle(object sender, MeterEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine(args.WarningMessage);
            LowerThresholdWarnings.Add(args.TemperatureReading);
        }

    }
}
