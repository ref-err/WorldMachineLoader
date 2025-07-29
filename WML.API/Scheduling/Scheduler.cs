using System;
using System.Collections.Generic;
using WorldMachineLoader.API.Interfaces;

namespace WorldMachineLoader.API.Scheduling
{
    public class Scheduler : IScheduler
    {
        private readonly List<ScheduledTask> _tasks = new List<ScheduledTask>();

        private enum ScheduleType
        {
            Once,
            Repeating,
            RepeatingLimited
        }

        private class ScheduledTask
        {
            public ScheduleType Type;
            public TimeSpan Delay;
            public TimeSpan Interval;
            public TimeSpan Elapsed;
            public TimeSpan Duration;
            public TimeSpan ExecutedTime;
            public Action Action;
        }

        public void RunAfter(TimeSpan delay, Action callback)
        {
            _tasks.Add(new ScheduledTask
            {
                Type = ScheduleType.Once,
                Delay = delay,
                Interval = TimeSpan.Zero,
                Action = callback,
                Elapsed = TimeSpan.Zero
            });
        }

        public void RunEvery(TimeSpan interval, Action callback)
        {
            _tasks.Add(new ScheduledTask
            {
                Type = ScheduleType.Repeating,
                Delay = interval,
                Interval = interval,
                Action = callback,
                Elapsed = TimeSpan.Zero
            });
        }

        public void RunFor(TimeSpan duration, TimeSpan delay, Action callback)
        {
            _tasks.Add(new ScheduledTask
            {
                Type = ScheduleType.RepeatingLimited,
                Delay = delay,
                Interval = delay,
                Action = callback,
                Elapsed = TimeSpan.Zero,
                Duration = duration,
                ExecutedTime = TimeSpan.Zero
            });
        }

        public void Update(TimeSpan delta)
        {
            for (int i = _tasks.Count - 1; i >= 0; i--)
            {
                var task = _tasks[i];
                task.Elapsed += delta;

                if (task.Elapsed >= task.Delay)
                {
                    task.Action?.Invoke();

                    switch (task.Type)
                    {
                        case ScheduleType.Once:
                            _tasks.RemoveAt(i);
                            break;

                        case ScheduleType.Repeating:
                            task.Elapsed -= task.Interval;
                            break;

                        case ScheduleType.RepeatingLimited:
                            task.ExecutedTime += task.Interval;
                            if (task.ExecutedTime >= task.Duration)
                                _tasks.RemoveAt(i);
                            else
                                task.Elapsed -= task.Interval;
                                break;
                    }
                }
            }
        }
    }
}
