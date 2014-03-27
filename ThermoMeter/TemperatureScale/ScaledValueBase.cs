
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Interface;

namespace ThermoMeter.TemperatureScale
{
    /// <summary>
    /// Partial implimentation of the generic scale.
    /// All scale types can extend from this.
    /// </summary>
    public abstract class ScaledValueBase : IScaledValue
    {
        public ScaledValueBase()
        {
        }

        public ScaledValueBase(double value)
        {
            Value = value;
        }

        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                if (_datetime.Equals(DateTime.MinValue))
                {
                    _datetime = DateTime.Now;
                }
            }
        }

        public DateTime RecordedDateTime
        {
            get
            {
                return _datetime;
            }
            set
            {
                _datetime = value;
            }
        }
        
        public IScaledValue Convert<T>() where T : IScaledValue
        {
            if (typeof(T).Equals(this.GetType()))
            {
                return this;
            }
            else
            {
                IScaledValue valueWithTargetType = ScaledValueFactory.createScaledValue<T>();
                valueWithTargetType.RecordedDateTime = _datetime;
                valueWithTargetType.CelsiusValue =  this.CelsiusValue;
                return valueWithTargetType;
            }
        }

        public int Compare(IScaledValue value)
        {
           
            if (Math.Round(this.CelsiusValue,3) > Math.Round(value.CelsiusValue,3))
            {
                return 1;
            }
            else if (Math.Round(this.CelsiusValue, 3) == Math.Round(value.CelsiusValue, 3))
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        protected double _value;
        protected DateTime _datetime;

        public abstract double CelsiusValue { get; set; }

        public abstract string ScaleDisplayName { get; }

        public static ScaledValueFactory Factory
        {
            get
            {
                ScaledValueFactory _factory = new ScaledValueFactory();
                return _factory;
            }
        }
    }
}
