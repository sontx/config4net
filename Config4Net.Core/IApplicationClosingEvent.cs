using System;

namespace Config4Net.Core
{
    /// <summary>
    /// A listener that listens to app closing event.
    /// </summary>
    public interface IApplicationClosingEvent
    {
        /// <summary>
        /// Trigger when app is closing.
        /// </summary>
        event EventHandler AppClosing;

        /// <summary>
        /// Register to the system to start listening app closing event.
        /// </summary>
        void Register();

        /// <summary>
        /// Unregister to the system.
        /// </summary>
        void Unregister();
    }

    /// <summary>
    /// Default implementation for <see cref="IApplicationClosingEvent"/> that listens to
    /// <see cref="AppDomain.ProcessExit"/> event from <see cref="AppDomain.CurrentDomain"/> 
    /// to know when the app is closing.
    /// </summary>
    public class DefaultApplicationClosingEvent : IApplicationClosingEvent
    {
        /// <inheritdoc />
        public event EventHandler AppClosing;

        /// <inheritdoc />
        public void Register()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            AppClosing?.Invoke(this, e);
        }

        /// <inheritdoc />
        public void Unregister()
        {
            AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
        }
    }
}