using System.Runtime.InteropServices;

namespace HomeCenter.Services
{
    public static class PowerUtils
    {
        public static void Hibernate()
            => SetSuspendState(true, true, true);

        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);
    }
}
