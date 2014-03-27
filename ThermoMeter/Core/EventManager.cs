using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Interface;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace ThermoMeter.Core
{
    /// <summary>
    /// Take care of the logic when and why to send a notification to the client.
    /// </summary>
    internal class EventManager
    {
        public EventManager(IMeterObserver observer)
        {
            Observer = observer;
            _isLowerFluctuating = false;
            _isUpperFluctuating = false;
        }

        public EventManager(IMeterObserver observer, IScaledValue lowerThreshold, IScaledValue upperThreshold, IScaledValue fluctuation, int readInterval)
        {
            Observer = observer;
            LowerThreshold = lowerThreshold;
            UpperThreshold = upperThreshold;
            Fluctuation = fluctuation;
            ReadInterval = readInterval;

            _isLowerFluctuating = false;
            _isUpperFluctuating = false;
        }

        /// <summary>
        /// When a new value is read, process the value
        /// </summary>
        /// <param name="value">a value which were just read from input</param>
        public void ReadValue(IScaledValue value)
        {
            OnValueRead(value);
            updateFluctuationInfo(value);

            //Determine if the value is outside of the thresholds,
            //as well as whether all the values read since last warning notification
            //are all within the flunctuation range (determined by _isLowerFluctuating
            //and _isUpperFluctuating flags). If the above condition meets, fire a new warning
            if (value.Compare(LowerThreshold) <= 0)
            {
                //if less or equal to lower limit, we have a problem.
                if (!_isLowerFluctuating && (_lastReadValue == null || _lastReadValue.Compare(LowerThreshold) > 0))
                {
                    OnLowerThresholdReached(value);
                    _isLowerFluctuating = true;
                }
            }
            else if (value.Compare(UpperThreshold) >= 0)
            {
                //if more or equal to upper limit, we have a problem too.
                if (!_isUpperFluctuating && (_lastReadValue == null ||  _lastReadValue.Compare(UpperThreshold) < 0))
                {
                    OnUpperThresholdReached(value);
                    _isUpperFluctuating = true;
                }
            }
            else
            {
                //else we are fine.
            }
            _lastReadValue = value;
        }

        /// <summary>
        /// check if this value is within the fluctuation range
        /// and update the flags accordingly.
        /// </summary>
        /// <param name="value">a value which were just read from input</param>
        private void updateFluctuationInfo(IScaledValue value)
        {
            if (value.CelsiusValue < LowerThreshold.CelsiusValue - Fluctuation.CelsiusValue ||
                value.CelsiusValue > LowerThreshold.CelsiusValue + Fluctuation.CelsiusValue)
            {
                _isLowerFluctuating = false;
            }

            if (value.CelsiusValue < UpperThreshold.CelsiusValue - Fluctuation.CelsiusValue ||
                value.CelsiusValue > UpperThreshold.CelsiusValue + Fluctuation.CelsiusValue)
            {
                _isUpperFluctuating = false;
            }
        }

        /// <summary>
        /// Fire a value read notification
        /// Delegate the handler from client.
        /// </summary>
        /// <param name="value"></param>
        public void OnValueRead(IScaledValue value)
        {
            if(Observer != null)
            {
                MeterEventArgs args = new MeterEventArgs(value);
                EventHandler<MeterEventArgs> handler = ValueUpdated;
                if (handler != null)
                {
                    handler(this, args);
                }
            }
        }

        /// <summary>
        /// Fire a Lower Threshold Reached notification
        /// Delegate the handler from client.
        /// </summary>
        /// <param name="value"></param>
        public void OnLowerThresholdReached(IScaledValue value)
        {
            if (Observer != null)
            {
                MeterEventArgs args = new MeterEventArgs(value, "Lower Threshold Reached!");
                EventHandler<MeterEventArgs> handler = LowerThresholdReached;
                if (handler != null)
                {
                    handler(this, args);
                }
            }
        }

        /// <summary>
        /// Fire a Upper Threshold Reached notification
        /// Delegate the handler from client.
        /// </summary>
        /// <param name="value"></param>
        public void OnUpperThresholdReached(IScaledValue value)
        {
            if (Observer != null)
            {
                MeterEventArgs args = new MeterEventArgs(value, "Upper Threshold Reached!");
                EventHandler<MeterEventArgs> handler = UpperThresholdReached;
                if (handler != null)
                {
                    handler(this, args);
                }
            }
        }

        //Event handlers where the client-side delegates are stored
        private event EventHandler<MeterEventArgs> ValueUpdated;
        private event EventHandler<MeterEventArgs> LowerThresholdReached;
        private event EventHandler<MeterEventArgs> UpperThresholdReached;

        //configurations and variables
        #region Variables
        private IScaledValue _lastReadValue { get; set; }
        public IScaledValue LowerThreshold{
            get
            {
                return _lowerThreshold;
            }
            set
            {
                if (UpperThreshold == null || value.Compare(UpperThreshold) < 0)
                {
                    _lowerThreshold = value;
                }
            }
        }
        public IScaledValue UpperThreshold
        {
            get
            {
                return _upperThreshold;
            }
            set
            {
                if (LowerThreshold == null || value.Compare(LowerThreshold) > 0)
                {
                    _upperThreshold = value;
                }
            }
        }
        public IScaledValue Fluctuation{get; set;}
        public IMeterObserver Observer { 
            get{
                return _observer;
            }
            set {
                //if there was already a client listening to this meter
                //remove its handlers first
                if (_observer != null)
                {
                    ValueUpdated -= _observer.ValueReadHandle;
                    LowerThresholdReached -= _observer.LowerThresholdReachedHandle;
                    UpperThresholdReached -= _observer.UpperThresholdReachedHandle;
                }

                _observer = value;
                ValueUpdated += _observer.ValueReadHandle;
                LowerThresholdReached += _observer.LowerThresholdReachedHandle;
                UpperThresholdReached += _observer.UpperThresholdReachedHandle;
            }
        }
        public int ReadInterval = 3000;
        private bool _isLowerFluctuating;
        private bool _isUpperFluctuating;
        private IScaledValue _lowerThreshold;
        private IScaledValue _upperThreshold;
        private IMeterObserver _observer;
        #endregion
    }
}
