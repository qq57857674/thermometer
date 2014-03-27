using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThermoMeter.Interface;
using ThermoMeter.TemperatureScale;
using System.Threading;

namespace ThermoMeter.Core
{
    public class ThermoMeter<TScaledValue> where TScaledValue : IScaledValue
    {
        public ThermoMeter(IMeterObserver observer)
        {
            _meterEventManager = new EventManager(observer);
            _inputReaderCTS = new CancellationTokenSource();
        }

        public ThermoMeter(IMeterObserver observer, IInputReader<IScaledValue> reader)
        {
            _meterEventManager = new EventManager(observer);
            InputReader = reader;
            _inputReaderCTS = new CancellationTokenSource();
        }

        public ThermoMeter(IMeterObserver observer, IScaledValue lowerThreshold, IScaledValue upperThreshold, IScaledValue fluctuation, int readInterval)
        {
            _meterEventManager = new EventManager(observer,lowerThreshold, upperThreshold, fluctuation, readInterval);
            _inputReaderCTS = new CancellationTokenSource();
        }

        /// <summary>
        /// Starts the meter
        /// </summary>
        /// <returns>if the meter started correctly</returns>
        public bool StartMeter()
        {
            _inputReaderTask = Task.Factory.StartNew(() => ReadFromInput(), _inputReaderCTS.Token);
            isStarted = _inputReaderTask.Status == TaskStatus.Running;
            return isStarted;
        }

        /// <summary>
        /// Stop the meter
        /// </summary>
        /// <returns>if the meter stopped correctly</returns>
        public bool StopMeter()
        {
            _inputReaderCTS.Cancel();
            _inputReaderTask.Wait();
            isStarted = _inputReaderTask.Status == TaskStatus.Running;
            return _inputReaderTask.Status == TaskStatus.RanToCompletion;
        }

        /// <summary>
        /// Keep the meter "alive". Constantly reading from input source
        /// Runs on a seperate thread
        /// </summary>
        public void ReadFromInput()
        {
            while (true)
            {
                if (_inputReaderCTS.IsCancellationRequested)
                {
                    //If cancel signal is sent, stop the loop
                    break;
                }

                //Check if the input is ready, if so, read a value
                if (InputReader.IsReady)
                {
                    IScaledValue value = InputReader.Read();
                    if (value != null)
                    {
                        if (!typeof(ValueType).Equals(typeof(TScaledValue)))
                        {
                            value = value.Convert<TScaledValue>();
                        }
                        _meterEventManager.ReadValue(value);
                        
                    }
                }
                Thread.Sleep(_meterEventManager.ReadInterval);
            }
        }

        public IMeterObserver Observer{
            get{
                return _meterEventManager.Observer;
            }
            set{
                _meterEventManager.Observer = value;
            }
        }

        public IScaledValue LowerThreshold
        {
            get
            {
                return _meterEventManager.LowerThreshold;
            }
            set
            {
                _meterEventManager.LowerThreshold = value;
            }
        }

        public IScaledValue UpperThreshold
        {
            get
            {
                return _meterEventManager.UpperThreshold;
            }
            set
            {
                _meterEventManager.UpperThreshold = value;
            }
        }

        public IScaledValue Fluctuation
        {
            get
            {
                return _meterEventManager.Fluctuation;
            }
            set
            {
                _meterEventManager.Fluctuation = value;
            }
        }

        public int ReadInterval
        {
            get
            {
                return _meterEventManager.ReadInterval;
            }
            set
            {
                _meterEventManager.ReadInterval = value;
            }
        }

        #region Variables
        private EventManager _meterEventManager { get; set; }
        public bool isStarted{get;set;}
        public IInputReader<IScaledValue> InputReader{get; set;}
        private Task _inputReaderTask;
        private CancellationTokenSource _inputReaderCTS;
        #endregion
    }
}
