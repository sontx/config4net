using System;

namespace Config4Net.Core
{
    public class Settings
    {
        private readonly ApplicationClosingEventWrapper _applicationClosingEventWrapper = new ApplicationClosingEventWrapper();

        /// <summary>
        /// The receiver application closing event that should be triggered when the
        /// application is shutting down.
        /// </summary>
        public IApplicationClosingEvent ApplicationClosingEvent
        {
            get => _applicationClosingEventWrapper?.ApplicationClosingEvent;
            set => _applicationClosingEventWrapper.ApplicationClosingEvent = value;
        }

        /// <summary>
        /// Auto register type whenever there have a request configuration data from an unkown type.
        /// </summary>
        public bool AutoRegisterConfigType { get; set; }

        /// <summary>
        /// Auto save configuration data to files when application is closing.
        /// Configuration will be saved into <see cref="ConfigDir"/> automatically.
        /// </summary>
        public bool AutoSaveWhenApplicationClosing { get; set; }

        /// <summary>
        /// Ignore mismatch type, the property that is mismatch type will be assigned
        /// default value.
        /// </summary>
        public bool IgnoreMismatchType { get; set; }

        /// <summary>
        /// The library will create new instance for null properties to prevent null reference.
        /// </summary>
        public bool PreventNullReference { get; set; }

        /// <summary>
        /// The directory that will be held configuration files. If it's null or empty,
        /// library will use current directory instead.
        /// </summary>
        public string ConfigDir { get; set; }

        /// <summary>
        /// The extenstion of configuration file that store configuration data. These files
        /// will be placed in <see cref="ConfigDir"/>
        /// </summary>
        public string ConfigFileExtension { get; set; }

        /// <summary>
        /// The special key for each application. That key belongs to application config which
        /// contains configuration data for whole application.
        /// </summary>
        public string AppConfigKey { get; set; }

        /// <summary>
        /// The write file timeout in milliseconds.
        /// </summary>
        public int WriteFileTimeout { get; set; }

        internal void SetOnApplicationClosing(Action onApplicationClosing)
        {
            _applicationClosingEventWrapper.OnApplicationClosing = onApplicationClosing;
        }

        internal void Release()
        {
            _applicationClosingEventWrapper.ApplicationClosingEvent?.Unregister();
        }
    }

    internal sealed class ApplicationClosingEventWrapper
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