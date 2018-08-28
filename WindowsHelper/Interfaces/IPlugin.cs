using System;

namespace WindowsHelper.Interfaces
{
    public interface IPlugin : IDisposable
    {
        string Name { get; }
        ISettings Settings { get; }

        bool Init();
        void DeInit();
    }
}