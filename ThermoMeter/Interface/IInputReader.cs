using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Core;

namespace ThermoMeter.Interface
{
    /// <summary>
    /// Interface for any input source
    /// </summary>
    /// <typeparam name="TScaledValue"></typeparam>
    public interface IInputReader<TScaledValue>
        where TScaledValue : IScaledValue
    {
        TScaledValue Read();
        bool IsReady{get;}
    }
}
