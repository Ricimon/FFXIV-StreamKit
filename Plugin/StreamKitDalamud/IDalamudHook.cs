using System;

namespace StreamKitDalamud
{
    public interface IDalamudHook : IDisposable
    {
        void HookToDalamud();
    }
}
