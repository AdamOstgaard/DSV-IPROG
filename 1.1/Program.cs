using System;
using System.Threading;
using System.Threading.Tasks;

namespace IPROG.Uppgifter.uppg1_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var t1 = new T1();
            t1.Start();
            Thread.Sleep(TimeSpan.FromSeconds(5));

            var t2 = new T2();
            t2.Start();
            Thread.Sleep(TimeSpan.FromSeconds(5));

            t1.Stop();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            t2.Stop();
        }
    }

    internal class T1
    {
        private bool IsStopping { get; set; }
        private Thread WorkThread { get; set; }

        /// <summary>
        /// Start the work thread on this object.
        /// </summary>
        public void Start() {
            IsStopping = false;
            WorkThread = new Thread(DoWork);
            WorkThread.Start();
        }

        /// <summary>
        /// Stop the execution of the work thread and wait for it to stop.
        /// </summary>
        public void Stop()
        { 
            IsStopping = true;
            WorkThread?.Join();
        }

        private void DoWork()
        {
            while (!IsStopping) {
                Console.WriteLine("Tråd 1");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }

    internal class T2
    {
        private bool IsStopping { get; set; }
        private Task WorkTask { get; set; }

        /// <summary>
        /// Start the work thread on this object.
        /// </summary>
        public void Start()
        {
            IsStopping = false;
            WorkTask = new Task(DoWork);
            WorkTask.Start();
        }

        /// <summary>
        /// Stop the execution of the work thread and wait for it to stop.
        /// </summary>
        public void Stop()
        {
            IsStopping = true;
            WorkTask?.Wait();
        }

        private void DoWork()
        {
            while (!IsStopping)
            {
                Console.WriteLine("Tråd 2");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
