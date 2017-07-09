using System;
using System.Windows.Forms;

namespace VolumeMonitor
{
    static class Extensions
    {
        public static void Invoke(this Control control, Action action)
        {
            control.Invoke(action);
        }
    }
}
