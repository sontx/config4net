using System;

namespace Config4Net.Core
{
    public interface IApplicationClosingEvent
    {
        event EventHandler AppClosing;
        void Register();
        void Unregister();
    }

    public class DefaultApplicationClosingEvent : IApplicationClosingEvent
    {
        public event EventHandler AppClosing;

        public void Register()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            AppClosing?.Invoke(this, e);
        }

        public void Unregister()
        {
            AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
        }
    }
}