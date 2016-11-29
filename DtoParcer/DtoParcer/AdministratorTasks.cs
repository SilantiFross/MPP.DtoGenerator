using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using DtoParcer.GenerationUnits.Components;

namespace DtoParcer
{
    internal class AdministratorTasks
    {
        private readonly Queue<Class> _taskQueue = new Queue<Class>();
        private readonly object _syncRoot = new object();
        private int _runningTasksCount;
        private readonly int _maxTasksCount;

        private bool HasTasks => _taskQueue.Count > 0;
        private bool CanAddToPool => _runningTasksCount < _maxTasksCount;

        public AdministratorTasks(int maxTasksCount)
        {
            _runningTasksCount = 0;
            _maxTasksCount = maxTasksCount;
            _taskQueue.Clear();
        }

        public void GenerateCsClass(ConcurrentQueue<StringBuilder> generatedCsClasses, Class adddedClass, CountdownEvent countdownEvent, Func<Class, StringBuilder> parceFunc)
        {
            lock (_syncRoot)
            {
                if (CanAddToPool)
                {
                    AddToPool(generatedCsClasses, adddedClass, countdownEvent, parceFunc);
                }
                else
                {
                    _taskQueue.Enqueue(adddedClass);
                }
            }
        }

        public void WaitAllTasks(CountdownEvent countdownEvent)
        {
            countdownEvent.Wait();
        }

        private void AddToPool(ConcurrentQueue<StringBuilder> generatedCsClasses, Class addedClass, CountdownEvent countdownEvent, Func<Class, StringBuilder> parceFunc)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                generatedCsClasses.Enqueue(parceFunc(addedClass));
                OnTaskFinish(generatedCsClasses, countdownEvent, parceFunc);
            });
            _runningTasksCount++;
        }

        private void OnTaskFinish(ConcurrentQueue<StringBuilder> generatedCsClasses, CountdownEvent countdownEvent, Func<Class, StringBuilder> parceFunc)
        {
            lock (_syncRoot)
            {
                _runningTasksCount--;

                if (HasTasks)
                {
                    var adddedClass = _taskQueue.Dequeue();
                    AddToPool(generatedCsClasses, adddedClass, countdownEvent, parceFunc);
                }
            }
            countdownEvent.Signal();
        }
    }
}
