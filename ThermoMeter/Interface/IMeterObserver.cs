using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Core;

namespace ThermoMeter.Interface
{
    /// <summary>
    /// Interface for client
    /// </summary>
    public interface IMeterObserver
    {
        void ValueReadHandle(object sender, MeterEventArgs args);
        void UpperThresholdReachedHandle(object sender, MeterEventArgs args);
        void LowerThresholdReachedHandle(object sender, MeterEventArgs args);
    }
}
