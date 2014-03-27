using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThermoMeter.Interface
{
    /// <summary>
    /// Generic type for scales
    /// </summary>
    public interface IScaledValue
    {
        double Value { get; set; }
        double CelsiusValue{get; set;}
        IScaledValue Convert<T>() where T : IScaledValue;
        int Compare(IScaledValue value);
        DateTime RecordedDateTime { get; set; }
        String ScaleDisplayName { get; }
    }
}
