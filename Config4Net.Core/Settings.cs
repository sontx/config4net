using System;
using Config4Net.Core.Manager;

namespace Config4Net.Core
{
    /// <summary>
    ///     Settings for <see cref="Config" />.
    /// </summary>
    public class Settings : ConfigDataManagerSettings
    {
        private readonly ApplicationClosingEventWrapper _applicationClosingEventWrapper =
            new ApplicationClosingEventWrapper();

        /// <summary>
        ///     The receiver application closing event that should be triggered when the
        ///     application is shutting down.
        /// </summary>
        public IApplicationClosingEvent ApplicationClosingEvent
        {
            get => _applicationClosingEventWrapper?.ApplicationClosingEvent;
            set => _applicationClosingEventWrapper.ApplicationClosingEvent = value;
        }

        /// <summary>
        ///     Auto register type whenever there have a request configuration data from an unkown type.
        /// </summary>
        public bool AutoRegisterConfigType { get; set; }

        /// <summary>
        ///     Configuration data to files when application is closing automatically.
        /// </summary>
        public bool SaveWhenApplicationClosing { get; set; }

        /// <summary>
        ///     If it's true, the library will use app name as a config key as default.
        ///     Otherwise, the library will use config's class name as default.
        /// </summary>
        /// <seealso cref="AppName" />
        public bool PreferAppNameAsKey { get; set; }

        /// <summary>
        ///     The write file timeout in milliseconds.
        /// </summary>
        public int WriteFileTimeout { get; set; }

        /// <summary>
        ///     Gets or sets app name that could be used as default key when the request configuration key
        ///     is missing.
        /// </summary>
        public string AppName { get; set; }

        internal void SetOnApplicationClosing(Action onApplicationClosing)
        {
            _applicationClosingEventWrapper.OnApplicationClosing = onApplicationClosing;
        }

        internal void Release()
        {
            _applicationClosingEventWrapper.ApplicationClosingEvent?.Unregister();
        }

        private class ApplicationClosingEventWrapper
        {
            private IApplicationClosingEvent _applicationClosingEvent;
            public Action OnApplicationClosing { get; set; }

            public IApplicationClosingEvent ApplicationClosingEvent
            {
                get => _applicationClosingEvent;
                set
                {
                    _applicationClosingEvent = value;
                    if (_applicationClosingEvent != null)
                    {
                        _applicationClosingEvent.AppClosing -= ApplicationClosingEventOnAppClosing;
                        _applicationClosingEvent.Unregister();
                    }

                    _applicationClosingEvent = value;

                    if (_applicationClosingEvent != null)
                    {
                        _applicationClosingEvent.AppClosing += ApplicationClosingEventOnAppClosing;
                        _applicationClosingEvent.Register();
                    }
                }
            }

            private void ApplicationClosingEventOnAppClosing(object sender, EventArgs eventArgs)
            {
                OnApplicationClosing?.Invoke();
            }
        }
    }
}