using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VideoOS.Platform.DriverFramework.Definitions;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;

namespace BeiaDeviceDriver
{
    /// <summary>
    /// This is an example of how events can be retrieved from a hardware supporting events through polling.
    /// TODO: Implement the polling request and update the list of supported events. 
    /// </summary>
    public class InputPoller
    {
        private IEventManager _eventManager;
        private bool _shuttingDown = false;
        private Lazy<Thread> _listenerThread;
        private readonly IDictionary<string, Action<string>> _eventHandlers = new Dictionary<string, Action<string>>();
        private readonly BeiaDeviceDriverConnectionManager _connectionManager;

        public InputPoller(IEventManager eventManager, BeiaDeviceDriverConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            _eventManager = eventManager;
            InitEventHandlers();
        }

        public void Start()
        {
            Toolbox.Log.Trace("Input poller starting");

            _listenerThread = new Lazy<Thread>(() => new Thread(CommunicationThreadMainLoop)
            {
                Name = string.Format(System.Globalization.CultureInfo.InvariantCulture, "BeiaDeviceDriver driver listener thread for input events"),
            }, LazyThreadSafetyMode.ExecutionAndPublication);

            _listenerThread.Value.Start();
        }

        public void Stop()
        {
            Toolbox.Log.Trace("Input poller stopping");
            _shuttingDown = true;
        }

        private void CommunicationThreadMainLoop()
        {
            while (!_shuttingDown)
            {
                try
                {
                    string[] events = null;

                    // TODO: Make request to retrieve events from hardware

                    if (events != null && events.Any())
                    {
                        ProcessEvents(events);

                    }
                    Thread.Sleep(250);      // We check every 250 ms
                }
                catch (Exception e)
                {
                    Toolbox.Log.LogError("Input poller", e.StackTrace);
                    Thread.Sleep(3000);
                }
            }
        }

        private void InitEventHandlers()
        {
            _eventHandlers.Clear();
            // TODO: Modify the list to match what is supported by your hardware
            _eventHandlers.Add("Input_Activate_1" /* TODO: Change to match what is provided by device */, eventId =>
            {
                _eventManager.NewEvent(Constants.Input1.ToString(), EventId.InputActivated);
            });
            _eventHandlers.Add("Input_Dectivate_1" /* TODO: Change to match what is provided by device */, eventId =>
            {
                _eventManager.NewEvent(Constants.Input1.ToString(), EventId.InputDeactivated);
            });
        }

        private void ProcessEvents(string[] events)
        {
            foreach (string receivedEvent in events)
            {
                Action<string> action = GetAction(receivedEvent);
                action?.Invoke(receivedEvent);
            }
        }

        private Action<string> GetAction(string receivedEvent)
        {
            Action<string> action;
            if (_eventHandlers.TryGetValue(receivedEvent, out action))
            {
                return action;
            }
            return null;
        }
    }
}
