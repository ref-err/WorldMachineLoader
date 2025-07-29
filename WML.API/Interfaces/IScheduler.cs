using System;

namespace WorldMachineLoader.API.Interfaces
{
    internal interface IScheduler
    {
        void RunAfter(TimeSpan delay, Action callback);
        void RunEvery(TimeSpan interval, Action callback);
        void RunFor(TimeSpan duration, TimeSpan delay, Action callback);
    }
}
