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
        void ValueReadHandle(IScaledValue value);
        void UpperThresholdReachedHandle(IScaledValue value);
        void LowerThresholdReachedHandle(IScaledValue value);
    }
}
