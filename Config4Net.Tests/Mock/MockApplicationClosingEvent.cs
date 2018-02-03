using Config4Net.Core;
using System;

namespace Config4Net.Tests.Mock
{
    internal class MockApplicationClosingEvent : IApplicationClosingEvent
    {
        public event EventHandler AppClosing;

        public void Register()
        {
        }

        public void SimulateEvent()
        {
            AppClosing?.Invoke(this, EventArgs.Empty);
        }

        public void Unregister()
        {
        }
    }
}