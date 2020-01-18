using System;
using StardewModdingAPI;

namespace StardewScript.Framework
{
    public interface IScript: IDisposable
    {
        void Initialize(IModHelper helper, IMonitor monitor);
        void DeInitialize();
        void SetEmergencyShutdownCallback(string scriptFilename, Action<string, Exception> callback);
    }
}